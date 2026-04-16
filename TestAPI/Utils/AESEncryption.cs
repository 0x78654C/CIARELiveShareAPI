using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace TestAPI.Utils
{
    public class AESEncryption
    {
        private static readonly Encoding encoding = Encoding.UTF8;
        private const int Iterations = 600_000;
        private const int SaltSize = 16;
        private const int KeySize = 32;

        /// <summary>
        /// AES-256-CBC encryption with random salt, random IV, and HMAC-SHA256 integrity check.
        /// </summary>
        /// <param name="plainText">String input for encryption.</param>
        /// <param name="password">Master Password</param>
        /// <returns>string</returns>
        public static string Encrypt(string plainText, string password)
        {
            try
            {
                byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
                byte[] key = DeriveKey(password, salt);

                using var aes = Aes.Create();
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Padding = PaddingMode.PKCS7;
                aes.Mode = CipherMode.CBC;
                aes.Key = key;
                aes.GenerateIV();

                using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                byte[] buffer = encoding.GetBytes(plainText);
                string encryptedText = Convert.ToBase64String(encryptor.TransformFinalBlock(buffer, 0, buffer.Length));

                string mac = BitConverter.ToString(
                    HmacSHA256(Convert.ToBase64String(aes.IV) + encryptedText, key))
                    .Replace("-", "").ToLower();

                var keyValues = new Dictionary<string, object>
                {
                    { "iv", Convert.ToBase64String(aes.IV) },
                    { "salt", Convert.ToBase64String(salt) },
                    { "value", encryptedText },
                    { "mac", mac },
                };
                return Convert.ToBase64String(encoding.GetBytes(JsonSerializer.Serialize(keyValues)));
            }
            catch (Exception e)
            {
                return "Error encrypting: " + e.ToString();
            }
        }

        /// <summary>
        /// AES-256-CBC decryption with MAC verification before decryption.
        /// </summary>
        /// <param name="plainText">String input for decryption</param>
        /// <param name="password">Master Password</param>
        /// <returns>string</returns>
        public static string Decrypt(string plainText, string password)
        {
            try
            {
                byte[] base64Decoded = Convert.FromBase64String(plainText);
                string base64DecodedStr = encoding.GetString(base64Decoded);
                var payload = JsonSerializer.Deserialize<Dictionary<string, string>>(base64DecodedStr);

                byte[] salt = Convert.FromBase64String(payload["salt"]);
                byte[] key = DeriveKey(password, salt);

                // Verify MAC before decryption to prevent padding oracle attacks
                string computedMac = BitConverter.ToString(
                    HmacSHA256(payload["iv"] + payload["value"], key))
                    .Replace("-", "").ToLower();

                if (!CryptographicOperations.FixedTimeEquals(
                    encoding.GetBytes(computedMac),
                    encoding.GetBytes(payload["mac"])))
                {
                    return "Error decrypting: MAC verification failed";
                }

                using var aes = Aes.Create();
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Padding = PaddingMode.PKCS7;
                aes.Mode = CipherMode.CBC;
                aes.Key = key;
                aes.IV = Convert.FromBase64String(payload["iv"]);

                using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                byte[] buffer = Convert.FromBase64String(payload["value"]);
                return encoding.GetString(decryptor.TransformFinalBlock(buffer, 0, buffer.Length));
            }
            catch (Exception e)
            {
                return "Error decrypting: " + e.ToString();
            }
        }

        private static byte[] DeriveKey(string password, byte[] salt)
        {
            using var keyGenerator = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
            return keyGenerator.GetBytes(KeySize);
        }

        private static byte[] HmacSHA256(string data, byte[] key)
        {
            using var hmac = new HMACSHA256(key);
            return hmac.ComputeHash(encoding.GetBytes(data));
        }
    }
}
