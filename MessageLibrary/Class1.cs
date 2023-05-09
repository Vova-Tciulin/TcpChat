namespace MessageLibrary;

public class ClientMessage
{
    public string Name { get; set; }
    public string Command { get; set; }
    public string Message { get; set; }

    public ClientMessage(string name)
    {
        Name = name;
    }
}