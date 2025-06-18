using System.Text.RegularExpressions;

public class AccountNameSpecification : ISpecification<string>
{
    private static readonly string[] ForbiddenNicknames = { "�ٺ�", "��û��", "���", "������" };
    private static readonly Regex NicknameRegex = new Regex(@"^[��-�Ra-zA-Z]{2,10}$", RegexOptions.Compiled);
    public string ErrorMessage { get; private set; }

    public bool IsStatisfiedBy(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            ErrorMessage = ("�г����� ������� �� �����ϴ�.");
            return false;
        }

        if (!NicknameRegex.IsMatch(value))
        {
            ErrorMessage = ("�г����� 2�� �̻� 10�� ������ �ѱ� �Ǵ� �����̾�� �մϴ�.");
            return false;
        }

        foreach (var forbidden in ForbiddenNicknames)
        {
            if (value.Contains(forbidden))
            {
                ErrorMessage = ($"�г��ӿ� �������� �ܾ ���ԵǾ� �ֽ��ϴ�: {forbidden}");
                return false;
            }
        }

        return true;
    }
}
