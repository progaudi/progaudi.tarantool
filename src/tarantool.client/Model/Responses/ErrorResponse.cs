namespace Tarantool.Client.Model.Responses
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