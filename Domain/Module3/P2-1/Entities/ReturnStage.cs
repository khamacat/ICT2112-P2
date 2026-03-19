using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class ReturnStage
{
    private CarbonStageType? _stageType;
    private CarbonStageType? StageType { get => _stageType; set => _stageType = value; }
    public void UpdateStageType(CarbonStageType newValue) => _stageType = newValue;

    private StageType? _stageTypeAlt;
    private StageType? StageTypeAlt { get => _stageTypeAlt; set => _stageTypeAlt = value; }
    public void UpdateStageTypeAlt(StageType newValue) => _stageTypeAlt = newValue;
}