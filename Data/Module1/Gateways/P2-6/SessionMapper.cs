using ProRental.Data.UnitOfWork;
using ProRental.Domain.Entities;
using ProRental.Interfaces.Data;

namespace ProRental.Data;

public class SessionMapper : ISessionMapper
{
    private readonly AppDbContext _db;

    public SessionMapper(AppDbContext db)
    {
        _db = db;
    }

    public Session? FindBySessionId(int sessionId)
    {
        return _db.Sessions
            .AsEnumerable()
            .FirstOrDefault(s => s.SessionId == sessionId);
    }

    public List<Session> FindByUserId(int userId)
    {
        return _db.Sessions
            .AsEnumerable()
            .Where(s => s.UserId == userId)
            .ToList();
    }

    public void Insert(Session session)
    {
        _db.Sessions.Add(session);
        _db.SaveChanges();
    }

    public void Update(Session session)
    {
        _db.Sessions.Update(session);
        _db.SaveChanges();
    }

    public void Delete(int sessionId)
    {
        var session = _db.Sessions
            .AsEnumerable()
            .FirstOrDefault(s => s.SessionId == sessionId);

        if (session != null)
        {
            _db.Sessions.Remove(session);
            _db.SaveChanges();
        }
    }

    public void DeleteExpiredSessions()
    {
        var expired = _db.Sessions
            .AsEnumerable()
            .Where(s => s.ExpiresAt <= DateTime.UtcNow)
            .ToList();

        _db.Sessions.RemoveRange(expired);
        _db.SaveChanges();
    }
}