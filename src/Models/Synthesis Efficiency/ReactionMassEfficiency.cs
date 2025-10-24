using System.ComponentModel;

namespace GacsApp.Models.Synthesis_Efficiency;

public enum ReactionMassEfficiency
{
    [Description("Yield ≥ 50% or RME > 80%")]
    HighYield = 10,

    [Description("Yield 25 - 50% or RME 50 - 80%")]
    ModerateYield = 7,

    [Description("Yield 10 - 25%")]
    LowYield = 4,

    [Description("Yield 1 - 10%")]
    VeryLowYield = 2,

    [Description("Yield < 1% or Not Specified")]
    ExtremelyLowOrNotSpecified = 0
}