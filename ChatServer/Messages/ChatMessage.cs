using System.Text.Json.Serialization;

namespace ChatServer.Messages;

public class ChatMessage
{
    [JsonPropertyName("id")] public string? Id { get; set; }
    [JsonPropertyName("type")] public string? Type { get; set; }
    [JsonPropertyName("service")] public string? Service { get; set; }
    [JsonPropertyName("html")] public string? Html { get; set; }
    [JsonPropertyName("src")] public string? Src { get; set; }
    [JsonPropertyName("text")] public string? Text { get; set; }
    [JsonPropertyName("user")] public User? User { get; set; }
}