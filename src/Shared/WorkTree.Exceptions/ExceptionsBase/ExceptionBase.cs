using System.Net;

namespace WorkTree.Exceptions.ExceptionsBase;

public abstract class ExceptionBase: SystemException
{
  public ExceptionBase(string errorMessage) : base(errorMessage)
  {
  }

  public abstract List<string> GetErrors();
  public abstract HttpStatusCode GetHttpStatusCode();
}