using System.ComponentModel;

namespace GacsApp.Models.SynthesisEfficiency;

public enum SimplicityScalability
{
    [Description("One-pot, room temperature, no specialized equipment")]
    OnePotRoomTempNoEquipment = 10,

    [Description("One-pot with simple, common lab equipment")]
    OnePotCommonEquipment = 8,

    [Description("One-pot with single specialized reactor")]
    OnePotSpecializedReactor = 5,

    [Description("Multi-step synthesis or complex instrumentation)")]
    MultiStepComplexInstrumentation = 2,

    [Description("Highly complex, dangerous, or poorly described process")]
    HighlyComplexOrPoorlyDescribed = 0
}
