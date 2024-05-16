namespace MotorcycleRental.Domain.Models
{
    public class ErrorResponse
    {
        public string Message { get; set; }
        public string Detail { get; set; }

        public ErrorResponse(string message)
        {
            Message = message;
        }

        public ErrorResponse(string message, string detail)
        {
            Message = message;
            Detail = detail;
        }
    }
}
