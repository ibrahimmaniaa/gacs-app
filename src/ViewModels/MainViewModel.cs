using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GacsApp.Models;
using GacsApp.Models.ResourceSustainability;
using GacsApp.Services;
using GacsApp.Utils;

namespace GacsApp.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IMyService _service;

    public IEnumerable<EnumDisplayItem<PrecursorOrigin>> PrecursorOriginItems => EnumResolver.GetDisplayItems<PrecursorOrigin>();

    [ObservableProperty]
    private EnumDisplayItem<PrecursorOrigin>? selectedPrecursorOriginItem;

    public IEnumerable<EnumDisplayItem<SolventGreenness>> SolventGreennessItems => EnumResolver.GetDisplayItems<SolventGreenness>();

    [ObservableProperty]
    private EnumDisplayItem<SolventGreenness>? selectedSolventGreennessItem;

    [ObservableProperty]
    private string status;

    public IAsyncRelayCommand DoWorkCommand { get; }

    public MainViewModel(IMyService service)
    {
        _service = service;
        DoWorkCommand = new AsyncRelayCommand(CalculateScoreAsync);
    }

    private async Task CalculateScoreAsync()
    {
        if (SelectedPrecursorOriginItem == null) return;
        Status = (await _service.DoWorkAsync(SelectedPrecursorOriginItem.Value, SelectedSolventGreennessItem.Value)).ToString();
    }
}