using UnityEngine;
using System.Threading.Tasks;
using Firebase.Firestore;
using System.Collections.Generic;
public class CommentRepository : FirebaseRepositoryBase
{
    public async Task<Comment> PostComment(string postId, string username,string useremail, string content)
    {
        Comment newComment = new Comment
            (
                commentUID: "",  // ���� ����
                postUID: postId,
                userName: username,
                userEmail: useremail,
                mainText: content,
                timestamp: Timestamp.GetCurrentTimestamp()
            );

        DocumentReference docRef = await Firestore.Collection("comments").AddAsync(newComment);

        newComment.CommentUID = docRef.Id;
        await ExecuteAsync(() => docRef.UpdateAsync("CommentUID", docRef.Id), "��� �ۼ�");

        return newComment;
    }
    public async Task DeleteComment(string commentID)
    {
        await ExecuteAsync(() => Firestore.Collection("comments").Document(commentID).DeleteAsync(), "��ۻ���");
    }

    public async Task<List<Comment>> LoadAllComments()
    {
        var snapshot = await ExecuteAsync(() => Firestore.Collection("comments").GetSnapshotAsync(), "��� ���� �ҷ�����");

        var comments = new List<Comment>();

        foreach (var doc in snapshot.Documents)
        {
            Comment comment = new Comment(
                        commentUID: doc.Id,
                        postUID: doc.TryGetValue<string>("PostUID", out var postId) ? postId : "",
                        userName: doc.TryGetValue<string>("UserName", out var name) ? name : "",
                        userEmail: doc.TryGetValue<string>("UserEmail", out var email) ? email : "",
                        mainText: doc.TryGetValue<string>("MainText", out var text) ? text : "",
                        timestamp: doc.TryGetValue<Timestamp>("Timestamp", out var time) ? time : Timestamp.GetCurrentTimestamp()
                    );

            comments.Add(comment);
        }

        return comments;
    }

}
