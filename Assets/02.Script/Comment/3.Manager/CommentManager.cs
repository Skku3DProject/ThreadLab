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

    // 검증 상수
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
            // 새 댓글 생성 (도메인에서 Specification으로 검증)
            string commentUID = Guid.NewGuid().ToString();
            DateTime now = DateTime.UtcNow;
            var newComment = new Comment(commentUID, postId, username, useremail, content, now);

            // Repository에 저장 (DTO로 변환해서)
            var savedDTO = await _repository.PostComment(newComment.ToDTO());

            _comments.Add(newComment);

            //OnCommentAdded?.Invoke(newComment);
            OnDataChanged?.Invoke(postId);
            
            Debug.Log($"댓글 생성 성공: {newComment.CommentUID}");
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"댓글 저장 실패: {ex.Message}");
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

            // 권한 확인
            if (!comment.IsOwner(userEmail))
            {
                return false;
            }

            await _repository.DeleteComment(commentID);
            _comments.Remove(comment);

            //OnCommentDeleted?.Invoke(commentID);
            OnDataChanged?.Invoke(comment.PostUID);

            Debug.Log($"댓글 삭제 성공: {commentID}");
            return true;
        //}
        //catch
        //{
        //    Debug.LogError($"댓글 삭제 실패");
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
