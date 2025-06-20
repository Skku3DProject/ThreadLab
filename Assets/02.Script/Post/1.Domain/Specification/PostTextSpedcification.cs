using System.Text.RegularExpressions;
using UnityEngine;

public class PostTextSpedcification : ISpecification<string>
{
    // 필요에 따라 추가하거나 제거할 수 있습니다.
    private static readonly string[] ForbiddenWords = { "욕설", "비속어", "광고", "불법" };

    // 본문의 최소/최대 길이를 정의합니다.
    private const int MaxBodyLength = 2000;
    public string ErrorMessage { get; private set; }

    public bool IsStatisfiedBy(string value)
    {
        // 1. 본문이 비어있는지 확인합니다.
        if (string.IsNullOrWhiteSpace(value))
        {
            ErrorMessage = "게시판 본문은 비어있을 수 없습니다.";
            return false;
        }

        // 2. 본문 길이 제한을 확인합니다.
        if ( value.Length > MaxBodyLength)
        {
            ErrorMessage = $"게시판 본문은 {MaxBodyLength}자 이하여야 합니다.";
            return false;
        }

        // 3. 금지어가 포함되어 있는지 확인합니다.
        foreach (var forbidden in ForbiddenWords)
        {
            if (value.Contains(forbidden))
            {
                ErrorMessage = $"게시판 본문에 부적절한 단어가 포함되어 있습니다: '{forbidden}'";
                return false;
            }
        }

        // 모든 조건을 만족하면 유효합니다.
        return true;
    }
}
