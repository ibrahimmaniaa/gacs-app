using GacsApp.Models;
using GacsApp.Services.interfaces;

namespace GacsApp.Services;

public class ScoringService : IScoringService
{
    public Task<int> CalculateTotalScoreAsync(GacsSelection selection)
    {
        return Task.FromResult(selection.GetAllSelectedEnums()
            .Where(v => v != null)
            .Sum(Convert.ToInt32));
    }
}