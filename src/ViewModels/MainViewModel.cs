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

  public IEnumerable<EnumDisplayItem<PrecursorOrigin>> PrecursorOriginItems => EnumHelper.GetDisplayItems<PrecursorOrigin>();

  public IEnumerable<EnumDisplayItem<SolventGreenness>> SolventGreennessItems => EnumHelper.GetDisplayItems<SolventGreenness>();

  public IEnumerable<EnumDisplayItem<EnergyInput>> EnergyInputItems => EnumHelper.GetDisplayItems<EnergyInput>();

  public IEnumerable<EnumDisplayItem<EFactorWasteGeneration>> EFactorWasteGenerationItems => EnumHelper.GetDisplayItems<EFactorWasteGeneration>();

  public IEnumerable<EnumDisplayItem<SynthesisTime>> SynthesisTimeItems => EnumHelper.GetDisplayItems<SynthesisTime>();

  public IEnumerable<EnumDisplayItem<SimplicityScalability>> SimplicityScalabilityItems => EnumHelper.GetDisplayItems<SimplicityScalability>();

  public IEnumerable<EnumDisplayItem<PurificationSimplicity>> PurificationSimplicityItems => EnumHelper.GetDisplayItems<PurificationSimplicity>();

  public IEnumerable<EnumDisplayItem<ReactionMassEfficiency>> ReactionMassEfficiencyItems => EnumHelper.GetDisplayItems<ReactionMassEfficiency>();

  public IEnumerable<EnumDisplayItem<QuantumYield>> QuantumYieldItems => EnumHelper.GetDisplayItems<QuantumYield>();

  public IEnumerable<EnumDisplayItem<MorphologyUniformity>> MorphologyUniformityItems => EnumHelper.GetDisplayItems<MorphologyUniformity>();

  public IEnumerable<EnumDisplayItem<PerformanceApplicability>> PerformanceApplicabilityItems => EnumHelper.GetDisplayItems<PerformanceApplicability>();


  [ObservableProperty]
  private GacsSelection selection = new();

  [ObservableProperty]
  private int? score;

  public IAsyncRelayCommand CalculateScoreCommand { get; }


  public IList<Brush> SliceColors { get; } =
  [
    new SolidColorBrush((Color)ColorConverter.ConvertFromString("#268602")), // Dark Green
    new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ACD201")), // Yellowish Green
    new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF737")), // Light Yellow
    new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDB1F")), // Dark Yellow
    new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA51F")), // Orange
    new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D90404")), // Red
  ];


  public MainViewModel(IScoringService scoringService)
  {
    this.scoringService = scoringService;
    CalculateScoreCommand = new AsyncRelayCommand(CalculateScoreAsync);
  }


  private async Task CalculateScoreAsync()
  {
    if (Selection.GetNullableEnumProperties().Any(v => v == null)) return;

    Score = await scoringService.CalculateTotalScoreAsync(Selection);
  }
}