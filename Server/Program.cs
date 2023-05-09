
using System.Net;
using Server;

ServerObject server=new ServerObject(IPAddress.Parse("127.0.0.1"),8888);

await server.StartAsync();