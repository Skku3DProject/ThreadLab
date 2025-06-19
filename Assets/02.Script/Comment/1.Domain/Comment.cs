using System;
using UnityEngine;

public class Comment
{
    // �б� ���� ������Ƽ
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
            throw new ArgumentException("��� UID�� �ʼ��Դϴ�.");

        if (string.IsNullOrWhiteSpace(postUID))
            throw new ArgumentException("�Խñ� UID�� �ʼ��Դϴ�.");

        CommentUID = commentUID;
        PostUID = postUID;
        UserName = userName.Trim();
        UserEmail = userEmail.Trim().ToLower();
        MainText = mainText.Trim();
        CreatedAt = createdAt;
    }

    // DTO�� ��ȯ
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

    // ������ Ȯ��
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
