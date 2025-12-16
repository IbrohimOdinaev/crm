using crm.DataAccess.Enums;
using static crm.Presentation.Helpers.BaseHelpers;


namespace crm.test.PresentationTests.HelpersTests;

public class BaseHelpersTests
{
    [Theory]
    [InlineData("42", 42)]
    [InlineData("Pending", OrderStatus.Pending)]
    [InlineData("hello", "hello")]
    public async Task GetString_ValidValues_ShouldReturnInputTypes<T>(string input, T expected)
    {
        Console.SetIn(new StringReader(input));
        var sw = new StringWriter();
        Console.SetOut(sw);

        var result = await GetStringAsync<T>("Test", default);

        Assert.Equal(expected, result);
    }
} 