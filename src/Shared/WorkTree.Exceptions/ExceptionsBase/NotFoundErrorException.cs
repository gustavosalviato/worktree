using System.Net;

namespace WorkTree.Exceptions.ExceptionsBase;

public class NotFoundErrorException : ExceptionBase
{
    public NotFoundErrorException(string errorMessage) : base(errorMessage)
    {
    }

    public override List<string> GetErrors()
    {
        return new List<string> { Message };
    }

    public override HttpStatusCode GetHttpStatusCode() => HttpStatusCode.NotFound;
}