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
        // Program.wsClients.Add(this);
        var msg = e.Data == "BALUS"
            ? "Are you kidding?"
            : "I'm not available now.";

        Send(msg);
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