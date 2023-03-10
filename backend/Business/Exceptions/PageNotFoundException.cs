namespace Business.Exceptions;

public class PageNotFoundException : Exception
{
    public PageNotFoundException(string message, int pageNumber)
        : base(ModifyMessage(message, pageNumber))
    {
    }

    private static string ModifyMessage(string message, int pageNumber)
    {
        return $"{message}: {pageNumber}";
    }
}