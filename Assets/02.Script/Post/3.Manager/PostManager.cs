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
        // 이때 CurrentPost도 변경해줘야함 현재 보는 디테일 패널의 post로
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
            Debug.Log("AddPost 시작");
            Account account = AccountManager.Instance.MyAccount;
            Debug.Log($"Account: {account?.Email}");

            Post post = new Post(account.Email + DateTime.UtcNow, account.Email, account.NickName, text, DateTime.UtcNow, null);
            Debug.Log($"Post 생성 완료: {post.ID}");

            _postList.Add(post);
            CurrentPost = post.ToDTO();
            Debug.Log("CurrentPost 설정 완료");

            await _postRepository.AddPost(post.ToDTO());
            Debug.Log("Repository AddPost 완료");

            OnDataChanged?.Invoke();
            Debug.Log("OnDataChanged 호출 완료");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"AddPost 에러: {e.Message}");
            Debug.LogError($"상세 에러: {e.StackTrace}");
            throw; // 에러를 다시 던져서 CreatePost에서 잡을 수 있게 함
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
