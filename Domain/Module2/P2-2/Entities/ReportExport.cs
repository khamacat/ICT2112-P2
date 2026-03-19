namespace ProRental.Domain.Entities;
using ProRental.Domain.Enums;
public partial class Reportexport
{
    private VisualType _type;
    private VisualType type { get => _type; set => _type = value; }
    public void UpdateType(VisualType newValue) => _type = newValue;

    private FileFormat _format;
    private FileFormat format { get => _format; set => _format = value; }
    public void UpdateFormat(FileFormat newValue) => _format = newValue;
}