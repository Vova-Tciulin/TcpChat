using System.Net.Sockets;
using System.Text.Json;
using MessageLibrary;

namespace Server;

public class ClientObject
{
    public string Id { get; } = Guid.NewGuid().ToString();
    public string? name;
    public StreamReader Reader { get; }
    public StreamWriter Writer { get; }
    private ServerObject _server;
    public ClientObject(TcpClient client, ServerObject server)
    {
        _server = server;
        Reader = new StreamReader(client.GetStream());
        Writer = new StreamWriter(client.GetStream());
    }

    public async Task Listen()
    {

        while (true)
        {
            string recieve = await Reader.ReadLineAsync();
            var message = JsonSerializer.Deserialize<ClientMessage>(recieve);

            if (string.IsNullOrEmpty(name)) name = message.Name;

            if (message.Command=="all")
            {
                _server.SendToAll(Id, message);
            }
            else
            {
                _server.Send(Id, message);
            }


        }
    }
}