using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

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
        PostDTO postDTO = new Post(account.Email + DateTime.UtcNow, account.Email, account.NickName, text, DateTime.UtcNow, null).ToDTO();
        //PostDTO postDTO = new Post("account.Email" + DateTime.UtcNow, "account.Email", "account.NickName", text, DateTime.UtcNow, null).ToDTO();
        CurrentPost = postDTO;
        await _postRepository.AddPost(postDTO);
        await UpdatePost();
    }

    public async Task UpdatePost()
    {
        if(PostList != null || PostList.Count > 0)
        {
            await _postRepository.UpdatePost(PostList);
            _postList.Clear();
        }
        
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

    public void ShowCurruntPost(string id)
    {
        CurrentPost = FindById(id);
        UnityEngine.Debug.Log(CurrentPost.ID);
        OnShowDetail?.Invoke(CurrentPost.ID);
    }

}
