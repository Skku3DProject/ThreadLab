using UnityEngine;

public class CommentContentSpecification : ISpecification<string>
{
    public string ErrorMessage { get; private set; }

    public bool IsStatisfiedBy(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            ErrorMessage = "댓글 내용은 비어있을 수 없습니다.";
            return false;
        }

        if (content.Length > 500)
        {
            ErrorMessage = "댓글은 500자를 초과할 수 없습니다.";
            return false;
        }

        if (content.Trim().Length < 2)
        {
            ErrorMessage = "댓글은 최소 2글자 이상이어야 합니다.";
            return false;
        }

        return true;
    }
}
