using System.ComponentModel;

namespace GacsApp.Models;

public enum PrecursorOrigin
{
    [Description("Waste biomass or agri-food waste")]
    WasteBiomass = 12,

    [Description("Abundant non-waste natural materials")]
    AbundantNonWaste = 9,

    [Description("Purified bio-based molecules")]
    PurifiedBioBased = 6,

    [Description("Common synthetic chemicals")]
    CommonChemicals = 3,

    [Description("Chemicals with known high toxicity")]
    ChemicalsHighToxicity = 1,

    [Description("Not specified or highly hazardous")]
    NotSpecified = 3
}