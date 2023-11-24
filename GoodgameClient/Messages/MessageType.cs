using System.Text.Json.Serialization;

namespace GoodgameClient.Messages;

public class MessageType
{
    [JsonPropertyName("type")] public string? Type { get; set; }
}