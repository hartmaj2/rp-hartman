using System.ComponentModel.DataAnnotations;

// Used for custom validation of a birth number (correct lentgth, digits only, divisibility by 11, digits represent valid date)
public class ValidBirthNumberAttribute : ValidationAttribute
{

    // Only support 10 digit Czech birth numbers (these are the ones issued after 1.1.1954)
    private const int CorrectDigitsCount = 10;

    // Used by ParticipantFormData BirthNumber setter to make sure that the birth number is valid before computing the age automatically
    public static ValidBirthNumberAttribute Instance = new();

    // Here for some reason the form was not validating properly when using ValidationResult overload of the IsValid method
    // If I returned new ValidationResult with an error message, the form still showed the box as green even though the error message was displayed
    // That is the reason I am using bool IsValid override instead
    public override bool IsValid(object? value)
    {
        if (value is string stringValue)
        {
            if (stringValue == String.Empty) return true; // allow also when user doesn't enter any birth number
            if (stringValue.Length != CorrectDigitsCount)
            {
                ErrorMessage = $"Rodné číslo musí obsahovat přesně {CorrectDigitsCount} cifer.";
                return false;
            }
            if (!ContainsOnlyDigits(stringValue))
            {
                ErrorMessage = "Rodné číslo musí obsahovat pouze cifry.";
            }
            if (!IsDivisibleByEleven(stringValue))
            {
                ErrorMessage = "Rodné číslo musí být dělitelné číslem 11.";
                return false;
            }
            if (!RepresentsValidDate(stringValue))
            {
                ErrorMessage = "Část rodného čísla odpovídající datu není platná.";
                return false;        
            }
            return true;
        }
        ErrorMessage = "Z neznámého důvodu toto rodné číslo není platné.";
        return false;
    }

    private static bool ContainsOnlyDigits(string birthNumber)
    {
        foreach (var chr in birthNumber)
        {
            if (!char.IsAsciiDigit(chr)) return false;
        }
        return true;
    }

    // Check divisibility by the following rule:
    //  1. take the sum of digits on even numbered places (indexing from 0)
    //  2. subtract the sum of digits at odd numbered places
    //  3. check if the result is divisible by 11
    private static bool IsDivisibleByEleven(string birthNumber)
    {
        int checkSum = 0;
        int signChoice = 1; // this variable will flip between -1 and 1
        foreach (var chr in birthNumber)
        {
            checkSum += signChoice * (chr - '0');
            signChoice *= -1; // flip the sign
        }
        return checkSum % 11 == 0;
    }

    // Check if date is valid by using the built-in DateOnly.TryParseExact function    
    private static bool RepresentsValidDate(string birthNumber)
    {
        var month = ParseMonth(birthNumber[2..4]).ToString("D2"); // D2 tells the parser to make the number at least 2 digits long
        string dateString = "20" + birthNumber[0..2] + month.ToString() + birthNumber[4..6];
        return DateOnly.TryParseExact(dateString,"yyyyMMdd",out _);
    }

    // Parse the month string according to rules of czech birth numbers
    // for girls they can be 51-62 or 71-82
    // for boys they can be 01-12 or 21-32
    private static int ParseMonth(string monthString)
    {
        var month = int.Parse(monthString);
        if (month >= 51) month -= 50;
        if (month >= 21) month -= 20;
        return month;     
    }

}

public class StrictValidBirthNumberAttribute : ValidBirthNumberAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is string strval)
        {
            if (strval == String.Empty) 
            {
                ErrorMessage = "Rodné číslo účastníka musí být zadáno.";
                return false;
            }
        }
        return base.IsValid(value);
    }
}

public class SwitchableBirthNumberValidationAttribute : ValidationAttribute
{

    private static readonly StrictValidBirthNumberAttribute StrictValidator = new();
    private static readonly ValidBirthNumberAttribute LenientValidator = new();

    public static bool UseStrictValidator { get; set; } = true;

    public override bool IsValid(object? value)
    {
        // ErrorMessage = UseStrictValidator ? StrictValidator.ErrorMessage : LenientValidator.ErrorMessage;
        // ErrorMessageString = UseStrictValidator ? StrictValidator.ErrorMessage : LenientValidator.ErrorMessage;
        // ErrorMessage = "Switchable validator error";
        if (UseStrictValidator)
        {
            if (!StrictValidator.IsValid(value))
            {
                ErrorMessage = StrictValidator.ErrorMessage;
                return false;
            }
            return true;
        }
        else
        {
            if (!LenientValidator.IsValid(value))
            {
                ErrorMessage = LenientValidator.ErrorMessage;
                return false;
            }
            return true;  
        }
    }

}

// Implements name validation with more accurate error messages than if I used a single regex pattern match
public class ValidNameAttribute : ValidationAttribute
{

    // The separators we allow in a name are space, hyphen-minus or single quote (Johan Sebastian, Jane-Anne, O'Brian)
    private static readonly char[] Separators = [' ','-','\''];

    // Backing field used to print custom error messages based on which property is being validated
    private string _validatedPropertyName;

    public ValidNameAttribute(string validatedPropertyName)
    {
        _validatedPropertyName = validatedPropertyName;
    }

    public override bool IsValid(object? value)
    {
        var stringValue = (string) value!;
        var words = stringValue.Split(Separators,StringSplitOptions.None);
        foreach (var word in words)
        {
            if (word.Length == 0) // if we got an empty string after a split, there must have been more separators in a row
            {
                ErrorMessage = $"{_validatedPropertyName} nesmí obsahovat více než jeden oddělovač v řadě.";
                return false;
            }
            for (int i = 0; i < word.Length; i++) // got through each letter and check if it is valid
            {
                ErrorMessage = null;
                if (char.IsDigit(word[i]))
                {
                    ErrorMessage = $"{_validatedPropertyName} nesmí obsahovat cifry.";
                }
                else if (char.IsSymbol(word[i]))
                {
                    ErrorMessage = $"{_validatedPropertyName} nesmí obsahovat symboly.";
                }
                else if (char.IsPunctuation(word[i]))
                {
                    if (i != word.Length - 1)
                    {
                        ErrorMessage = $"{_validatedPropertyName} může obsahovat diakritiku pouze na konci slov.";
                    }
                    if (word[i] != '.') // . can be in names that contain Jr. or Bc. or similar
                    {
                        ErrorMessage = $"Jediná povolená diakritika ve {_validatedPropertyName} je . nebo ' ";
                    }
                }
                else if (!char.IsLetter(word[i]))
                {
                    ErrorMessage = $"{_validatedPropertyName} nesmí obsahovat speciální znaky.";
                }
                if (ErrorMessage is not null) // there was some error becuase we set the ErrorMessage to something
                {
                    return false;
                }
            }
        }
        return true;
        

    }
}


// Validates phone number based on if there is + at the start or not
// + at start is automatically removed by the ParticipantForm PhoneNumber setter if the number starts with +420 so we know that Czech numbers don't have + at start
public class ValidPhoneNumberAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        var stringValue = (string) value!;
        bool isForeign = false;
        if (stringValue[0] == '+')
        {
            stringValue = stringValue[1..]; // remove + if it was at start
            isForeign = true; // only foreign numbers are allowed to start with +
        }
        if (!ContainsOnlyDigits(stringValue))
        {
            ErrorMessage = "Telefonní číslo nesmí obsahovat žádné jiné symboly než je +.";
            return false;
        }
        if (isForeign) // is foreign phone number
        {
            if (stringValue.Length < 7 || stringValue.Length > 15)
            {
                ErrorMessage = "Toto mezinárodní telefonní číslo má neplatnou délku.";
                return false;
            }
        }
        else // is czech phone number
        {
            if (stringValue.Length != 9)
            {
                ErrorMessage = "Česká telefoní čísla musí mít přesně 9 cifer.";
                return false;
            }
        }
        return true;


    }

    private static bool ContainsOnlyDigits(string phoneNumber)
    {
        foreach (var chr in phoneNumber)
        {
            if (!char.IsAsciiDigit(chr))
            {
                return false;
            }
        }
        return true;
    }

}

// Custom IntegerRangeValidator 
// (it is better than the built-in because it allows me to show custom error messages based on the range I provided without having to hard code the number range inside the message)
public class IntegerRangeValidator : ValidationAttribute
{
    private int _minValue;
    private int _maxValue;

    // Backing field used to print custom error messages based on which property is being validated
    private string _validatedPropertyName;

    public IntegerRangeValidator(int min, int max, string propertyName)
    {
        _minValue = min;
        _maxValue = max;
        _validatedPropertyName = propertyName;
    }

    public override bool IsValid(object? value)
    {
        if (value is int intValue)
        {
            if (intValue >= _minValue && intValue <= _maxValue)
            {
                return true;
            }
        }
        ErrorMessage = $"{_validatedPropertyName} musí mít hodnotu v rozmezí od {_minValue} do {_maxValue}";
        return false;
    }
}