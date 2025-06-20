using Firebase.Firestore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class PostRepository : FirebaseRepositoryBase
{
    public async Task AddPost(PostDTO post)
    {
        await ExecuteAsync(() =>
               Firestore.Collection("posts").Document(post.ID).SetAsync(post.ToDictionary()), "게시글 저장");
        //try
        //{
        //    Debug.Log("Repository AddPost 시작");
        //    await ExecuteAsync(() =>
        //        Firestore.Collection("posts").Document(post.ID).SetAsync(post.ToDictionary()), "게시글 저장");
        //    Debug.Log("Repository AddPost 완료");
        //}
        //catch (System.Exception e)
        //{
        //    Debug.LogError($"Repository AddPost 에러: {e.Message}");
        //    throw;
        //}
    }

    public async Task UpdatePost(PostDTO posts)
    {
        var post = posts.ToDictionary();
        DocumentReference docRef = Firestore.Collection("posts").Document(posts.ID);
        await ExecuteAsync(() => docRef.UpdateAsync(post), "UpdateAsync");
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
