using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Extensions;
using Firebase.Firestore;
using System;
using UnityEditor.Experimental.GraphView;

public class PostRepository : FirebaseRepositoryBase
{
    public async Task AddPost(PostDTO post)
    {
        await ExecuteAsync(() =>
            Firestore.Collection("posts").Document(post.ID).SetAsync(post.ToDictionary()), "게시글 저장");
    }

    public async Task UpdatePost(List<PostDTO> posts)
    {
        foreach(var pos in posts)
        {
            var post = pos.ToDictionary();
            DocumentReference docRef = Firestore.Collection("posts").Document(pos.ID);
            await ExecuteAsync(() => docRef.UpdateAsync(post), "Load");
        }
    }

    public async Task<List<Post>> GetPosts() 
    {
        var snapshot = await ExecuteAsync(() => Firestore.Collection("posts").GetSnapshotAsync(), "모든 게시글 불러오기");

        List<Post> postdto = new List<Post>();

        foreach (var doc in snapshot.Documents)
        {
            postdto.Add(new Post(doc.ToDictionary()));
        }

        return postdto;
    }

    public async Task<PostDTO> GetPost(string PostId)
    {
        await FirebaseManager.Instance.InitTask;
        return null;
    }
 
    public async Task DeletePost(PostDTO post)
    {
        await ExecuteAsync(() =>
           Firestore.Collection("posts").Document(post.ID).DeleteAsync(), "게시글 삭제");
    }
}
