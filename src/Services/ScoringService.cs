using GacsApp.Models;
using GacsApp.Services.interfaces;
using GacsApp.Utils;


namespace GacsApp.Services;

public class ScoringService : IScoringService
{
    public Task<int> CalculateTotalScoreAsync(GacsSelection selection)
    {
        return Task.FromResult(selection.GetNullableEnumProperties()
            .Where(v => v != null)
            .Sum(Convert.ToInt32));
    }
}