using UnityEngine;
using System.Threading.Tasks;
using Firebase.Firestore;
using System.Collections.Generic;
public class CommentRepository : FirebaseRepositoryBase
{
    private const string COLLECTION_NAME = "comments";

    public async Task<List<CommentDTO>> LoadAllComments()
    {
        return await ExecuteAsync(async () =>
        {
            var snapshot = await Firestore.Collection(COLLECTION_NAME).GetSnapshotAsync();

            var comments = new List<CommentDTO>();
            foreach (var document in snapshot.Documents)
            {
                if (document.Exists)
                {
                    var comment = document.ConvertTo<CommentDTO>();
                    comments.Add(comment);
                }
            }

            return comments;

        }, "¸ðµç ´ñ±Û ·Îµå");
    }

    public async Task<CommentDTO> PostComment(CommentDTO commentDTO)
    {
        return await ExecuteAsync(async () =>
        {
            var docRef = Firestore.Collection(COLLECTION_NAME).Document(commentDTO.CommentUID);
            await docRef.SetAsync(commentDTO);
            return commentDTO;
        }, "´ñ±Û ÀúÀå");
    }

    public async Task DeleteComment(string commentUID)
    {
        await ExecuteAsync(async () =>
        {
            var docRef = Firestore.Collection(COLLECTION_NAME).Document(commentUID);
            await docRef.DeleteAsync();
        }, "´ñ±Û »èÁ¦");
    }
}
