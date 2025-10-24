using System.ComponentModel;

namespace GacsApp.Models.ResourceSustainability;

public enum EFactorWasteGeneration
{
    [Description("E-Factor < 10 (Minimal waste)")]
    MinimalWaste = 10,

    [Description("E-Factor 10 - 50")]
    LowWaste = 7,

    [Description("E-Factor 50 - 100")]
    ModerateWaste = 4,

    [Description("E-Factor 100 - 500")]
    HighWaste = 2,

    [Description("E-Factor > 500 or Not Specified")]
    VeryHighWasteOrNotSpecified = 0
}