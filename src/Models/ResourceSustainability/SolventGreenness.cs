using System.ComponentModel;

namespace GacsApp.Models.ResourceSustainability;

public enum SolventGreenness
{
    [Description("Water only")]
    Water = 10,

    [Description("Green solvents")]
    GreenSolvents = 7,

    [Description("Low-toxicity solvents")]
    LowToxicity = 4,

    [Description("Problematic solvents")]
    Problematic = 2,

    [Description("Highly hazardous solvents")]
    HighlyHazardous = 0
}