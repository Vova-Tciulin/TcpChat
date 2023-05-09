
using net;

Console.WriteLine("клиент начал свою работу.\nВведите ваше имя:");

var name = Console.ReadLine();
Client client=new Client(name);
client.Connect("127.0.0.1",8888);
Task.Run(async()=>await client.SendAsync());
await client.RecieveAsync();