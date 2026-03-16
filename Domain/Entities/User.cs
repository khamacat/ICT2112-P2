using System;
using System.Collections.Generic;

namespace ProRental.Domain.Entities;

public partial class User
{
    public int Userid { get; private set; }

    public string Name { get; private set; } = null!;

    public string Email { get; private set; } = null!;

    public string Passwordhash { get; private set; } = null!;

    public int? Phonecountry { get; private set; }

    public string? Phonenumber { get; private set; }

    public virtual Customer? Customer { get; private set; }

    public virtual ICollection<Notificationpreference> Notificationpreferences { get; private set; } = new List<Notificationpreference>();

    public virtual ICollection<Notification> Notifications { get; private set; } = new List<Notification>();

    public virtual ICollection<Session> Sessions { get; private set; } = new List<Session>();

    public virtual Staff? Staff { get; private set; }
}
