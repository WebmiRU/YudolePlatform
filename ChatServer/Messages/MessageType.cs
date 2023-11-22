using System.Text.Json.Serialization;

namespace ChatServer.Messages;

public class MessageType
{
    [JsonPropertyName("type")] public string? Type { get; set; }
}