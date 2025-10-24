using System.ComponentModel;

namespace GacsApp.Models.ProductQuality;

public enum MorphologyUniformity
{
    [Description("Size < 3 nm and SD < 0.5 nm (or PDI < 0.1)")]
    HighlyUniformSmall = 10,

    [Description("Size 3–5 nm and SD 0.5–1.5 nm (or PDI 0.1–0.2)")]
    VeryUniform = 8,

    [Description("Size 5–7 nm and SD 1.5–2.5 nm (or PDI 0.2–0.3)")]
    ModeratelyUniform = 6,

    [Description("Defined size but broad distribution (SD > 2.5 nm)")]
    BroadDistribution = 4,

    [Description("No characterization or very poor uniformity")]
    PoorlyCharacterized = 0
}