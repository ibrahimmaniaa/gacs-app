using System.ComponentModel;

namespace GacsApp.Models.ProductQuality;

public enum PerformanceApplicability
{
    [Description("High-performance in demanding application (e.g., in vivo bioimaging, electrocatalysis) with data")]
    HighPerformance = 10,

    [Description("Good performance in standard application (e.g., sensing, in vitro imaging) with data")]
    GoodPerformance = 7,

    [Description("Application demonstrated but with mediocre performance")]
    ModeratePerformance = 4,

    [Description("Application claimed but with minimal or no supporting data")]
    ClaimedWithoutData = 2,

    [Description("No application investigated or mentioned")]
    NoApplication = 0
}