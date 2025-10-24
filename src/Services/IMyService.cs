using GacsApp.Models.ResourceSustainability;

namespace GacsApp.Services;

public interface IMyService
{
  Task<int> DoWorkAsync(PrecursorOrigin opt1, SolventGreenness opt2);
}