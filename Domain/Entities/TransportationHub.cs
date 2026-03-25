using System;
using System.Collections.Generic;

namespace ProRental.Domain.Entities;

public abstract partial class TransportationHub
{
    private int _hubId;
    private int HubId { get => _hubId; set => _hubId = value; }

    private double _longitude;
    private double Longitude { get => _longitude; set => _longitude = value; }

    private double _latitude;
    private double Latitude { get => _latitude; set => _latitude = value; }

    private string _countryCode = null!;
    private string CountryCode { get => _countryCode; set => _countryCode = value; }

    private string _address = null!;
    private string Address { get => _address; set => _address = value; }

    private string? _operationalStatus;
    private string? OperationalStatus { get => _operationalStatus; set => _operationalStatus = value; }

    private string? _operationTime;
    private string? OperationTime { get => _operationTime; set => _operationTime = value; }

    public virtual ICollection<DeliveryBatch> DeliveryBatches { get; private set; } = new List<DeliveryBatch>();
}
