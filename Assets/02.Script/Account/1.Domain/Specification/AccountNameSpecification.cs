using System.Text.RegularExpressions;

public class AccountNameSpecification : ISpecification<string>
{
    private static readonly string[] ForbiddenNicknames = { "¹Ùº¸", "¸ÛÃ»ÀÌ", "¿î¿µÀÚ", "±èÁ¤Àº" };
    private static readonly Regex NicknameRegex = new Regex(@"^[°¡-ÆRa-zA-Z]{2,10}$", RegexOptions.Compiled);
    public string ErrorMessage { get; private set; }

    public bool IsStatisfiedBy(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            ErrorMessage = ("´Ğ³×ÀÓÀº ºñ¾îÀÖÀ» ¼ö ¾ø½À´Ï´Ù.");
            return false;
        }

        if (!NicknameRegex.IsMatch(value))
        {
            ErrorMessage = ("´Ğ³×ÀÓÀº 2ÀÚ ÀÌ»ó 10ÀÚ ÀÌÇÏÀÇ ÇÑ±Û ¶Ç´Â ¿µ¹®ÀÌ¾î¾ß ÇÕ´Ï´Ù.");
            return false;
        }

        foreach (var forbidden in ForbiddenNicknames)
        {
            if (value.Contains(forbidden))
            {
                ErrorMessage = ($"´Ğ³×ÀÓ¿¡ ºÎÀûÀıÇÑ ´Ü¾î°¡ Æ÷ÇÔµÇ¾î ÀÖ½À´Ï´Ù: {forbidden}");
                return false;
            }
        }

        return true;
    }
}
