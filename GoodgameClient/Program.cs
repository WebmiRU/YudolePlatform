// wss://chat-1.goodgame.ru/chat2/

using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using GoodgameClient.Messages;
using WebSocketSharp;

// namespace ChatServer;

namespace GoodgameClient;

public class Program
{
    private static readonly Queue<object> OutQueue = new();
    // private static TcpClient client = new();

    private static void ConnectChatServer()
    {
        for (;;)
            try
            {
                var client = new TcpClient();
                client.Connect("127.0.0.1", 5380);
                // client.Connect("127.0.0.1", 5382);
                if (!client.Connected)
                {
                    Console.WriteLine("Chat server connection error, reconnecting...");
                    Thread.Sleep(3000);
                    continue;
                }

                var buffer = new byte[10000];
                int bytesRead;


                for (;;)
                    try
                    {
                        var stream = client.GetStream();

                        if (!client.Connected) break;

                        if (client.Available > 0)
                        {
                            bytesRead = stream.Read(buffer, 0, buffer.Length);
                            var json = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                            var messageType = JsonSerializer.Deserialize<MessageType>(json)?.Type;
                            Console.WriteLine(messageType);
                        }

                        if (OutQueue.Count > 0 && client.Connected)
                        {
                            var message = JsonSerializer.Serialize(OutQueue.Peek());

                            stream.Write(Encoding.UTF8.GetBytes(message));
                            stream.Flush();

                            if (client.Connected) OutQueue.Dequeue();
                            else break;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        break;
                    }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
    }

    private static void ConnectGoodgame()
    {
        using var ws = new WebSocket("wss://chat-1.goodgame.ru/chat2/");
        ws.OnMessage += (sender, e) =>
        {
            try
            {
                var messageType = JsonSerializer.Deserialize<MessageType>(e.Data).Type;

                Console.WriteLine("MESSAGE TYPE: " + messageType);
                Console.WriteLine(e.Data);

                switch (messageType)
                {
                    case "welcome":
                        var welcomeResponse = JsonSerializer.Serialize(new JoinMessage
                        {
                            Data = new JoinMessageData
                            {
                                ChannelId = "53029"
                            }
                        });

                        ws.Send(welcomeResponse);
                        break;

                    case "success_join":

                        break;

                    case "message":
                        var chatMessage = JsonSerializer.Deserialize<ChatMessage>(e.Data);
                        // Send message to server
                        OutQueue.Enqueue(new OutChatMessage
                        {
                            Html = chatMessage.Data.Text,
                            Text = chatMessage.Data.Text,
                            Src = chatMessage.Data.Text
                        });
                        break;

                    case "channel_counters":
                        var channelCounters = JsonSerializer.Deserialize<ChatMessage>(e.Data);
                        OutQueue.Enqueue(new OutChatMessage
                        {
                            Html = "Channel counters",
                            Text = "Channel counters",
                            Src = "Channel counters",
                            User = new User
                            {
                                Nickname = "Channel counters"
                            }
                        });
                        break;
                }
            }
            catch
            {
                Console.WriteLine(e.Data);
            }
        };

        ws.OnOpen += (sender, args) => { Console.WriteLine("Connection to GoodGame chat server established"); };

        ws.OnClose += (sender, args) =>
        {
            Console.WriteLine("Connection to GoodGame chat has been closed, reconnecting...");
            ws.Connect();
        };

        ws.OnError += (sender, args) =>
        {
            Console.WriteLine("Connection to GoodGame chat error: " + args.Message);
            ws.Close();
        };
    }

    public static void Main(string[] args)
    {
        var connectChatServer = new Thread(ConnectChatServer);
        connectChatServer.Start();

        // var processOutMessages = new Thread(ProcessOutMessages);
        // processOutMessages.Start();

        var connectGoodgame = new Thread(ConnectGoodgame);
        connectGoodgame.Start();

        Console.ReadKey();
    }
}