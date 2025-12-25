namespace GacsApp.Models;

public record EnumDisplayItem<T>(T Value, string DisplayName)
{
  public override string ToString() => DisplayName;
}