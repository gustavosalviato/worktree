using System.Net;

namespace WorkTree.Exceptions.ExceptionsBase;

public class InvalidCredentialsError : ExceptionBase
{
    public InvalidCredentialsError(string errorMessage) : base(errorMessage)
    {
    }

    public override List<string> GetErrors()
    {
        return new List<string> { Message };
    }

    public override HttpStatusCode GetHttpStatusCode() => HttpStatusCode.BadRequest;
}