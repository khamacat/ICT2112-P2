using ProRental.Domain.Entities;
using ProRental.Domain.Enums;

namespace ProRental.Interfaces.Domain;

public interface ISessionService
{
    Session CreateSession(int userId, UserRole role);
    bool ValidateSession(int sessionId);
    void TerminateSession(int sessionId);
}