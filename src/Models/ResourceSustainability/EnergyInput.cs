using System.ComponentModel;

namespace GacsApp.Models.ResourceSustainability;

public enum EnergyInput
{
    [Description("Room temperature, Sunlight or Ambient processes")]
    Ambient = 8,

    [Description("Microwave or Ultrasound assisted (efficient heating)")]
    MicrowaveOrUltrasound = 6,

    [Description("Hydrothermal or Solvothermal (<200°C)")]
    LowTempHydrothermal = 4,

    [Description("Pyrolysis, Calcination or Hydrothermal (>200°C)")]
    HighTempHydrothermal = 2,

    [Description("High-energy methods")]
    HighEnergy = 1,

    [Description("Not specified")]
    NotSpecified = 0
}