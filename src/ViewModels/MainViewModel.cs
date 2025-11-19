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


  public List<Brush> SliceColors { get; } =
  [
    new SolidColorBrush((Color)ColorConverter.ConvertFromString("#52B788")), // Green
    new SolidColorBrush((Color)ColorConverter.ConvertFromString("#85C17E")), // Light Green
    new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B5D07F")), // Yellow-Green
    new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D9D96F")), // Yellow
    new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F8B739")), // Orange-Yellow
    new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F08C3A")), // Orange
    new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E85D4A")), // Orange-Red
  ];


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