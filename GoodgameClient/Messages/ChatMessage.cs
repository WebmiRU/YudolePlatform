using System.Text.Json.Serialization;

namespace GoodgameClient.Messages;

public class ChatMessageData
{
    [JsonPropertyName("channel_id")] public string? ChannelId { get; set; } = "join";
    [JsonPropertyName("user_id")] public long? UserId { get; set; } = 0;
    [JsonPropertyName("user_name")] public string? UserName { get; set; }
    [JsonPropertyName("premium")] public int? Premium { get; set; }
    [JsonPropertyName("color")] public string? Color { get; set; }
    [JsonPropertyName("role")] public string? Role { get; set; }
    [JsonPropertyName("mobile")] public int? Mobile { get; set; }
    [JsonPropertyName("message_id")] public long? MessageId { get; set; }
    [JsonPropertyName("timestamp")] public int? Timestamp { get; set; }
    [JsonPropertyName("text")] public string? Text { get; set; }
}

public class ChatMessage
{
    [JsonPropertyName("type")] public string? Type { get; set; } = "join";
    [JsonPropertyName("data")] public ChatMessageData Data { get; set; } = new();
}