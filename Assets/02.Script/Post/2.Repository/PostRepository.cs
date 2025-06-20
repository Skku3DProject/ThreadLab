using Firebase.Firestore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class PostRepository : FirebaseRepositoryBase
{
    public async Task AddPost(PostDTO post)
    {
        await ExecuteAsync(() =>
               Firestore.Collection("posts").Document(post.ID).SetAsync(post.ToDictionary()), "�Խñ� ����");
        //try
        //{
        //    Debug.Log("Repository AddPost ����");
        //    await ExecuteAsync(() =>
        //        Firestore.Collection("posts").Document(post.ID).SetAsync(post.ToDictionary()), "�Խñ� ����");
        //    Debug.Log("Repository AddPost �Ϸ�");
        //}
        //catch (System.Exception e)
        //{
        //    Debug.LogError($"Repository AddPost ����: {e.Message}");
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
        var snapshot = await ExecuteAsync(() => Firestore.Collection("posts").GetSnapshotAsync(), "��� �Խñ� �ҷ�����");

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
           Firestore.Collection("posts").Document(post.ID).DeleteAsync(), "�Խñ� ����");
    }
}
