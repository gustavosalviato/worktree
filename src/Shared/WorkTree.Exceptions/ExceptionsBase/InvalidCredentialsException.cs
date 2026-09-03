using System.Net;

namespace WorkTree.Exceptions.ExceptionsBase;

public class InvalidCredentialsException : ExceptionBase
{
    public InvalidCredentialsException(string errorMessage) : base(errorMessage)
    {
    }

    public override List<string> GetErrors()
    {
        return new List<string> { Message };
    }

    public override HttpStatusCode GetHttpStatusCode() => HttpStatusCode.Unauthorized;
}