using System.ComponentModel;

namespace GacsApp.Models.Synthesis_Efficiency;

public enum PurificationSimplicity
{
    [Description("None or trivial")]
    NoneOrTrivial = 5,

    [Description("Single simple step")]
    SingleSimpleStep = 4,

    [Description("Single complex step or two simple steps")]
    ModerateComplexity = 3,

    [Description("Multiple complex steps")]
    MultipleComplexSteps = 1,

    [Description("Highly complex, multi-day purification")]
    HighlyComplexMultiDay = 0
}
