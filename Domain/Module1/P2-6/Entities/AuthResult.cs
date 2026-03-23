namespace ProRental.Domain.Entities;

/// <summary>
/// Returned by IAuthenticationService.Authenticate().
/// Carries both success/failure status and, on success, the newly created session.
/// </summary>
public class AuthResult
{
    public bool IsSuccess { get; private set; }
    public string? ErrorMessage { get; private set; }
    public Session? Session { get; private set; }
    public string? UserName { get; private set; }

    private AuthResult() { }

    public static AuthResult Success(Session session, string userName)
    {
        return new AuthResult
        {
            IsSuccess = true,
            Session = session,
            UserName = userName
        };
    }

    public static AuthResult Failure(string message)
    {
        return new AuthResult
        {
            IsSuccess = false,
            ErrorMessage = message
        };
    }
}