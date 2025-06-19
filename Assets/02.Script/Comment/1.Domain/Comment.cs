using Firebase.Firestore;
using UnityEngine;

[FirestoreData]
public class Comment
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

    public Comment(string commentUID, string postUID, string userName, string userEmail, string mainText, Timestamp timestamp)
    {
        CommentUID = commentUID;
        PostUID = postUID;
        UserName = userName;
        UserEmail = userEmail;
        MainText = mainText;
        Timestamp = timestamp;
    }
}
