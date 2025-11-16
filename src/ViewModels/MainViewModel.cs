using System.Windows;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GacsApp.Models;
using GacsApp.Models.ProductQuality;
using GacsApp.Models.ResourceSustainability;
using GacsApp.Models.SynthesisEfficiency;
using GacsApp.Services.interfaces;
using GacsApp.Utils;
using System.Windows.Media;

namespace GacsApp.ViewModels;

public partial class MainViewModel : ObservableObject
{
  private readonly IScoringService scoringService;

  public IEnumerable<EnumDisplayItem<PrecursorOrigin>> PrecursorOriginItems => EnumResolver.GetDisplayItems<PrecursorOrigin>();

  public IEnumerable<EnumDisplayItem<SolventGreenness>> SolventGreennessItems => EnumResolver.GetDisplayItems<SolventGreenness>();

  public IEnumerable<EnumDisplayItem<EnergyInput>> EnergyInputItems => EnumResolver.GetDisplayItems<EnergyInput>();

  public IEnumerable<EnumDisplayItem<EFactorWasteGeneration>> EFactorWasteGenerationItems => EnumResolver.GetDisplayItems<EFactorWasteGeneration>();

  public IEnumerable<EnumDisplayItem<SynthesisTime>> SynthesisTimeItems => EnumResolver.GetDisplayItems<SynthesisTime>();

  public IEnumerable<EnumDisplayItem<SimplicityScalability>> SimplicityScalabilityItems => EnumResolver.GetDisplayItems<SimplicityScalability>();

  public IEnumerable<EnumDisplayItem<PurificationSimplicity>> PurificationSimplicityItems => EnumResolver.GetDisplayItems<PurificationSimplicity>();

  public IEnumerable<EnumDisplayItem<ReactionMassEfficiency>> ReactionMassEfficiencyItems => EnumResolver.GetDisplayItems<ReactionMassEfficiency>();

  public IEnumerable<EnumDisplayItem<QuantumYield>> QuantumYieldItems => EnumResolver.GetDisplayItems<QuantumYield>();

  public IEnumerable<EnumDisplayItem<MorphologyUniformity>> MorphologyUniformityItems => EnumResolver.GetDisplayItems<MorphologyUniformity>();

  public IEnumerable<EnumDisplayItem<PerformanceApplicability>> PerformanceApplicabilityItems => EnumResolver.GetDisplayItems<PerformanceApplicability>();


  [ObservableProperty]
  private GacsSelection selection = new();

  [ObservableProperty]
  private int score;

  public IAsyncRelayCommand DoWorkCommand { get; }


  public double[] SliceSizes { get; } = [25, 30, 35, 28, 40, 32, 38, 27, 33, 36, 36];

  public Brush SliceColors { get; } = new LinearGradientBrush(
                                                              [
                                                                new GradientStop((Color)ColorConverter.ConvertFromString("#52B788"), 0.0), // Green
                                                                new GradientStop((Color)ColorConverter.ConvertFromString("#85C17E"), 0.4), // Light Green
                                                                new GradientStop((Color)ColorConverter.ConvertFromString("#B5D07F"), 0.41), // Yellow-Green
                                                                new GradientStop((Color)ColorConverter.ConvertFromString("#D9D96F"), 0.42), // Yellow
                                                                new GradientStop((Color)ColorConverter.ConvertFromString("#F8D66D"), 0.43), // Light Yellow
                                                                new GradientStop((Color)ColorConverter.ConvertFromString("#F8B739"), 0.47), // Orange-Yellow
                                                                new GradientStop((Color)ColorConverter.ConvertFromString("#F08C3A"), 0.6), // Orange
                                                                new GradientStop((Color)ColorConverter.ConvertFromString("#E85D4A"), 0.7), // Orange-Red
                                                                new GradientStop((Color)ColorConverter.ConvertFromString("#E85D75"), 0.8), // Light Red
                                                                new GradientStop((Color)ColorConverter.ConvertFromString("#E85D9A"), 0.9), // Pinkish Red
                                                                new GradientStop((Color)ColorConverter.ConvertFromString("#FF0000"), 1.0) // Red
                                                              ],
                                                              new Point(0, 0),
                                                              new Point(1, 0)
                                                             );


  public MainViewModel(IScoringService scoringService)
  {
    this.scoringService = scoringService;
    DoWorkCommand = new AsyncRelayCommand(CalculateScoreAsync);
  }


  private async Task CalculateScoreAsync()
  {
    if (Selection.GetAllSelectedEnums().Any(v => v == null)) return;

    Score = await scoringService.CalculateTotalScoreAsync(Selection);
  }
}