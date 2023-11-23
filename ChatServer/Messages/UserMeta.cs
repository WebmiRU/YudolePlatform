using System.Text.Json.Serialization;

namespace ChatServer.Messages;

public class UserMeta
{
    [JsonPropertyName("badges")] public string[]? Badges { get; set; }
}