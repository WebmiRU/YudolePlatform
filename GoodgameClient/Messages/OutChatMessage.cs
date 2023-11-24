using System.Text.Json.Serialization;

namespace GoodgameClient.Messages;

public class UserMeta
{
    [JsonPropertyName("badges")] public string[]? Badges { get; set; }
}

public class User
{
    [JsonPropertyName("id")] public string? Id { get; set; }
    [JsonPropertyName("nickname")] public string? Nickname { get; set; }
    [JsonPropertyName("login")] public string? Login { get; set; }
    [JsonPropertyName("meta")] public UserMeta? Meta { get; set; } = new();
}

public class OutChatMessage
{
    [JsonPropertyName("id")] public string? Id { get; set; }
    [JsonPropertyName("type")] public string? Type { get; set; } = "chat/message";
    [JsonPropertyName("service")] public string? Service { get; set; } = "goodgame";
    [JsonPropertyName("html")] public string? Html { get; set; }
    [JsonPropertyName("text")] public string? Text { get; set; }
    [JsonPropertyName("text_clear")] public string? TextClear { get; set; }
    [JsonPropertyName("user")] public User? User { get; set; } = new();
}