using System.ComponentModel;

namespace GacsApp.Models.ProductQuality;

public enum QuantumYield
{
    [Description("≥ 50%")]
    GreaterOrEqual50 = 10,

    [Description("25 – 50%")]
    Between25And50 = 8,

    [Description("10 – 25%")]
    Between10And25 = 6,

    [Description("5 – 10%")]
    Between5And10 = 4,

    [Description("1 – 5%")]
    Between1And5 = 2,

    [Description("< 1% or Not Specified")]
    LessThan1OrNotSpecified = 0
}