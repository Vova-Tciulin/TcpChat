using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using MessageLibrary;

namespace Server;

public class ServerObject
{
    private TcpListener _tcpListener;
    private List<ClientObject> _clients = new List<ClientObject>();
    public ServerObject(IPAddress ipAddress,int port)
    {
        _tcpListener = new TcpListener(ipAddress, port);
    }

    public async Task StartAsync()
    {
        _tcpListener.Start();
        Console.WriteLine("Сервер начал свою работу.");

        while (true)
        {
            var tcpClient = await _tcpListener.AcceptTcpClientAsync();
            ClientObject client = new ClientObject(tcpClient,this);
            Console.WriteLine("Подключился новый пользователь.");
            _clients.Add(client);

            Task.Run(async () => await client.Listen());
        }
    }

    public async Task SendToAll(string id,ClientMessage message)
    {
        Console.WriteLine($"Кому: {message.Command} от кого:{message.Name}: {message.Message}");
        foreach (var client in _clients)
        {
            if (client.Id!=id)
            {
                var serializeMsg = JsonSerializer.Serialize(message);
                await client.Writer.WriteLineAsync(serializeMsg);
                await client.Writer.FlushAsync();
            }
           
        }
    }

    public async Task Send(string fromId, ClientMessage message)
    {
        var user = _clients.FirstOrDefault(u => u.name == message.Command);

        if (user!=null)
        {
            var serializeMsg = JsonSerializer.Serialize(message);
            
            await user.Writer.WriteLineAsync(serializeMsg);
            await user.Writer.FlushAsync();
        }
        else
        {
            user = _clients.FirstOrDefault(u => u.Id == fromId);
            message.Name = "сервер";
            message.Message = "Сообщение не удалось отправить.";
            var serializeMsg = JsonSerializer.Serialize(message);
            
            await user.Writer.WriteLineAsync(serializeMsg);
            await user.Writer.FlushAsync();
        }
    }
}