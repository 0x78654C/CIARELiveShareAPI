using System.Collections.Concurrent;

namespace CIARELiveShareAPI.Utils
{
    public class GlobalVariables
    {
        public static readonly ConcurrentDictionary<string, string> connections = new();
        public static readonly ConcurrentDictionary<string, string> hostData = new();
        public const int MaxConnectionsPerSession = 10;
        public const int MaxSessions = 1000;
    }
}
