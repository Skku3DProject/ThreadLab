using UnityEngine;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Collections.Generic;
using System.Xml.Linq;
using System;
public class CommentManager : MonoBehaviourSingleton<CommentManager>
{
    private CommentRepository _repository;
    private List<Comment> _comments;

    public event Action<string> OnDataChanged;

    protected override void Awake()
    {
        base.Awake();

        _comments = new List<Comment>();
        _repository = new CommentRepository();
    }
    private async void Start()
    {
        await InitAsync();
    }

    private async Task InitAsync()
    {
        _comments = await _repository.LoadAllComments();
    }
    public async Task PostComment(string postId, string username, string useremail, string content)
    {
        _comments.Add(await _repository.PostComment(postId, username, useremail, content));
        OnDataChanged.Invoke(PostManager.Instance.CurrentPost.ID);
    }
    public async Task DeleteComment(string commentID)
    {
        await _repository.DeleteComment(commentID);
        var deletedComment = _comments.Find(c => c.CommentUID == commentID);
        _comments.Remove(deletedComment);
    }
    public List<Comment> GetCommentsByPostId(string postId)
    {
        return _comments.FindAll(c => c.PostUID == postId);
    }
}
