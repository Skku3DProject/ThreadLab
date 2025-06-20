using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PostManager : MonoBehaviourSingleton<PostManager>
{
    private List<Post> _postList = new List<Post>();
    public List<PostDTO> PostList => _postList.ConvertAll(p => p.ToDTO());

    private PostRepository _postRepository;

    public PostDTO CurrentPost;

    public event Action OnDataChanged;
    public event Action<string> OnShowDetail;

    protected override void Awake()
    {
        base.Awake();
    }

    private async void Start()
    {
        await InitAsync();
    }

    private async Task InitAsync()
    {
        _postRepository = new PostRepository();
        await UpdatePost();
    }

    public async Task AddPost(string text)
    {
        Account account = AccountManager.Instance.MyAccount;
        Post post = new Post(account.Email + DateTime.UtcNow, account.Email, account.NickName, text, DateTime.UtcNow, null);
        _postList.Add(post);
        CurrentPost = post.ToDTO();
        await _postRepository.AddPost(post.ToDTO());
        OnDataChanged?.Invoke();
        //try
        //{
        //    Debug.Log("AddPost ����");
        //    Account account = AccountManager.Instance.MyAccount;
        //    Debug.Log($"Account: {account?.Email}");

        //    Post post = new Post(account.Email + DateTime.UtcNow, account.Email, account.NickName, text, DateTime.UtcNow, null);
        //    Debug.Log($"Post ���� �Ϸ�: {post.ID}");

        //    _postList.Add(post);
        //    CurrentPost = post.ToDTO();
        //    Debug.Log("CurrentPost ���� �Ϸ�");

        //    await _postRepository.AddPost(post.ToDTO());
        //    Debug.Log("Repository AddPost �Ϸ�");

        //    OnDataChanged?.Invoke();
        //    Debug.Log("OnDataChanged ȣ�� �Ϸ�");
        //}
        //catch (System.Exception e)
        //{
        //    Debug.LogError($"AddPost ����: {e.Message}");
        //    Debug.LogError($"�� ����: {e.StackTrace}");
        //    throw; // ������ �ٽ� ������ CreatePost���� ���� �� �ְ� ��
        //}
    }

    public async Task UpdatePost()
    {
        if (PostList != null || PostList.Count > 0)
        {
            await _postRepository.UpdatePost(PostList);
            _postList.Clear();
        }

        _postList = await _postRepository.GetPosts();
        OnDataChanged?.Invoke();
    }
    

    public void ShowCurruntPost(string id)
    {
        CurrentPost = FindById(id);
        UnityEngine.Debug.Log(CurrentPost.ID);
        OnShowDetail?.Invoke(CurrentPost.ID);
    }

    public async Task DeletePost(string id)
    {
        PostDTO postDTO = FindById(id);
        _postList.Remove(_postList.Find(a => a.ID == id));
        if (postDTO.Email != AccountManager.Instance.MyAccount.Email)
        {
            return;
        }
        await _postRepository.DeletePost(postDTO);
        OnDataChanged?.Invoke();
    }
    public PostDTO FindById(string id)
    {
        PostDTO postdto = PostList.Find(a => a.ID == id);

        if (postdto == null)
        {
            throw new Exception("postdto not found");
        }

        return postdto;
    }

}
