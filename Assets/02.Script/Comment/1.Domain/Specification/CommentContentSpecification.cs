using UnityEngine;

public class CommentContentSpecification : ISpecification<string>
{
    public string ErrorMessage { get; private set; }

    public bool IsStatisfiedBy(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            ErrorMessage = "��� ������ ������� �� �����ϴ�.";
            return false;
        }

        if (content.Length > 500)
        {
            ErrorMessage = "����� 500�ڸ� �ʰ��� �� �����ϴ�.";
            return false;
        }

        if (content.Trim().Length < 2)
        {
            ErrorMessage = "����� �ּ� 2���� �̻��̾�� �մϴ�.";
            return false;
        }

        return true;
    }
}
