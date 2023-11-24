using System.Text.Json.Serialization;

namespace GoodgameClient.Messages;

public class JoinMessageData
{
    [JsonPropertyName("channel_id")] public string? ChannelId { get; set; }
    [JsonPropertyName("hidden")] public int? Hidden { get; set; } = 0;
    [JsonPropertyName("mobile")] public bool? Mobile { get; set; } = false;
    [JsonPropertyName("reload")] public bool? Reload { get; set; } = false;
}

public class JoinMessage
{
    [JsonPropertyName("type")] public string? Type { get; set; } = "join";
    [JsonPropertyName("data")] public JoinMessageData Data { get; set; } = new();
}