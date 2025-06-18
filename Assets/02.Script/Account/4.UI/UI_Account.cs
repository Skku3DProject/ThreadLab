using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading.Tasks;
using TMPro;

public class UI_Account : MonoBehaviour
{
    [SerializeField] private Button _loginButton;
    [SerializeField] private Button _registerButton;
    [SerializeField] private TextMeshProUGUI _messageText;

    [SerializeField] private TMP_InputField _emailInput;
    [SerializeField] private TMP_InputField _passwordInput;
    [SerializeField] private TMP_InputField _nameInput;

    private async void Start()
    {
        SetInteractable(false);
        // FirebaseManager 초기화 기다림 InitTask-> 이작업이 파이어베이스 매니저에서 처음에 파이어베이스 초기화하는 작업 저장해둔거임
        await FirebaseManager.Instance.InitTask;
        SetInteractable(true);

        // 버튼 이벤트 연결
        _loginButton.onClick.AddListener(() => _ = OnLoginClicked());
        _registerButton.onClick.AddListener(() => _ = OnRegisterClicked());
    }

    private void SetInteractable(bool state)
    {
        _loginButton.interactable = state;
        _registerButton.interactable = state;
    }

    private async Task OnLoginClicked()
    {
        string email = _emailInput.text.Trim();
        string password = _passwordInput.text.Trim();

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            ShowMessage("이메일과 비밀번호를 입력하세요.");
            return;
        }

        try
        {
            var account = await AccountManager.Instance.LoginAsync(email, password);
            ShowMessage($"로그인 성공, {account.NickName}");
        }
        catch (Exception e)
        {
            ShowMessage($"로그인 실패: {e.Message}");
        }
    }

    private async Task OnRegisterClicked()
    {
        string email = _emailInput.text.Trim();
        string password = _passwordInput.text.Trim();
        string name = _nameInput.text.Trim();

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(name))
        {
            ShowMessage("모든 필드를 입력하세요.");
            return;
        }

        try
        {
            var account = await AccountManager.Instance.RegisterAsync(email, password, name);
            ShowMessage($"회원가입 성공! 닉네임: {account.NickName}");
        }
        catch (Exception e)
        {
            ShowMessage($"회원가입 실패: {e.Message}");
        }
    }

    private void ShowMessage(string message)
    {
        if (_messageText != null)
            _messageText.text = message;
        Debug.Log(message);
    }
}
