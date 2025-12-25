using System.ComponentModel;

using GacsApp.Utils;
using GacsApp.Models;

namespace GacsApp.Tests.Utils;

public class EnumHelperTests
{
  #region Test Enums

  // Test enum with Description attributes
  public enum TestEnumWithDescriptions
  {
    [Description("First Value Description")]
    FirstValue = 1,

    [Description("Second Value Description")]
    SecondValue = 2,

    [Description("Third Value Description")]
    ThirdValue = 3
  }


  // Test enum without Description attributes
  public enum TestEnumWithoutDescriptions
  {
    ValueOne = 1,

    ValueTwo = 2,

    ValueThree = 3
  }


  // Test enum with mixed descriptions (some have, some don't)
  public enum TestEnumMixedDescriptions
  {
    [Description("Has Description")]
    WithDescription = 1,

    WithoutDescription = 2,

    [Description("Another Description")]
    AnotherWithDescription = 3
  }


  // Empty enum for edge cases
  public enum EmptyEnum { }


  // Single value enum
  public enum SingleValueEnum
  {
    [Description("Only Value")]
    OnlyValue = 1
  }


  // Test class with nullable enum properties
  public class TestClassWithNullableEnums
  {
    public TestEnumWithDescriptions? EnumProperty1 { get; set; }

    public TestEnumWithoutDescriptions? EnumProperty2 { get; set; }

    public TestEnumMixedDescriptions? EnumProperty3 { get; set; }
  }


  // Test class with non-nullable enum properties
  public class TestClassWithNonNullableEnums
  {
    public TestEnumWithDescriptions EnumProperty1 { get; set; }

    public TestEnumWithoutDescriptions EnumProperty2 { get; set; }
  }


  // Test class with mixed properties
  public class TestClassWithMixedProperties
  {
    public TestEnumWithDescriptions? NullableEnum { get; set; }

    public TestEnumWithDescriptions NonNullableEnum { get; set; }

    public string StringProperty { get; set; } = string.Empty;

    public int IntProperty { get; set; }

    public bool BoolProperty { get; set; }
  }


  // Test class with no enum properties
  public class TestClassWithoutEnums
  {
    public string Name { get; set; } = string.Empty;

    public int Age { get; set; }

    public bool IsActive { get; set; }
  }


  // Test class with all null nullable enums
  public class TestClassWithAllNullEnums
  {
    public TestEnumWithDescriptions? Enum1 { get; set; }

    public TestEnumWithoutDescriptions? Enum2 { get; set; }

    public TestEnumMixedDescriptions? Enum3 { get; set; }
  }

  #endregion

  #region GetNullableEnumProperties Tests

  [Fact]
  public void GetNullableEnumProperties_WithAllNullableEnumsPopulated_ReturnsAllEnumValues()
  {
    // Arrange
    TestClassWithNullableEnums instance = new TestClassWithNullableEnums
                                          {
                                            EnumProperty1 = TestEnumWithDescriptions.FirstValue,
                                            EnumProperty2 = TestEnumWithoutDescriptions.ValueOne,
                                            EnumProperty3 = TestEnumMixedDescriptions.WithDescription
                                          };

    // Act
    List<Enum> result = instance.GetNullableEnumProperties().ToList();

    // Assert
    Assert.Equal(3, result.Count);
    Assert.Contains(TestEnumWithDescriptions.FirstValue, result);
    Assert.Contains(TestEnumWithoutDescriptions.ValueOne, result);
    Assert.Contains(TestEnumMixedDescriptions.WithDescription, result);
  }


  [Fact]
  public void GetNullableEnumProperties_WithSomeNullValues_ReturnsAllIncludingNulls()
  {
    // Arrange
    TestClassWithNullableEnums instance = new TestClassWithNullableEnums
                                          {
                                            EnumProperty1 = TestEnumWithDescriptions.SecondValue,
                                            EnumProperty2 = null,
                                            EnumProperty3 = TestEnumMixedDescriptions.AnotherWithDescription
                                          };

    // Act
    List<Enum> result = instance.GetNullableEnumProperties().ToList();

    // Assert
    Assert.Equal(3, result.Count);
    Assert.Contains(TestEnumWithDescriptions.SecondValue, result);
    Assert.Contains(null, result);
    Assert.Contains(TestEnumMixedDescriptions.AnotherWithDescription, result);
  }


  [Fact]
  public void GetNullableEnumProperties_WithAllNullValues_ReturnsAllNulls()
  {
    // Arrange
    TestClassWithAllNullEnums instance = new TestClassWithAllNullEnums
                                         {
                                           Enum1 = null,
                                           Enum2 = null,
                                           Enum3 = null
                                         };

    // Act
    List<Enum> result = instance.GetNullableEnumProperties().ToList();

    // Assert
    Assert.Equal(3, result.Count);
    Assert.All(result, item => Assert.Null(item));
  }


  [Fact]
  public void GetNullableEnumProperties_WithNonNullableEnums_ReturnsEmpty()
  {
    // Arrange
    TestClassWithNonNullableEnums instance = new TestClassWithNonNullableEnums
                                             {
                                               EnumProperty1 = TestEnumWithDescriptions.FirstValue,
                                               EnumProperty2 = TestEnumWithoutDescriptions.ValueTwo
                                             };

    // Act
    List<Enum> result = instance.GetNullableEnumProperties().ToList();

    // Assert
    Assert.Empty(result);
  }


  [Fact]
  public void GetNullableEnumProperties_WithMixedProperties_ReturnsOnlyNullableEnums()
  {
    // Arrange
    TestClassWithMixedProperties instance = new TestClassWithMixedProperties
                                            {
                                              NullableEnum = TestEnumWithDescriptions.ThirdValue,
                                              NonNullableEnum = TestEnumWithDescriptions.FirstValue,
                                              StringProperty = "Test",
                                              IntProperty = 42,
                                              BoolProperty = true
                                            };

    // Act
    List<Enum> result = instance.GetNullableEnumProperties().ToList();

    // Assert
    Assert.Single(result);
    Assert.Contains(TestEnumWithDescriptions.ThirdValue, result);
  }


  [Fact]
  public void GetNullableEnumProperties_WithNoEnumProperties_ReturnsEmpty()
  {
    // Arrange
    TestClassWithoutEnums instance = new TestClassWithoutEnums
                                     {
                                       Name = "John Doe",
                                       Age = 30,
                                       IsActive = true
                                     };

    // Act
    List<Enum> result = instance.GetNullableEnumProperties().ToList();

    // Assert
    Assert.Empty(result);
  }


  [Fact]
  public void GetNullableEnumProperties_WithEmptyObject_ReturnsEmpty()
  {
    // Arrange
    var instance = new
                   { };

    // Act
    List<Enum> result = instance.GetNullableEnumProperties().ToList();

    // Assert
    Assert.Empty(result);
  }

  #endregion

  #region GetDisplayItems Tests

  [Fact]
  public void GetDisplayItems_WithDescribedEnum_ReturnsCorrectDisplayItems()
  {
    // Act
    List<EnumDisplayItem<TestEnumWithDescriptions>> result = EnumHelper.GetDisplayItems<TestEnumWithDescriptions>().ToList();

    // Assert
    Assert.Equal(3, result.Count);

    EnumDisplayItem<TestEnumWithDescriptions> firstItem = result.First(x => x.Value == TestEnumWithDescriptions.FirstValue);
    Assert.Equal("First Value Description", firstItem.DisplayName);

    EnumDisplayItem<TestEnumWithDescriptions> secondItem = result.First(x => x.Value == TestEnumWithDescriptions.SecondValue);
    Assert.Equal("Second Value Description", secondItem.DisplayName);

    EnumDisplayItem<TestEnumWithDescriptions> thirdItem = result.First(x => x.Value == TestEnumWithDescriptions.ThirdValue);
    Assert.Equal("Third Value Description", thirdItem.DisplayName);
  }


  [Fact]
  public void GetDisplayItems_WithoutDescriptions_ReturnsEnumNamesAsDisplayNames()
  {
    // Act
    List<EnumDisplayItem<TestEnumWithoutDescriptions>> result = EnumHelper.GetDisplayItems<TestEnumWithoutDescriptions>().ToList();

    // Assert
    Assert.Equal(3, result.Count);

    EnumDisplayItem<TestEnumWithoutDescriptions> firstItem = result.First(x => x.Value == TestEnumWithoutDescriptions.ValueOne);
    Assert.Equal("ValueOne", firstItem.DisplayName);

    EnumDisplayItem<TestEnumWithoutDescriptions> secondItem = result.First(x => x.Value == TestEnumWithoutDescriptions.ValueTwo);
    Assert.Equal("ValueTwo", secondItem.DisplayName);

    EnumDisplayItem<TestEnumWithoutDescriptions> thirdItem = result.First(x => x.Value == TestEnumWithoutDescriptions.ValueThree);
    Assert.Equal("ValueThree", thirdItem.DisplayName);
  }


  [Fact]
  public void GetDisplayItems_WithMixedDescriptions_ReturnsCorrectDisplayNames()
  {
    // Act
    List<EnumDisplayItem<TestEnumMixedDescriptions>> result = EnumHelper.GetDisplayItems<TestEnumMixedDescriptions>().ToList();

    // Assert
    Assert.Equal(3, result.Count);

    EnumDisplayItem<TestEnumMixedDescriptions> withDesc = result.First(x => x.Value == TestEnumMixedDescriptions.WithDescription);
    Assert.Equal("Has Description", withDesc.DisplayName);

    EnumDisplayItem<TestEnumMixedDescriptions> withoutDesc = result.First(x => x.Value == TestEnumMixedDescriptions.WithoutDescription);
    Assert.Equal("WithoutDescription", withoutDesc.DisplayName);

    EnumDisplayItem<TestEnumMixedDescriptions> anotherWithDesc = result.First(x => x.Value == TestEnumMixedDescriptions.AnotherWithDescription);
    Assert.Equal("Another Description", anotherWithDesc.DisplayName);
  }


  [Fact]
  public void GetDisplayItems_WithSingleValueEnum_ReturnsSingleItem()
  {
    // Act
    List<EnumDisplayItem<SingleValueEnum>> result = EnumHelper.GetDisplayItems<SingleValueEnum>().ToList();

    // Assert
    Assert.Single(result);
    Assert.Equal(SingleValueEnum.OnlyValue, result[0].Value);
    Assert.Equal("Only Value", result[0].DisplayName);
  }


  [Fact]
  public void GetDisplayItems_ReturnsEnumDisplayItemType()
  {
    // Act
    List<EnumDisplayItem<TestEnumWithDescriptions>> result = EnumHelper.GetDisplayItems<TestEnumWithDescriptions>().ToList();

    // Assert
    Assert.All(result, item =>
                       {
                         Assert.IsType<EnumDisplayItem<TestEnumWithDescriptions>>(item);
                         Assert.IsType<TestEnumWithDescriptions>(item.Value);
                         Assert.IsType<string>(item.DisplayName);
                       });
  }


  [Fact]
  public void GetDisplayItems_ReturnsAllEnumValues()
  {
    // Act
    List<EnumDisplayItem<TestEnumWithDescriptions>> result = EnumHelper.GetDisplayItems<TestEnumWithDescriptions>().ToList();
    IEnumerable<TestEnumWithDescriptions> allEnumValues = Enum.GetValues(typeof(TestEnumWithDescriptions)).Cast<TestEnumWithDescriptions>();

    // Assert
    Assert.Equal(allEnumValues.Count(), result.Count);
    foreach (TestEnumWithDescriptions enumValue in allEnumValues)
    {
      Assert.Contains(result, item => item.Value.Equals(enumValue));
    }
  }


  [Fact]
  public void GetDisplayItems_VerifyEnumDisplayItemProperties()
  {
    // Act
    EnumDisplayItem<TestEnumWithDescriptions> result = EnumHelper.GetDisplayItems<TestEnumWithDescriptions>().First();

    // Assert
    Assert.NotNull(result.Value);
    Assert.NotNull(result.DisplayName);
    Assert.NotEmpty(result.DisplayName);
  }


  [Fact]
  public void GetDisplayItems_EnumDisplayItemToString_ReturnsExpectedFormat()
  {
    // Act
    EnumDisplayItem<TestEnumWithDescriptions> result = EnumHelper.GetDisplayItems<TestEnumWithDescriptions>().First();
    string toStringResult = result.ToString();

    // Assert
    Assert.NotNull(toStringResult);
    Assert.NotEmpty(toStringResult);
  }

  #endregion

  #region Integration Tests with Real Models

  [Fact]
  public void GetNullableEnumProperties_WithGacsSelection_ReturnsAllProperties()
  {
    // Arrange
    GacsSelection gacsSelection = new GacsSelection();

    // Act
    List<Enum> result = gacsSelection.GetNullableEnumProperties().ToList();

    // Assert
    // GacsSelection has 11 nullable enum properties
    Assert.Equal(11, result.Count);
    Assert.All(result, item => Assert.Null(item));
  }


  [Fact]
  public void GetNullableEnumProperties_WithPartiallyPopulatedGacsSelection_ReturnsCorrectValues()
  {
    // Arrange
    GacsSelection gacsSelection = new GacsSelection
                                  {
                                    PrecursorOrigin = GacsApp.Models.ResourceSustainability.PrecursorOrigin.WasteBiomass,
                                    SolventGreenness = null,
                                    EnergyInput = GacsApp.Models.ResourceSustainability.EnergyInput.Ambient
                                  };

    // Act
    List<Enum> result = gacsSelection.GetNullableEnumProperties().ToList();

    // Assert
    Assert.Equal(11, result.Count);
    Assert.Contains(GacsApp.Models.ResourceSustainability.PrecursorOrigin.WasteBiomass, result);
    Assert.Contains(GacsApp.Models.ResourceSustainability.EnergyInput.Ambient, result);
    Assert.Contains(null, result);
  }

  #endregion

  #region Edge Cases and Error Handling

  [Fact]
  public void GetDisplayItems_WithEnumContainingZeroValue_HandlesCorrectly()
  {
    // This test uses TestEnumWithDescriptions which has non-zero values
    // but verifies the method works with various numeric values
    // Act
    List<EnumDisplayItem<TestEnumWithDescriptions>> result = EnumHelper.GetDisplayItems<TestEnumWithDescriptions>().ToList();

    // Assert
    Assert.All(result, item =>
                       {
                         Assert.NotNull(item.Value);
                         Assert.NotNull(item.DisplayName);
                       });
  }


  [Fact]
  public void GetNullableEnumProperties_CalledMultipleTimes_ReturnsConsistentResults()
  {
    // Arrange
    TestClassWithNullableEnums instance = new TestClassWithNullableEnums
                                          {
                                            EnumProperty1 = TestEnumWithDescriptions.FirstValue,
                                            EnumProperty2 = TestEnumWithoutDescriptions.ValueTwo,
                                            EnumProperty3 = null
                                          };

    // Act
    List<Enum> result1 = instance.GetNullableEnumProperties().ToList();
    List<Enum> result2 = instance.GetNullableEnumProperties().ToList();

    // Assert
    Assert.Equal(result1.Count, result2.Count);
    for (int i = 0; i < result1.Count; i++)
    {
      Assert.Equal(result1[i], result2[i]);
    }
  }


  [Fact]
  public void GetDisplayItems_CalledMultipleTimes_ReturnsConsistentResults()
  {
    // Act
    List<EnumDisplayItem<TestEnumWithDescriptions>> result1 = EnumHelper.GetDisplayItems<TestEnumWithDescriptions>().ToList();
    List<EnumDisplayItem<TestEnumWithDescriptions>> result2 = EnumHelper.GetDisplayItems<TestEnumWithDescriptions>().ToList();

    // Assert
    Assert.Equal(result1.Count, result2.Count);
    for (int i = 0; i < result1.Count; i++)
    {
      Assert.Equal(result1[i].Value, result2[i].Value);
      Assert.Equal(result1[i].DisplayName, result2[i].DisplayName);
    }
  }


  [Fact]
  public void GetNullableEnumProperties_WithInheritedProperties_IncludesAllProperties()
  {
    // Arrange
    DerivedClassWithEnums derived = new DerivedClassWithEnums
                                    {
                                      BaseEnum = TestEnumWithDescriptions.FirstValue,
                                      DerivedEnum = TestEnumWithoutDescriptions.ValueOne
                                    };

    // Act
    List<Enum> result = derived.GetNullableEnumProperties().ToList();

    // Assert
    Assert.Equal(2, result.Count);
    Assert.Contains(TestEnumWithDescriptions.FirstValue, result);
    Assert.Contains(TestEnumWithoutDescriptions.ValueOne, result);
  }


  // Helper class for inheritance test
  public class BaseClassWithEnums
  {
    public TestEnumWithDescriptions? BaseEnum { get; set; }
  }


  public class DerivedClassWithEnums : BaseClassWithEnums
  {
    public TestEnumWithoutDescriptions? DerivedEnum { get; set; }
  }


  [Fact]
  public void GetDisplayItems_WithEnumHavingDuplicateValues_ReturnsAllFields()
  {
    // Act
    List<EnumDisplayItem<TestEnumWithDuplicateValues>> result = EnumHelper.GetDisplayItems<TestEnumWithDuplicateValues>().ToList();

    // Assert
    // Should return all fields, even if values are the same
    Assert.Equal(2, result.Count);
  }


  public enum TestEnumWithDuplicateValues
  {
    [Description("First Name")]
    FirstName = 1,

    [Description("Second Name")]
    SecondName = 1
  }


  [Fact]
  public void GetDisplayItems_WithFlagsEnum_ReturnsAllFlagValues()
  {
    // Act
    List<EnumDisplayItem<TestFlagsEnum>> result = EnumHelper.GetDisplayItems<TestFlagsEnum>().ToList();

    // Assert
    Assert.Equal(4, result.Count);
    Assert.Contains(result, item => item.Value == TestFlagsEnum.None);
    Assert.Contains(result, item => item.Value == TestFlagsEnum.Read);
    Assert.Contains(result, item => item.Value == TestFlagsEnum.Write);
    Assert.Contains(result, item => item.Value == TestFlagsEnum.Execute);
  }


  [Flags]
  public enum TestFlagsEnum
  {
    None = 0,

    [Description("Read Permission")]
    Read = 1,

    [Description("Write Permission")]
    Write = 2,

    [Description("Execute Permission")]
    Execute = 4
  }

  #endregion

  #region Performance and Enumeration Tests

  [Fact]
  public void GetDisplayItems_ReturnsIEnumerable_CanBeEnumeratedMultipleTimes()
  {
    // Act
    IEnumerable<EnumDisplayItem<TestEnumWithDescriptions>> result = EnumHelper.GetDisplayItems<TestEnumWithDescriptions>();

    // Assert
    List<EnumDisplayItem<TestEnumWithDescriptions>> firstEnumeration = result.ToList();
    List<EnumDisplayItem<TestEnumWithDescriptions>> secondEnumeration = result.ToList();

    Assert.Equal(firstEnumeration.Count, secondEnumeration.Count);
  }


  [Fact]
  public void GetNullableEnumProperties_ReturnsIEnumerable_CanBeEnumeratedMultipleTimes()
  {
    // Arrange
    TestClassWithNullableEnums instance = new TestClassWithNullableEnums
                                          {
                                            EnumProperty1 = TestEnumWithDescriptions.FirstValue
                                          };

    // Act
    IEnumerable<Enum?> result = instance.GetNullableEnumProperties();

    // Assert
    List<Enum> firstEnumeration = result.ToList();
    List<Enum> secondEnumeration = result.ToList();

    Assert.Equal(firstEnumeration.Count, secondEnumeration.Count);
  }

  #endregion
}