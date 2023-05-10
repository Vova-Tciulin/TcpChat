
using Client;

Console.WriteLine("Введите ваше имя:");

var name = Console.ReadLine();
ClientObject client=new ClientObject(name);
client.Connect("127.0.0.1",8888);
Task.Run(async()=>await client.SendAsync());
await client.RecieveAsync();