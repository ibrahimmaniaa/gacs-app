using GacsApp.Models.ResourceSustainability;

namespace GacsApp.Services;

public class MyService : IMyService
{
  public Task<int> DoWorkAsync(PrecursorOrigin opt1, SolventGreenness opt2)
  {
      return Task.FromResult(Convert.ToInt32(opt1) + Convert.ToInt32(opt2));
  }
}