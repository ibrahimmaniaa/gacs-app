using GacsApp.Models.ProductQuality;
using GacsApp.Models.ResourceSustainability;
using GacsApp.Models.SynthesisEfficiency;

namespace GacsApp.Models;

public class GacsSelection
{
    public PrecursorOrigin? PrecursorOrigin { get; set; }

    public SolventGreenness? SolventGreenness { get; set; }

    public EnergyInput? EnergyInput { get; set; }

    public EFactorWasteGeneration? EFactorWasteGeneration { get; set; }

    public SynthesisTime? SynthesisTime { get; set; }

    public SimplicityScalability? SimplicityScalability { get; set; }

    public PurificationSimplicity? PurificationSimplicity { get; set; }

    public ReactionMassEfficiency? ReactionMassEfficiency { get; set; }

    public QuantumYield? QuantumYield { get; set; }

    public MorphologyUniformity? MorphologyUniformity { get; set; }

    public PerformanceApplicability? PerformanceApplicability { get; set; }
}