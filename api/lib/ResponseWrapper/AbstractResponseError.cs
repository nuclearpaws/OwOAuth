namespace ResponseWrapper
{
    public abstract class AbstractResponseError
    {
        public string Message { get; private set; }

        public AbstractResponseError(string message)
        {
            Message = message;
        }
    }
}
