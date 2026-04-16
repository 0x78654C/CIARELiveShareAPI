using CIARELiveShareAPI.Hubs;
using Microsoft.AspNetCore.ResponseCompression;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddResponseCompression(opts =>
{ 
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});

// Keep alive set to 20 seconds.
builder.Services.AddSignalR(hubOptions =>
{
    hubOptions.KeepAliveInterval = TimeSpan.FromSeconds(20.00);
    hubOptions.MaximumReceiveMessageSize = 5_000_000; // 5MB
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseRouting();
app.MapHub<LiveShare>("/live",opts=>
{
    opts.ApplicationMaxBufferSize = 10_000_000; // 10MB
    opts.TransportMaxBufferSize = 10_000_000; // 10MB
});

app.MapGet("/ping", () =>
{
    return Results.Ok("Pong");
});

app.Run();
