using Microsoft.EntityFrameworkCore;
using ProRental.Data.UnitOfWork;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Domain;

namespace ProRental.Data.Services;

/// <summary>
/// Concrete implementation of IAuthenticationService.
/// NOTE: named ProRentalAuthenticationService to avoid ambiguity with
/// Microsoft.AspNetCore.Authentication.IAuthenticationService.
/// </summary>
public class ProRentalAuthenticationService : IAuthenticationService
{
    private readonly AppDbContext _db;
    private readonly ISessionService _sessionService;

    public ProRentalAuthenticationService(AppDbContext db, ISessionService sessionService)
    {
        _db = db;
        _sessionService = sessionService;
    }

    public AuthResult Authenticate(string email, string password)
    {
        // ── Step 1: Fetch the user row including the role via raw SQL ────
        //
        // ROOT CAUSE OF THE BUG:
        // The auto-generated AppDbContext maps the User entity but deliberately
        // omits the `userrole` column — there is no .Property("Userrole") call
        // in the scaffolded OnModelCreating. EF Core therefore never SELECTs
        // that column, so the backing private field is always null / default.
        // Reflection on that backing field returns the default UserRole value,
        // which happens to be CUSTOMER (index 0) regardless of what is stored
        // in the database.
        //
        // FIX: query the role directly via raw SQL so EF's entity mapping is
        // bypassed entirely, identical in approach to CustomerValidationService.

        var rows = _db.Database
            .SqlQueryRaw<UserAuthRow>(
                @"SELECT userid, name, passwordhash, userrole::text AS userrole
                  FROM public.""User""
                  WHERE LOWER(email) = LOWER({0})",
                email)
            .ToList();

        if (rows.Count == 0)
            return AuthResult.Failure("Invalid email or password.");

        var row = rows[0];

        if (row.Passwordhash == null)
            return AuthResult.Failure("User account data is incomplete.");

        // Plain-text comparison kept to match existing behaviour.
        // Replace with BCrypt.Verify() when hashing is introduced.
        if (password != row.Passwordhash)
            return AuthResult.Failure("Invalid email or password.");

        // ── Step 2: Parse the DB role string into the enum ───────────────
        // DB stores e.g. "STAFF", "CUSTOMER", "ADMIN" as the enum label.
        if (!Enum.TryParse<UserRole>(row.Userrole, ignoreCase: true, out var role))
            return AuthResult.Failure($"Unrecognised user role '{row.Userrole}' on this account.");

        var name = row.Name ?? email;

        // ── Step 3: Create session with the correct role ─────────────────
        var session = _sessionService.CreateSession(row.Userid, role);
        return AuthResult.Success(session, name);
    }

    /// <summary>
    /// Projection type for the raw SQL query.
    /// SqlQueryRaw maps columns to properties by name (case-insensitive).
    /// </summary>
    private class UserAuthRow
    {
        public int Userid { get; set; }
        public string? Name { get; set; }
        public string? Passwordhash { get; set; }
        public string Userrole { get; set; } = string.Empty;
    }
}