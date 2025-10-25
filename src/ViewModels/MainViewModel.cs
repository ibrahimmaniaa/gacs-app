using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GacsApp.Models;
using GacsApp.Models.ProductQuality;
using GacsApp.Models.ResourceSustainability;
using GacsApp.Models.Synthesis_Efficiency;
using GacsApp.Services;
using GacsApp.Utils;

namespace GacsApp.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IMyService _service;

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
    private EnumDisplayItem<PrecursorOrigin>? selectedPrecursorOriginItem;
    [ObservableProperty]
    private EnumDisplayItem<SolventGreenness>? selectedSolventGreennessItem;
    [ObservableProperty]
    private EnumDisplayItem<EnergyInput>? selectedEnergyInputItem;
    [ObservableProperty]
    private EnumDisplayItem<EFactorWasteGeneration>? selectedEFactorWasteGenerationItem;
    [ObservableProperty]
    private EnumDisplayItem<SynthesisTime>? selectedSynthesisTimeItem;
    [ObservableProperty]
    private EnumDisplayItem<SimplicityScalability>? selectedSimplicityScalabilityItem;
    [ObservableProperty]
    private EnumDisplayItem<PurificationSimplicity>? selectedPurificationSimplicityItem;
    [ObservableProperty]
    private EnumDisplayItem<ReactionMassEfficiency>? selectedReactionMassEfficiencyItem;
    [ObservableProperty]
    private EnumDisplayItem<QuantumYield>? selectedQuantumYieldItem;
    [ObservableProperty]
    private EnumDisplayItem<MorphologyUniformity>? selectedMorphologyUniformityItem;
    [ObservableProperty]
    private EnumDisplayItem<PerformanceApplicability>? selectedPerformanceApplicabilityItem;

    [ObservableProperty]
    private int score;

    public IAsyncRelayCommand DoWorkCommand { get; }

    public MainViewModel(IMyService service)
    {
        _service = service;
        DoWorkCommand = new AsyncRelayCommand(CalculateScoreAsync);
    }

    private async Task CalculateScoreAsync()
    {
        if (SelectedPrecursorOriginItem == null) return;

        Score = await _service.DoWorkAsync(SelectedPrecursorOriginItem.Value, SelectedSolventGreennessItem.Value);
    }
}