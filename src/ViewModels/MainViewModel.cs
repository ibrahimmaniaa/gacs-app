using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GacsApp.Models;
using GacsApp.Models.ProductQuality;
using GacsApp.Models.ResourceSustainability;
using GacsApp.Models.SynthesisEfficiency;
using GacsApp.Services.interfaces;
using GacsApp.Utils;

namespace GacsApp.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IScoringService _scoringService;

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


    public MainViewModel(IScoringService scoringService)
    {
        _scoringService = scoringService;
        DoWorkCommand = new AsyncRelayCommand(CalculateScoreAsync);
    }


    private async Task CalculateScoreAsync()
    {
        if (Selection.GetAllSelectedEnums().Any(v => v == null)) return;

        Score = await _scoringService.CalculateTotalScoreAsync(Selection);
    }
}