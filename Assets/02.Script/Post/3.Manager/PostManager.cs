using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class PostManager : MonoBehaviourSingleton<PostManager>
{
    private List<Post> _postList;
    public List<PostDTO> PostList => _postList.ConvertAll(p => p.ToDTO());

    private PostRepository _postRepository;

    public PostDTO CurrentPost;
    
    public event Action OnDataChanged;
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

    public async Task AddPost(PostDTO postDTO)
    {
        await _postRepository.AddPost(postDTO);
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

        if(postdto == null)
        {
            throw new Exception("postdto not found");
        }

        return postdto;
    }
    
}
