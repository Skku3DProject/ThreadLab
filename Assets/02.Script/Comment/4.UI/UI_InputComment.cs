using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UI_InputComment : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private Button _submitButton;

    private void Awake()
    {
        _submitButton.onClick.AddListener(OnSubmit);
    }

    private async void OnSubmit()
    {
        string content = _inputField.text.Trim();
        if (string.IsNullOrEmpty(content)) return;

        var account = AccountManager.Instance.MyAccount;

        string currnetPostID = PostManager.Instance.CurrentPost.ID;
        await CommentManager.Instance.PostComment(currnetPostID, account.NickName, account.Email, content);


        _inputField.text = ""; // 입력창 초기화
    }
}
