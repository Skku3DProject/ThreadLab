using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class AccountManager : MonoBehaviourSingleton<AccountManager>
{
    private AccountRepository _repo;

    private List<Account> _accounts;
    public List<Account> Accounts => _accounts;

    private Account _myAccount;
    public Account MyAccount => _myAccount;

    [SerializeField] private string _nextSceneName;

    protected override void Awake()
    {
        base.Awake();

        _repo = new AccountRepository();
        _accounts = new List<Account>();
    }
    private async void Start()
    {
        await InitAsync();
    }
    private async Task InitAsync()
    {
        _repo = new AccountRepository();
        _accounts = await _repo.LoadAllAccounts();
    }

    public Task<Account> LoginAsync(string email, string password)
    {
        return _repo.Login(email, password);
    }

    public Task<Account> SignUpAsync(string email, string password, string name)
    {
        return _repo.SignUp(email, password, name);
    }

    public async void TryLoginAndMoveSceneAsync(string email, string password)
    {
        try
        {
            Account account = await LoginAsync(email, password);

            if (account != null)
            {
                _myAccount = account;

                Debug.Log($"나의 계정 : {_myAccount.NickName}");
                // 씬 전환
                SceneManager.LoadScene(_nextSceneName);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("로그인 씬이동 에러: " + ex.Message);
        }
    }
}
