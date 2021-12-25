using NomaNova.Ojeda.Utils.Services;
using NomaNova.Ojeda.Utils.Services.Interfaces;
using Xunit;

namespace NomaNova.Ojeda.Utils.Tests.Services;

public class Ean13SymbologyServiceTests
{
    private readonly IEan13SymbologyService _symbologyService;
    
    public Ean13SymbologyServiceTests()
    {
        _symbologyService = new Ean13SymbologyService();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("123123123123123")]
    [InlineData("123123")]
    [InlineData("5012345764215")] // invalid checksum
    public void IsValidFull_WhenInvalid_ShouldReturnFalse(string input)
    {
        Assert.False(_symbologyService.IsValidFull(input));
    }

    [Theory]
    [InlineData("1234567890128")]
    [InlineData("5012345764214")]
    [InlineData("0000000000017")]
    public void IsValidFull_WhenValid_ShouldReturnTrue(string input)
    {
        Assert.True(_symbologyService.IsValidFull(input));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("ABC")]
    [InlineData("123123123123123")]
    public void IsValid_WhenInvalid_ShouldReturnFalse(string input)
    {
        Assert.False(_symbologyService.IsValid(input));
    }

    [Theory]
    [InlineData("0")]
    [InlineData("1")]
    [InlineData("123456789012")]
    public void IsValid_WhenValid_ShouldReturnTrue(string input)
    {
        Assert.True(_symbologyService.IsValid(input));
    }

    [Theory]
    [InlineData(null, "0", "0000000000000")]
    [InlineData("5012345764214", "501234576422", "5012345764221")]
    [InlineData("0000000000000", "1", "0000000000017")]
    public void GenerateNext_WhenValidInput_ShouldReturnNext(string input, string output, string fullOutput)
    {
        var (outputResult, fullOutputResult) = _symbologyService.GenerateNext(input);
        
        Assert.Equal(output, outputResult);
        Assert.Equal(fullOutput, fullOutputResult);
    }

    [Theory]
    [InlineData("0", true, "0000000000000")]
    [InlineData("000000000001", true, "0000000000017")]
    [InlineData("123456789012", true, "1234567890128")]
    [InlineData("501234576421", true, "5012345764214")]
    [InlineData("1234567890128", true, "1234567890128")]
    [InlineData("0000000000017", true, "0000000000017")]
    public void ValidateAndFormatFull_WhenValidInput_ShouldProvideValidOutput(string input, bool isValid, string output)
    {
        var (isValidResult, outputResult) = _symbologyService.ValidateAndFormatFull(input);
        
        Assert.Equal(isValid, isValidResult);
        Assert.Equal(output, outputResult);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("0000000000019")]
    [InlineData("00000000000171")]
    [InlineData("ABC123456789")]
    public void ValidateAndFormatFull_WhenInvalidInput_ShouldReturnFalse(string input)
    {
        var (isValidResult, outputResult) = _symbologyService.ValidateAndFormatFull(input);
        
        Assert.False(isValidResult);
        Assert.Null(outputResult);
    }

    [Theory]
    [InlineData("0", true, "0")]
    [InlineData("000000000001", true, "1")]
    [InlineData("0000000000017", true, "1")]
    [InlineData("0000000000000", true, "0")]
    [InlineData("501234576421", true, "501234576421")]
    [InlineData("5012345764214", true, "501234576421")]
    public void ValidateAndFormat_WhenValidInput_ShouldProvideValidOutput(string input, bool isValid, string output)
    {
        var (isValidResult, outputResult) = _symbologyService.ValidateAndFormat(input);
        
        Assert.Equal(isValid, isValidResult);
        Assert.Equal(output, outputResult);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("0000000000019")]
    [InlineData("00000000000171")]
    [InlineData("ABC123456789")]
    public void ValidateAndFormat_WhenInvalidInput_ShouldReturnFalse(string input)
    {
        var (isValidResult, outputResult) = _symbologyService.ValidateAndFormat(input);
        
        Assert.False(isValidResult);
        Assert.Null(outputResult);
    }
}