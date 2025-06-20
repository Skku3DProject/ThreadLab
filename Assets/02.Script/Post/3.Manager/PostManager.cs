using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;

public class PostManager : MonoBehaviourSingleton<PostManager>
{
    private List<Post> _postList;
    public List<PostDTO> PostList => _postList.ConvertAll(p => p.ToDTO());

    private PostRepository _postRepository;

    public PostDTO CurrentPost;

    public event Action OnDataChanged;
    public event Action<string> OnShowDetail;

    public void InvokeEvent()
    {
        // �̶� CurrentPost�� ����������� ���� ���� ������ �г��� post��
        OnShowDetail.Invoke(CurrentPost.ID);
    }

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
        _postList = await _postRepository.GetPosts();

    }

    public async Task AddPost(string text)
    {
<<<<<<< Updated upstream
     //   Account account = AccountManager.Instance.MyAccount;
        //PostDTO postDTO = new Post(account.Email + DateTime.UtcNow, account.Email, account.NickName, text, DateTime.UtcNow, null).ToDTO();
        PostDTO postDTO = new Post("account.Email" + DateTime.UtcNow, "account.Email", "account.NickName", text, DateTime.UtcNow, null).ToDTO();
        CurrentPost = postDTO;
         await _postRepository.AddPost(postDTO);
=======
        try
        {
            Debug.Log("AddPost ����");
            Account account = AccountManager.Instance.MyAccount;
            Debug.Log($"Account: {account?.Email}");

            Post post = new Post(account.Email + DateTime.UtcNow, account.Email, account.NickName, text, DateTime.UtcNow, null);
            Debug.Log($"Post ���� �Ϸ�: {post.ID}");

            _postList.Add(post);
            CurrentPost = post.ToDTO();
            Debug.Log("CurrentPost ���� �Ϸ�");

            await _postRepository.AddPost(post.ToDTO());
            Debug.Log("Repository AddPost �Ϸ�");

            OnDataChanged?.Invoke();
            Debug.Log("OnDataChanged ȣ�� �Ϸ�");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"AddPost ����: {e.Message}");
            Debug.LogError($"�� ����: {e.StackTrace}");
            throw; // ������ �ٽ� ������ CreatePost���� ���� �� �ְ� ��
        }
>>>>>>> Stashed changes
    }

    public async Task UpdatePost()
    {
        await _postRepository.UpdatePost(PostList);
        _postList.Clear();
        _postList = await _postRepository.GetPosts();
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
