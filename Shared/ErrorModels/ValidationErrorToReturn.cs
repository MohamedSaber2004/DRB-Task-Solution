using System.Net;

public class ValidationErrorToReturn
{
    public int StatusCode { get; set; } = (int) HttpStatusCode.BadRequest;
    public string Message { get; set; } = "One or more validation errors occurred.";
    public IEnumerable<ValidationError> Errors { get; set; } = [];
}

