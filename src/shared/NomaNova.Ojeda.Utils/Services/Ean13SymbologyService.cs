using System;
using System.Text.RegularExpressions;
using NomaNova.Ojeda.Utils.Services.Interfaces;

namespace NomaNova.Ojeda.Utils.Services;

/**
 * EAN13 minimal representation: 0, 1, 123...
 * EAN13 full representation: 0000000000000, 0000000000017...
 */
public class Ean13SymbologyService : IEan13SymbologyService
{
    private const string Ean13Default = "0";
    private const string Ean13DefaultFull = "0000000000000";

    private const string Ean13MinimalRegexExpression = "^[0-9]{1,12}$";
    private const string Ean13FullRegexExpression = "^[0-9]{13}$";

    private readonly Regex _ean13MinimumRegex;
    private readonly Regex _ean13FullRegex;

    public Ean13SymbologyService()
    {
        _ean13MinimumRegex = new Regex(Ean13MinimalRegexExpression);
        _ean13FullRegex = new Regex(Ean13FullRegexExpression);
    }

    /**
     * Validates the minimal representation of the asset id.
     * Accept any number of up to 12 digits.
     */
    public bool IsValid(string input)
    {
        return !string.IsNullOrEmpty(input) && _ean13MinimumRegex.IsMatch(input);
    }

    /**
     * Validates the full representation of the asset id.
     * Accept numbers of 13 digits with a valid EAN13 check digit.
     */
    public bool IsValidFull(string input)
    {
        if (string.IsNullOrEmpty(input) || !_ean13FullRegex.IsMatch(input))
        {
            return false;
        }

        var ean13 = input[..12];
        var checkDigit = Convert.ToInt32(input[12..]);

        var calculatedCheckDigit = CalculateCheckDigit(ean13);

        return calculatedCheckDigit == checkDigit;
    }

    /**
     * Accepts an existing full 13 digit EAN13,
     * and generates the numerically next possible value.
     * When null is used as input, the default values are returned.
     */
    public (string output, string fullOutput) GenerateNext(string input)
    {
        if (input == null)
        {
            return (Ean13Default, Ean13DefaultFull);
        }

        if (!IsValidFull(input))
        {
            throw new ArgumentException("Invalid EAN13 value", nameof(input));
        }

        var value = Convert.ToInt64(input[..12]);
        var nextValue = value + 1;
        var nextValuePadded = nextValue.ToString().PadLeft(12, '0');

        var checkDigit = CalculateCheckDigit(nextValuePadded);

        return (nextValue.ToString(), $"{nextValuePadded}{checkDigit}");
    }

    /**
     * Accepts both a minimal and full form,
     * and always returns a minimal form when valid.
     */
    public (bool isValid, string output) ValidateAndFormat(string input)
    {
        if (IsValid(input))
        {
            var ean13Trimmed = input.TrimStart('0');

            if (string.IsNullOrEmpty(ean13Trimmed))
            {
                ean13Trimmed = Ean13Default;
            }

            return (true, ean13Trimmed);
        }

        if (IsValidFull(input))
        {
            var ean13WithoutCheckDigit = input[..12];
            var ean13Trimmed = ean13WithoutCheckDigit.TrimStart('0');

            if (string.IsNullOrEmpty(ean13Trimmed))
            {
                ean13Trimmed = Ean13Default;
            }

            return (true, ean13Trimmed);
        }

        return (false, null);
    }

    /**
     * Accepts both a minimal and full form,
     * and always returns a full form when valid.
     */
    public (bool isValid, string fullOutput) ValidateAndFormatFull(string input)
    {
        if (IsValid(input))
        {
            return (true, FormatWithCheckDigit(input));
        }

        if (IsValidFull(input))
        {
            return (true, input);
        }

        return (false, null);
    }

    /**
     * Accept up to 12 digits as input.
     * Add '0' padding digits on the left when input is less than 12 digits.
     * Add a 13th EAN13 checksum digit.
     */
    private string FormatWithCheckDigit(string input)
    {
        if (!IsValid(input))
        {
            throw new ArgumentException("Invalid EAN13 value", nameof(input));
        }

        var paddedEan13 = input.PadLeft(12, '0');
        var checkDigit = CalculateCheckDigit(paddedEan13);

        return $"{paddedEan13}{checkDigit}";
    }

    private static int CalculateCheckDigit(string input)
    {
        var sum = 0;

        for (var i = input.Length; i >= 1; i--)
        {
            var digit = Convert.ToInt32(input.Substring(i - 1, 1));

            // This appears to be backwards but the EAN-13 checksum must be calculated
            // this way to be compatible with UPC-A.
            if (i % 2 == 0)
            {
                sum += digit * 3; // Odd
            }
            else
            {
                sum += digit * 1; // Even
            }
        }

        return (10 - (sum % 10)) % 10;
    }
}