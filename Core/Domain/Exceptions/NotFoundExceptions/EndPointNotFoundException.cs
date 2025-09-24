public class EndPointNotFoundException(string message)
    : NotFoundException($"End-Point {message} is not found.")
{ }

