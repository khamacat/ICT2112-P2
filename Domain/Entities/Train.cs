using System;
using System.Collections.Generic;

namespace ProRental.Domain.Entities;

public partial class Train
{
    public int TransportId { get; private set; }

    public int TrainId { get; private set; }

    public string? TrainType { get; private set; }

    public string? TrainNumber { get; private set; }

    public virtual Transport Transport { get; private set; } = null!;
}
