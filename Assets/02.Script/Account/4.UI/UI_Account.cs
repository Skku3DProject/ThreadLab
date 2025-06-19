using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading.Tasks;
using TMPro;

public class UI_Account : MonoBehaviour
{

    [Header("로그인")]
    [SerializeField] GameObject LoginPanel;
    [SerializeField] private Button _loginButton;
    [SerializeField] private Button _goToSignupPanelButton;
    [SerializeField] private TMP_InputField _loginEmaillField;
    [SerializeField] private TMP_InputField _loginPasswordField;



    [Header("회원가입")]
    [SerializeField] GameObject SingupPanel;
    [SerializeField] private Button _signupButton;
    [SerializeField] private Button _goToLoginPanelButton;
    [SerializeField] private TMP_InputField _singupEmailInput;
    [SerializeField] private TMP_InputField _signupPasswordInput;
    [SerializeField] private TMP_InputField _singupNameInput;






    [SerializeField] private TextMeshProUGUI _messageText;



    private async void Start()
    {
        SetInteractable(false);
        // FirebaseManager 초기화 기다림 InitTask-> 이작업이 파이어베이스 매니저에서 처음에 파이어베이스 초기화하는 작업 저장해둔거임
        await FirebaseManager.Instance.InitTask;
        SetInteractable(true);

        // 버튼 이벤트 연결
        _loginButton.onClick.AddListener(OnLoginClicked);
        _signupButton.onClick.AddListener(() => _ = OnSignUpClicked());

        _goToSignupPanelButton.onClick.AddListener(OnClickGoToSingupPanel);
        _goToLoginPanelButton.onClick.AddListener(OnClickGoToLoginPanel);

        OnClickGoToLoginPanel();
    }
    private void OnClickGoToSingupPanel()
    {
        LoginPanel.SetActive(false);
        SingupPanel.SetActive(true);
    }
    private void OnClickGoToLoginPanel()
    {
        LoginPanel.SetActive(true);
        SingupPanel.SetActive(false);
    }
    private void SetInteractable(bool state)
    {
        _loginButton.interactable = state;
        _goToSignupPanelButton.interactable = state;
    }

    private void OnLoginClicked()
    {
        Debug.Log("클릭 로그인");

        string email = _loginEmaillField.text.Trim();
        string password = _loginPasswordField.text.Trim();

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            ShowMessage("이메일과 비밀번호를 입력하세요.");
            return;
        }

        Debug.Log("클릭 로그인");

        AccountManager.Instance.TryLoginAndMoveSceneAsync(email, password);

    }

    private async Task OnSignUpClicked()
    {
        string email = _singupEmailInput.text.Trim();
        string password = _signupPasswordInput.text.Trim();
        string name = _singupNameInput.text.Trim();

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
            Debug.Log("회원가입 성공");

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
