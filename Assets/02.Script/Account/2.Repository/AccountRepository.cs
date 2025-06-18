using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class AccountRepository : FirebaseRepositoryBase
{
    public async Task<Account> Login(string email, string password)
    {
        await Auth.SignInWithEmailAndPasswordAsync(email, password);
        return await LoadAccount();
    }

    public async Task<Account> SignUp(string email, string password, string name)
    {
        await Auth.CreateUserWithEmailAndPasswordAsync(email, password);

        Account account;
        try
        {
            account = new Account(email, name); // 명세 검사 포함
        }
        catch
        {
            throw new Exception("회원가입 정보가 올바르지 않습니다.");
        }

        await SaveAccount(account);

        return account;
    }

    public async Task<Account> LoadAccount()
    {
        string uid = Auth.CurrentUser.UserId;

        var doc = await ExecuteAsync(() =>
            Firestore.Collection("accounts").Document(uid).GetSnapshotAsync(), "계정 로드");

        if (!doc.Exists)
            throw new Exception("계정 없음");

        return new Account(
            doc.GetValue<string>("email"),
            doc.GetValue<string>("name")
        );
    }

    public async Task SaveAccount(Account account)
    {
        string uid = Auth.CurrentUser.UserId;

        var data = new Dictionary<string, object>
        {
            { "email", account.Email },
            { "name", account.NickName }
        };

        await ExecuteAsync(() =>
            Firestore.Collection("accounts").Document(uid).SetAsync(data), "계정 저장");
    }

    public async Task<List<Account>> LoadAllAccounts()
    {
        var snapshot = await ExecuteAsync(() => Firestore.Collection("accounts").GetSnapshotAsync(), "모든 계정 불러오기");

        var accounts = new List<Account>();

        foreach (var doc in snapshot.Documents)
        {
            // 각 문서에서 필드 읽기 (예외 방지를 위해 TryGetValue도 고려 가능)
            string email = doc.ContainsField("email") ? doc.GetValue<string>("email") : "";
            string name = doc.ContainsField("name") ? doc.GetValue<string>("name") : "";

            accounts.Add(new Account(email, name));
        }

        return accounts;
    }
}
