using System.Threading.Tasks;
using UnityEngine;

public class AccountManager : MonoBehaviourSingleton<AccountManager>
{
    private AccountRepository _repo;

    protected override void Awake()
    {
        _repo = new AccountRepository();
    }

    public Task<Account> LoginAsync(string email, string password)
    {
        return _repo.SignIn(email, password);
    }

    public Task<Account> RegisterAsync(string email, string password, string name)
    {
        return _repo.SignUp(email, password, name);
    }
}
