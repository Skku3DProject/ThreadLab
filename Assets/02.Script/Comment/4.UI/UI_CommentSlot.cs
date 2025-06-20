using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CommentSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _userName;
    [SerializeField] private TextMeshProUGUI _mainText;
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private Button _deleteButton;

    private string _commentId;
    private string _postId;
    private UI_Comments _parentUI;

    public void Refresh(CommentDTO comment, UI_Comments parentUI)
    {
        _commentId = comment.CommentUID;
        _postId = comment.PostUID;
        _parentUI = parentUI;

        _userName.text = comment.UserName;
        _mainText.text = comment.MainText;
        _timeText.text = comment.Timestamp.ToDateTime().ToString("yyyy-MM-dd HH:mm");

        _deleteButton.onClick.RemoveAllListeners();
        _deleteButton.onClick.AddListener(OnClickDelete);
    }

    private async void OnClickDelete()
    {
        await CommentManager.Instance.DeleteComment(_commentId,AccountManager.Instance.MyAccount.Email);
        _parentUI.RefreshComments(_postId); //���� ������ UI ����
    }
}
