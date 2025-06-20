using UnityEngine;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Collections.Generic;
using System.Xml.Linq;
using System;
using System.Linq;
public class CommentManager : MonoBehaviourSingleton<CommentManager>
{
    private CommentRepository _repository;
    private List<Comment> _comments;
    private List<CommentDTO> _commentsDTO => _comments.ConvertAll(c => c.ToDTO());

    // ���� ���
    private const int MAX_CONTENT_LENGTH = 500;
    private const int MIN_CONTENT_LENGTH = 1;

    public event Action<string> OnDataChanged;
    //public event Action<Comment> OnCommentAdded;
   //public event Action<Comment> OnCommentUpdated;
    //public event Action<string> OnCommentDeleted;

    protected override void Awake()
    {
        base.Awake();
        _comments = new List<Comment>();
        _repository = new CommentRepository();

    }
    
    public async void Start()
    { 
       _comments = await LoadAllComments();
       Debug.Log(_commentsDTO.Count);
    }

    public async Task<List<Comment>> LoadAllComments()
    {
       return await _repository.LoadAllComments();
    }
    
    public async Task<bool> PostComment(string postId, string username, string useremail, string content)
    {
        try
        {
            // �� ��� ���� (�����ο��� Specification���� ����)
            string commentUID = Guid.NewGuid().ToString();
            DateTime now = DateTime.UtcNow;
            var newComment = new Comment(commentUID, postId, username, useremail, content, now);

            // Repository�� ���� (DTO�� ��ȯ�ؼ�)
            var savedDTO = await _repository.PostComment(newComment.ToDTO());

            _comments.Add(newComment);

            //OnCommentAdded?.Invoke(newComment);
            OnDataChanged?.Invoke(postId);
            
            Debug.Log($"��� ���� ����: {newComment.CommentUID}");
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"��� ���� ����: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteComment(string commentID, string userEmail)
    {
        //try
        //{
            var comment = _comments.FirstOrDefault(c => c.CommentUID == commentID);
            if (comment == null)
            {
                return false;
            }

            // ���� Ȯ��
            if (!comment.IsOwner(userEmail))
            {
                return false;
            }

            await _repository.DeleteComment(commentID);
            _comments.Remove(comment);

            //OnCommentDeleted?.Invoke(commentID);
            OnDataChanged?.Invoke(comment.PostUID);

            Debug.Log($"��� ���� ����: {commentID}");
            return true;
        //}
        //catch
        //{
        //    Debug.LogError($"��� ���� ����");
        //    return false;
        //}
    }

    public List<CommentDTO> GetCommentsByPostId(string postId)
    {
        return _commentsDTO.Where(c => c.PostUID == postId)
                       .OrderBy(c => c.Timestamp)
                       .ToList();
    }

    public Comment GetCommentById(string commentId)
    {
        return _comments.FirstOrDefault(c => c.CommentUID == commentId);
    }

}
