using System.ComponentModel;

namespace GacsApp.Models.SynthesisEfficiency;

public enum SynthesisTime
{
    [Description("≤ 1 hour")]
    LessThanOrEqualOneHour = 5,

    [Description("1 - 3 hours")]
    OneToThreeHours = 4,

    [Description("3 - 6 hours")]
    ThreeToSixHours = 3,

    [Description("6 - 12 hours")]
    SixToTwelveHours = 2,

    [Description("12 - 24 hours")]
    TwelveToTwentyFourHours = 1,

    [Description("> 24 hours")]
    MoreThan24Hours = 0
}
