using ProRental.Domain.Entities;

namespace ProRental.Interfaces.Data;

public interface ISessionMapper
{
    Session? FindBySessionId(int sessionId);
    List<Session> FindByUserId(int userId);
    void Insert(Session session);
    void Update(Session session);
    void Delete(int sessionId);
    void DeleteExpiredSessions();
}