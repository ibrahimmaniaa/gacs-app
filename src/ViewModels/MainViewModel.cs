using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GacsApp.Models; // Add this using
using GacsApp.Services;
using GacsApp.Utils;

namespace GacsApp;

public partial class MainViewModel : ObservableObject
{
    private readonly IMyService _service;

    public IEnumerable<string> StatusCodes => EnumResolver.GetDisplayItems<StatusCode>();

    [ObservableProperty]
    private EnumDisplayItem<StatusCode> selectedStatusItem;

    [ObservableProperty]
    private string status = string.Empty;

    public IAsyncRelayCommand DoWorkCommand { get; }

    public MainViewModel(IMyService service)
    {
        _service = service;
        DoWorkCommand = new AsyncRelayCommand(DoWorkAsync);
    }

    private async Task DoWorkAsync()
    {
        Status = "Working...";
        var result = await _service.DoWorkAsync(selectedStatusItem?.ToString(), selectedStatusItem.ToString());
        Status = result;
    }
}