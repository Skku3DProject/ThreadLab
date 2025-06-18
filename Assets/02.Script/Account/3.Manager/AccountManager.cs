using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class AccountManager : MonoBehaviourSingleton<AccountManager>
{
    private AccountRepository _repo;

    private List<Account> _accounts;
    public List<Account> Accounts => _accounts;

    protected override void Awake()
    {
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
}
