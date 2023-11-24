// wss://chat-1.goodgame.ru/chat2/

using System.Text.Json;
using GoodgameClient.Messages;
using WebSocketSharp;

// namespace ChatServer;

namespace GoodgameClient;

public class Program
{
    public static Queue<object> OutQueue = new();
    
    public static void Main(string[] args)
    {
        try
        {
            using var ws = new WebSocket("wss://chat-1.goodgame.ru/chat2/");
            
            ws.OnOpen += (sender, e) =>
            {

            };
                
            ws.OnClose += (sender, e) => {
                    
            };
                
            ws.OnError += (sender, e) => {
                    
            };
                
            ws.OnMessage += (sender, e) =>
            {
                // var json = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                var messageType = JsonSerializer.Deserialize<MessageType>(e.Data).Type;

                Console.WriteLine("MESSAGE: " + messageType);
                // Console.WriteLine(e.Data);

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
                        Console.WriteLine(chatMessage.Type);
                        Console.WriteLine(chatMessage.Data.UserName);
                        Console.WriteLine(chatMessage.Data.Text);
                        break;
                }
            };

            ws.Connect();
            Console.ReadKey(true);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}