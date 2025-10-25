using GacsApp.Models;
using GacsApp.Models.ResourceSustainability;

namespace GacsApp.Services;

public interface IScoringService
{
  Task<int> CalculateTotalScoreAsync(GacsSelection selection);
}