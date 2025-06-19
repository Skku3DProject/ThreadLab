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
        _loginButton.onClick.AddListener(OnLoginClicked);
        _registerButton.onClick.AddListener(() => _ = OnSignUpClicked());
    }

    private void SetInteractable(bool state)
    {
        _loginButton.interactable = state;
        _registerButton.interactable = state;
    }

    private void OnLoginClicked()
    {
        string email = _emailInput.text.Trim();
        string password = _passwordInput.text.Trim();

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            ShowMessage("이메일과 비밀번호를 입력하세요.");
            return;
        }


        AccountManager.Instance.TryLoginAndMoveSceneAsync(email, password);

    }

    private async Task OnSignUpClicked()
    {
        string email = _emailInput.text.Trim();
        string password = _passwordInput.text.Trim();
        string name = _nameInput.text.Trim();

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(name))
        {
            ShowMessage("모든 항목을 입력해라~");
            return;
        }

        // 이메일 명세 검사
        var emailSpec = new AccountEmailSpecification();
        if (!emailSpec.IsStatisfiedBy(email))
        {
            ShowMessage(emailSpec.ErrorMessage);
            return;
        }

        // 닉네임 명세 검사
        var nameSpec = new AccountNameSpecification();
        if (!nameSpec.IsStatisfiedBy(name))
        {
            ShowMessage(nameSpec.ErrorMessage);
            return;
        }

        //로그인 시도
        try
        {
            var account = await AccountManager.Instance.SignUpAsync(email, password, name);
            ShowMessage($"회원가입 성공 : {account}님 하이용~");
        }
        catch (Exception e)
        {
            ShowMessage($"회원가입: {e.Message}");
        }
    }

    private void ShowMessage(string message)
    {
        if (_messageText == null) return;

        _messageText.gameObject.SetActive(true);
        _messageText.text = message;
    }
}
