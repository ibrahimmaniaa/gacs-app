namespace GacsApp.Models;

/// <summary>
/// Defines the pizza slice angles for each GACS criterion based on their maximum possible scores.
/// The angles are proportional to the maximum score, summing to 360 degrees.
/// </summary>
public static class SliceConfiguration
{
  // Calculate angles proportional to max scores (total 360 degrees)
  // The angles are calculated as: (MaxScore / TotalMaxScore) × 360°
  public static readonly double PrecursorOriginAngle = 43;

  public static readonly double SolventGreennessAngle = 36;

  public static readonly double EnergyInputAngle = 29;

  public static readonly double EFactorWasteAngle = 36;

  public static readonly double SynthesisTimeAngle = 18;

  public static readonly double SimplicityScalabilityAngle = 36;

  public static readonly double PurificationSimplicityAngle = 18;

  public static readonly double ReactionMassEfficiencyAngle = 36;

  public static readonly double QuantumYieldAngle = 36;

  public static readonly double MorphologyUniformityAngle = 36;

  public static readonly double PerformanceApplicabilityAngle = 36;


  /// <summary>
  /// Gets all slice angles in the order they appear in the GACS selection.
  /// </summary>
  public static double[] GetSliceAngles() =>
  [
    PrecursorOriginAngle,
    SolventGreennessAngle,
    EnergyInputAngle,
    EFactorWasteAngle,
    SynthesisTimeAngle,
    SimplicityScalabilityAngle,
    PurificationSimplicityAngle,
    ReactionMassEfficiencyAngle,
    QuantumYieldAngle,
    MorphologyUniformityAngle,
    PerformanceApplicabilityAngle
  ];
}