namespace ExpertAdministration.Server.Exceptions;

/// <summary>
/// Occurs when the specified id is not found.
/// </summary>
public class IdNotFoundException : Exception
{
    public IdNotFoundException()
    {
    }

    /// <param name="id">The specified id that cannot be found.</param>
    public IdNotFoundException(string id)
    {
        
    }
        
    /// <param name="id">The specified id that cannot be found.</param>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public IdNotFoundException(string id, string message) : base(message)
    {
        
    }
}