using System.Net;

namespace WorkTree.Exceptions.ExceptionsBase;

public class UnauthorizedErrorException : ExceptionBase
{
    public UnauthorizedErrorException(string errorMessage) : base(errorMessage)
    {
    }

    public override List<string> GetErrors()
    {
        return new List<string> { Message };
    }

    public override HttpStatusCode GetHttpStatusCode() => HttpStatusCode.Unauthorized;
}