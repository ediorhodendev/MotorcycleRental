using Amazon.Runtime.Internal;
using MotorcycleRental.Domain.Models;

namespace MotorcycleRental.Domain.Exceptions
{
    public class CustomApplicationException : Exception
    {
        public Models.ErrorResponse ErrorResponse { get; }

        public CustomApplicationException(string message, string detail = null)
            : base(message)
        {
            ErrorResponse = new Models.ErrorResponse(message, detail);
        }
    }
}
