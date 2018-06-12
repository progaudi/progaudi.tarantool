namespace ProGaudi.Tarantool.Client.Model
{
    public class ErrorResponse
    {
        public ErrorResponse(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; }
    }
}