using System.Text.Json.Serialization;

namespace GoodgameClient.Messages;


public class OutChannelJoinSuccessMessage
{
    [JsonPropertyName("id")] public string? Id { get; set; }
    [JsonPropertyName("type")] public string Type { get; set; } = "channel/join/success";
    [JsonPropertyName("channel_id")] public string? ChannelId { get; set; }
    [JsonPropertyName("service")] public string Service { get; set; } = "goodgame";
}