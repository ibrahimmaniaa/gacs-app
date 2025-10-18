namespace GacsApp.Services;

public class MyService : IMyService
{
  public async Task<string> DoWorkAsync(string? opt1, string? opt2)
  {
    await Task.Delay(3000); // simulate I/O or network operation
    return $"Done: {opt1 ?? "n/a"} + {opt2 ?? "n/a"} at {DateTime.Now:T}";
  }
}