using System.Text.Json.Serialization;

namespace ChatServer.Messages;

public class Subscribe
{
    [JsonPropertyName("type")] public string? Type { get; set; }
    [JsonPropertyName("events")] public string[]? Events { get; set; }
}