using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class PostManager : MonoBehaviourSingleton<PostManager>
{
    private List<Post> _postList = new List<Post>();
    public List<PostDTO> PostList => _postList.ConvertAll(p => p.ToDTO());

    private PostRepository _postRepository;

    public PostDTO CurrentPost;

    public event Action OnDataChanged;
    public event Action<string> OnShowDetail;
    public event Action<bool> OnLikeChanged;
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
    }
    public async Task UpdatePost()
    {
        if (PostList != null || PostList.Count > 0)
        {
            _postList.Clear();
        }

        _postList = await _postRepository.GetPosts();
        OnDataChanged?.Invoke();
    }

    public void ShowCurruntPost(string id)
    {
        CurrentPost = FindById(id);
      
    }

    public async Task UpdateWrite(string text )
    {
        PostDTO postDTO = CurrentPost;
        Post post = new Post(postDTO.ID, postDTO.Email, postDTO.NickName, text, postDTO.WriteTime, postDTO.Likes);
        await _postRepository.UpdatePost(post.ToDTO());
        await UpdatePost();
    }

    public async Task DeletePost()
    {
        _postList.Remove(_postList.Find(a => a.ID == CurrentPost.ID));
        if (CurrentPost.Email != AccountManager.Instance.MyAccount.Email)
        {
            return;
        }
        await _postRepository.DeletePost(CurrentPost);
        await UpdatePost();
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

    public bool LikePost(string id)
    {
        Post likePost = _postList.Find(a => a.ID == id);
        
        Debug.Log($"{likePost.ID} liked {likePost.Likes.Count}");
        
        // OnLikeChanged?.Invoke(likePost.LikeStateChange(AccountManager.Instance.MyAccount.Email));
        return likePost.LikeStateChange(AccountManager.Instance.MyAccount.Email);
    }
    
    
}
