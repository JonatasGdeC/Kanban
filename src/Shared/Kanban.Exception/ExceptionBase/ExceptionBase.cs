namespace Kanban.Exception.ExceptionBase;

public abstract class ExceptionBase(string message) : System.Exception(message: message)
{
    public abstract int StatusCode { get; }
    public abstract List<string> GetErrors();
}