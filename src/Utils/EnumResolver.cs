using System.ComponentModel;
using System.Reflection;

namespace GacsApp.Utils;

public record EnumDisplayItem<T>(T Value, string DisplayName)
{
    public override string ToString() => DisplayName;
}

public static class EnumResolver
{
    public static IEnumerable<EnumDisplayItem<T>> GetDisplayItems<T>() where T : Enum =>
        Enum.GetValues(typeof(T))
            .Cast<T>()
            .Select(value => new EnumDisplayItem<T>(value, GetDisplayName(value)));

    private static string GetDisplayName<T>(T value) where T : Enum
    {
        FieldInfo? field = value.GetType().GetField(value.ToString());
        if (field == null) throw new InvalidOperationException();
        DescriptionAttribute? attr = (DescriptionAttribute?)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
        return attr?.Description ?? value.ToString();
    }
}
