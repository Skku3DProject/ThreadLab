using System.Text.RegularExpressions;
using UnityEngine;

public class PostTextSpedcification : ISpecification<string>
{
    // �ʿ信 ���� �߰��ϰų� ������ �� �ֽ��ϴ�.
    private static readonly string[] ForbiddenWords = { "�弳", "��Ӿ�", "����", "�ҹ�" };

    // ������ �ּ�/�ִ� ���̸� �����մϴ�.
    private const int MaxBodyLength = 2000;
    public string ErrorMessage { get; private set; }

    public bool IsStatisfiedBy(string value)
    {
        // 1. ������ ����ִ��� Ȯ���մϴ�.
        if (string.IsNullOrWhiteSpace(value))
        {
            ErrorMessage = "�Խ��� ������ ������� �� �����ϴ�.";
            return false;
        }

        // 2. ���� ���� ������ Ȯ���մϴ�.
        if ( value.Length > MaxBodyLength)
        {
            ErrorMessage = $"�Խ��� ������ {MaxBodyLength}�� ���Ͽ��� �մϴ�.";
            return false;
        }

        // 3. ����� ���ԵǾ� �ִ��� Ȯ���մϴ�.
        foreach (var forbidden in ForbiddenWords)
        {
            if (value.Contains(forbidden))
            {
                ErrorMessage = $"�Խ��� ������ �������� �ܾ ���ԵǾ� �ֽ��ϴ�: '{forbidden}'";
                return false;
            }
        }

        // ��� ������ �����ϸ� ��ȿ�մϴ�.
        return true;
    }
}
