using Firebase.Firestore;
using UnityEngine;
[FirestoreData]
public class CommentDTO
{
    [FirestoreProperty]
    public string CommentUID { get; set; }

    [FirestoreProperty]
    public string PostUID { get; set; }

    [FirestoreProperty]
    public string UserName { get; set; }

    [FirestoreProperty]
    public string UserEmail { get; set; }

    [FirestoreProperty]
    public string MainText { get; set; }

    [FirestoreProperty]
    public Timestamp Timestamp { get; set; }

    // Firebase 직렬화를 위한 기본 생성자
    public CommentDTO() { }

    // DTO 생성자
    public CommentDTO(string commentUID, string postUID, string userName, string userEmail, string mainText, Timestamp timestamp)
    {
        CommentUID = commentUID;
        PostUID = postUID;
        UserName = userName;
        UserEmail = userEmail;
        MainText = mainText;
        Timestamp = timestamp;
    }
}

