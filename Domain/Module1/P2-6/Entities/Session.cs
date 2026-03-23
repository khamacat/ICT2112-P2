using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class Session
{
    // ── Public accessors for auto-generated private properties ──────────
    public int SessionId => Sessionid;
    public int UserId => Userid;
    public string RoleString => Role;
    public DateTime CreatedAt => Createdat;
    public DateTime ExpiresAt => Expiresat;

    // ── Computed helpers ─────────────────────────────────────────────────
    public UserRole RoleEnum => Enum.Parse<UserRole>(Role);
    public bool IsExpired() => DateTime.UtcNow >= Expiresat;

    // ── Factory method ───────────────────────────────────────────────────
    public static Session Create(int userId, UserRole role)
    {
        var flags = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;

        var session = new Session();
        typeof(Session).GetProperty("Userid", flags)!.SetValue(session, userId);
        typeof(Session).GetProperty("Role", flags)!.SetValue(session, role.ToString());
        typeof(Session).GetProperty("Createdat", flags)!.SetValue(session, DateTime.UtcNow);
        typeof(Session).GetProperty("Expiresat", flags)!.SetValue(session, DateTime.UtcNow.AddHours(2));
        return session;
    }
}