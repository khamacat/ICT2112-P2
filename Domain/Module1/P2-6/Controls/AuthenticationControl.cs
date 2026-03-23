using ProRental.Domain.Entities;
using ProRental.Interfaces.Domain;

namespace ProRental.Domain.Controls;

/// <summary>
/// <<Control>> AuthenticationControl
/// Orchestrates user login (via IAuthenticationService) and session lifecycle (via ISessionService).
/// Corresponds to the AuthenticationControl class in the Module 1 P2-6 class diagram.
/// </summary>
public class AuthenticationControl
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ISessionService _sessionService;

    public AuthenticationControl(
        IAuthenticationService authenticationService,
        ISessionService sessionService)
    {
        _authenticationService = authenticationService;
        _sessionService = sessionService;
    }

    /// <summary>
    /// Authenticates the user with the given credentials.
    /// On success, a session is created and the AuthResult (containing the session) is returned.
    /// On failure, the AuthResult carries an error message and no session.
    /// </summary>
    /// <param name="userId">The user's numeric ID.</param>
    /// <param name="password">The user's plaintext password (hashing is handled by IAuthenticationService).</param>
    /// <returns>AuthResult indicating success/failure with session or error message.</returns>
    public AuthResult AuthenticateUser(string email, string password)
{
    return _authenticationService.Authenticate(email, password);
}
    /// <summary>
    /// Terminates the session identified by sessionId, effectively logging the user out.
    /// </summary>
    /// <param name="sessionId">The session to invalidate.</param>
    public void Logout(int sessionId)
    {
        _sessionService.TerminateSession(sessionId);
    }
}