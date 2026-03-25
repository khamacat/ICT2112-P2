using System;
using System.Collections.Generic;

namespace ProRental.Domain.Entities;

public partial class Train : Transport
{
    private int _trainId;
    private int TrainId { get => _trainId; set => _trainId = value; }

    private string? _trainType;
    private string? TrainType { get => _trainType; set => _trainType = value; }

    private string? _trainNumber;
    private string? TrainNumber { get => _trainNumber; set => _trainNumber = value; }
}
