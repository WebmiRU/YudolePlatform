using System.Text.Json;
using ChatServer.Messages;
using WebSocketSharp;
using WebSocketSharp.Server;
using ErrorEventArgs = WebSocketSharp.ErrorEventArgs;

namespace ChatServer;

public class WSS : WebSocketBehavior
{
    public void Snd(string message)
    {
        Send(message);
    }

    protected override void OnMessage(MessageEventArgs e)
    {
        var messageType = JsonSerializer.Deserialize<MessageType>(e.Data).Type;

        switch (messageType)
        {
            case "subscribe":
                var subscribe = JsonSerializer.Deserialize<Subscribe>(e.Data);

                if (!Program.subscribersWs.ContainsKey(this)) Program.subscribersWs[this] = new List<string>();

                foreach (var v in subscribe.Events)
                    if (!Program.subscribersWs[this].Contains(v))
                        Program.subscribersWs[this].Add(v);
                break;

            case "unsubscribe":
                var unsubscribe = JsonSerializer.Deserialize<Subscribe>(e.Data);
                foreach (var v in unsubscribe.Events) Program.subscribersWs[this].Remove(v);
                break;
        }
    }

    protected override void OnOpen()
    {
        Console.WriteLine("CONNECTED!");
    }

    protected override void OnClose(CloseEventArgs e)
    {
        Program.subscribersWs.Remove(this, out _);
    }

    protected override void OnError(ErrorEventArgs e)
    {
    }
}