using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using ChatServer.Messages;

var tcpListener = new TcpListener(IPAddress.Any, 5380);
var subscribers = new ConcurrentDictionary<TcpClient, string[]>();

Thread t = new Thread(() =>
{
    for(;;)
    {
        // foreach (var x in subscribers.Where(v => v.Value.Contains("xxx")).Select(k => k.Key))
        // {
        //     Console.WriteLine(x);        
        // }
        
        // Console.WriteLine(subscribers.Count);
        // foreach (var s in subscribers.Values)
        // {
        //     foreach (var v in s)
        //     {
        //         Console.WriteLine(v);
        //     }
        // }
        
        Thread.Sleep(1000);
    }

    // dictionary.Keys.Any(key => key.Contains("a"));
});

t.Start();

try
{
    tcpListener.Start(); // запускаем сервер
    Console.WriteLine("Сервер запущен. Ожидание подключений... ");

    while (true)
    {
        var tcpClient = await tcpListener.AcceptTcpClientAsync();
        Task.Run(async () => await ProcessClientAsync(tcpClient));
        // new Thread(async () => await ProcessClientAsync(tcpClient)).Start();
    }
}
finally
{
    tcpListener.Stop();
}

async Task ProcessClientAsync(TcpClient client)
{
    var stream = client.GetStream();
    var buffer = new byte[10000];
    int bytesRead;

    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
    {
        var json = Encoding.ASCII.GetString(buffer, 0, bytesRead);

        switch (JsonSerializer.Deserialize<MessageType>(json).Type)
        {
            case "subscribe":
                var message = JsonSerializer.Deserialize<MessageSubscribe>(json);
                subscribers[client] = message.Events;
                break;
        }
    }

    subscribers.TryRemove(client, out _);
    client.Close();
}