using System.Net.Sockets;
using System.Text.Json;
using System.Text.Json.Nodes;
using MessageLibrary;

namespace Client;

public class ClientObject
{
    private TcpClient _client;
    private StreamReader _reader;
    private StreamWriter _writer;
    private ClientMessage _message;
    public ClientObject(string name)
    {
        _client = new TcpClient();
        _message = new ClientMessage(name);
    }

    public void Connect(string hostname,int port)
    {
        while (true)
        {
            try
            {
                _client.Connect(hostname,port);
                Console.WriteLine("Подключение к серверу выполнено успешно.");
                _reader = new StreamReader(_client.GetStream());
                _writer = new StreamWriter(_client.GetStream());
                break;
            }
            catch 
            {
                Console.WriteLine("Не удалось подключиться к серверу. Повторная попытка подключиться.");
                Thread.Sleep(2000);
            }
        }
    }
    public async Task SendAsync()
    {
        Console.WriteLine("Введите имя получателя или 'all' для отправки сообщения для всех.");
        Console.WriteLine("После введите сообщение.");
        while (true)
        {
           
            var command = Console.ReadLine();
            
            var message = Console.ReadLine();
            _message.Message = message;
            _message.Command = command;
            var serializeMessage = JsonSerializer.Serialize(_message);
            
            await _writer.WriteLineAsync(serializeMessage);
            await _writer.FlushAsync();
        }
    }

    public async Task RecieveAsync()
    {
        while (true)
        {
            try
            {
                var message = await _reader.ReadLineAsync();

                if (!string.IsNullOrEmpty(message))
                {
                    var deserializeMessage = JsonSerializer.Deserialize<ClientMessage>(message);
                    Console.WriteLine($"{deserializeMessage.Name}: {deserializeMessage.Message}");
                }
            }
            catch
            {
                Console.WriteLine("Похоже, что соединение было разорвано.");
                
                break;
            }
            
        }
    }
}