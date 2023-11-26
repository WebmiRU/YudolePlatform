using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using ChatServer.Messages;
using WebSocketSharp.Server;

namespace ChatServer;

internal class Program
{
    public static ConcurrentDictionary<TcpClient, List<string>> subscribersTcp = new();
    public static ConcurrentDictionary<WSS, List<string>> subscribersWs = new();

    private static void Main(string[] args)
    {
        var t = new Thread(() =>
        {
            for (;;)
            {
                Console.WriteLine(subscribersTcp.Count > 0 ? subscribersTcp.First().Value.Count : "NONE");
                Thread.Sleep(1000);
            }
        });

        // t.Start();

        var wss = new WebSocketServer("ws://0.0.0.0:5300");
        wss.AddWebSocketService<WSS>("/");
        wss.Start();

        var tcpListener = new TcpListener(IPAddress.Any, 5380);

        try
        {
            tcpListener.Start();
            Console.WriteLine("Сервер запущен. Ожидание подключений...");

            while (true)
            {
                var tcpClient = tcpListener.AcceptTcpClient();
                Task.Run(async () => await ProcessClientAsync(tcpClient));
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            tcpListener.Stop();
        }
    }

    private static Task ProcessClientAsync(TcpClient client)
    {
        var stream = client.GetStream();
        var buffer = new byte[10000];
        int bytesRead;

        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
        {
            var json = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine("MESSAGE: " + json);
            var messageType = JsonSerializer.Deserialize<MessageType>(json)?.Type;

            switch (messageType)
            {
                case "subscribe":
                    var subscribe = JsonSerializer.Deserialize<Subscribe>(json);

                    if (!subscribersTcp.ContainsKey(client)) subscribersTcp[client] = new List<string>();

                    foreach (var v in subscribe.Events)
                        if (!subscribersTcp[client].Contains(v))
                            subscribersTcp[client].Add(v);
                    break;

                case "unsubscribe":
                    var unsubscribe = JsonSerializer.Deserialize<Subscribe>(json);
                    foreach (var v in unsubscribe.Events) subscribersTcp[client].Remove(v);
                    break;

                default:
                    // Send message to TCP subscribers
                    foreach (var c in subscribersTcp.Where(v => v.Value.Contains(messageType)).Select(k => k.Key))
                    {
                        try
                        {
                            var cStream = c.GetStream();
                            cStream.Write(Encoding.UTF8.GetBytes(json));
                            cStream.Flush();
                        }
                        catch
                        {
                            // ignored
                        }
                    }

                    // Send message to Websocket subscribers
                    foreach (var c in subscribersWs.Where(v => v.Value.Contains(messageType)).Select(k => k.Key))
                        c.Snd(json);
                    break;
            }
        }

        subscribersTcp.Remove(client, out _);
        client.Close();
        return Task.CompletedTask;
    }
}