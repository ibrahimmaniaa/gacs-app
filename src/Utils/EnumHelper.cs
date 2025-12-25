using System.ComponentModel;
using System.Reflection;

using GacsApp.Models;


namespace GacsApp.Utils;

public static class EnumHelper
{
  public static IEnumerable<Enum?> GetNullableEnumProperties(this object instance)
  {
    return instance.GetType()
           .GetProperties(BindingFlags.Public | BindingFlags.Instance)
           .Where(property => Nullable.GetUnderlyingType(property.PropertyType)?.IsEnum ?? false)
           .Select(property => (Enum?)property.GetValue(instance));
  }


  public static IEnumerable<EnumDisplayItem<T>> GetDisplayItems<T>() where T : Enum
  {
    return typeof(T)
           .GetFields(BindingFlags.Public | BindingFlags.Static)
           .Select(field =>
                   {
                     T value = (T)field.GetValue(null)!;
                     return new EnumDisplayItem<T>(value, GetDisplayName(value));
                   });
  }



  private static string GetDisplayName<T>(T value) where T : Enum
  {
    FieldInfo? field = value.GetType().GetField(value.ToString());

    if (field == null) throw new InvalidOperationException($"Enum field '{value.ToString()}' is invalid.");

    DescriptionAttribute? attribute = (DescriptionAttribute?)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
    return attribute?.Description ?? value.ToString();
  }
}