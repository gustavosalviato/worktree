namespace WorkTree.Communication.Responses;

public class ResponseErrorMessagesJson
{
    public List<string> Errors { get; private set; }

    public ResponseErrorMessagesJson(string messsage)
    {
        Errors = new List<string> { messsage };
    }

    public ResponseErrorMessagesJson(List<string> messages)
    {
        Errors = messages;
    }
}