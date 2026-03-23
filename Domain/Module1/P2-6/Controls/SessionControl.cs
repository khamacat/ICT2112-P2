using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Data;
using ProRental.Interfaces.Domain;

namespace ProRental.Domain.Controls;

public class SessionControl : ISessionService
{
    private readonly ISessionMapper _sessionMapper;

    public SessionControl(ISessionMapper sessionMapper)
    {
        _sessionMapper = sessionMapper;
    }

    public Session CreateSession(int userId, UserRole role)
{
    var session = Session.Create(userId, role);
    _sessionMapper.Insert(session);
    return session;
}

    public bool ValidateSession(int sessionId)
    {
        var session = _sessionMapper.FindBySessionId(sessionId);
        if (session == null) return false;
        return !session.IsExpired();
    }

    public void TerminateSession(int sessionId)
    {
        _sessionMapper.Delete(sessionId);
    }
}