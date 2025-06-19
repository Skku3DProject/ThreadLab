using System;
using UnityEngine;

public class Comment
{
    // 읽기 전용 프로퍼티
    public string CommentUID { get; }
    public string PostUID { get; }
    public string UserName { get; }
    public string UserEmail { get; }
    public string MainText { get; private set; }
    public DateTime CreatedAt { get; }

    public Comment(string commentUID, string postUID, string userName, string userEmail, string mainText, DateTime createdAt)
    {
        var ContentSpecification = new CommentContentSpecification();

        if(!ContentSpecification.IsStatisfiedBy(mainText))
        {
            throw new Exception(ContentSpecification.ErrorMessage);
        }

        var emailSpecification = new AccountEmailSpecification();
        if(!emailSpecification.IsStatisfiedBy(userEmail))
        {
            throw new Exception(emailSpecification.ErrorMessage);
        }

        var nameSpecification = new AccountNameSpecification();
        if(!nameSpecification.IsStatisfiedBy(userName))
        {
            throw new Exception(nameSpecification.ErrorMessage); 
        }

        if (string.IsNullOrWhiteSpace(commentUID))
            throw new ArgumentException("댓글 UID는 필수입니다.");

        if (string.IsNullOrWhiteSpace(postUID))
            throw new ArgumentException("게시글 UID는 필수입니다.");

        CommentUID = commentUID;
        PostUID = postUID;
        UserName = userName.Trim();
        UserEmail = userEmail.Trim().ToLower();
        MainText = mainText.Trim();
        CreatedAt = createdAt;
    }

    // DTO로 변환
    public CommentDTO ToDTO()
    {
        return new CommentDTO(
            CommentUID,
            PostUID,
            UserName,
            UserEmail,
            MainText,
            Firebase.Firestore.Timestamp.FromDateTime(CreatedAt)
        );
    }

    // 소유자 확인
    public bool IsOwner(string userEmail)
    {
        var emailSpecification = new AccountEmailSpecification();
        if (!emailSpecification.IsStatisfiedBy(userEmail))
        {
            throw new Exception(emailSpecification.ErrorMessage);
        }

        return UserEmail.Equals(userEmail.Trim().ToLower(), StringComparison.OrdinalIgnoreCase);
    }

}
