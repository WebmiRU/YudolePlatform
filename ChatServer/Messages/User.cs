using System.Text.Json.Serialization;

namespace ChatServer.Messages;

public class User
{
    [JsonPropertyName("id")] public string? Id { get; set; }
    [JsonPropertyName("nickname")] public string? Nickname { get; set; }
    [JsonPropertyName("login")] public string? Login { get; set; }
    [JsonPropertyName("meta")] public UserMeta? Meta { get; set; }
}