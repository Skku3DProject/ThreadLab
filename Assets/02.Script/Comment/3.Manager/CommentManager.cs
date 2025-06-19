using UnityEngine;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Collections.Generic;
using System.Xml.Linq;
public class CommentManager : MonoBehaviourSingleton<CommentManager>
{
    private CommentRepository _repository;
    private List<Comment> _comments;


    protected override void Awake()
    {
        base.Awake();

        _comments = new List<Comment>();
        _repository = new CommentRepository();
    }
    private async void Start()
    {
        await InitAsync();

        Debug.Log(AccountManager.Instance.MyAccount.NickName);
        await PostComment("post", AccountManager.Instance.MyAccount.NickName, AccountManager.Instance.MyAccount.Email, "ÇÜ¹ö°Å´Â ¿Õ¸ÀÀÖ¾î");
    }

    private async Task InitAsync()
    {
        _comments = await _repository.LoadAllComments();
    }
    public async Task PostComment(string postId, string username, string useremail, string content)
    {
        _comments.Add(await _repository.PostComment(postId, username, useremail, content));
    }
    public async Task DeleteComment(string commentID)
    {
        await _repository.DeleteComment(commentID);
        var deletedComment = _comments.Find(c => c.CommentUID == commentID);
        _comments.Remove(deletedComment);
    }

}
