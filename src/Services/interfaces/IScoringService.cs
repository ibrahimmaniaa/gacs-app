using GacsApp.Models;


namespace GacsApp.Services.interfaces;

public interface IScoringService
{
  Task<int> CalculateTotalScoreAsync(GacsSelection selection);
}