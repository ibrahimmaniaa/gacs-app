using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GacsApp.Models; // Add this using
using GacsApp.Services;

namespace GacsApp;

public partial class MainViewModel : ObservableObject
{
    private readonly IMyService _service;

    [ObservableProperty]
    private ObservableCollection<ExampleEnum> firstOptions;

    [ObservableProperty]
    private ObservableCollection<string> secondOptions;

    [ObservableProperty]
    private ExampleEnum? selectedFirst;

    [ObservableProperty]
    private string? selectedSecond;

    [ObservableProperty]
    private string status = string.Empty;

    public IAsyncRelayCommand DoWorkCommand { get; }

    public MainViewModel(IMyService service)
    {
        _service = service;
        FirstOptions = new ObservableCollection<ExampleEnum>
        {
            ExampleEnum.FirstOption,
            ExampleEnum.SecondOption,
            ExampleEnum.ThirdOption,
            ExampleEnum.FourthOption
        };
        SecondOptions = new ObservableCollection<string> { "A", "B", "C" };
        DoWorkCommand = new AsyncRelayCommand(DoWorkAsync);
    }

    private async Task DoWorkAsync()
    {
        Status = "Working...";
        var result = await _service.DoWorkAsync(SelectedFirst?.ToString(), SelectedSecond);
        Status = result;
    }
}