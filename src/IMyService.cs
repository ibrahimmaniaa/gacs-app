namespace gacs_app;

public interface IMyService
{
  Task<string> DoWorkAsync(string? opt1, string? opt2);
}