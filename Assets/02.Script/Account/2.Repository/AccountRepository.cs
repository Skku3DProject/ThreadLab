using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class AccountRepository : FirebaseRepositoryBase
{
    public async Task<Account> SignIn(string email, string password)
    {
        var credential = await Auth.SignInWithEmailAndPasswordAsync(email, password);
        return await LoadAccount();
    }

    public async Task<Account> SignUp(string email, string password, string name)
    {
        var credential = await Auth.CreateUserWithEmailAndPasswordAsync(email, password);

        var account = new Account(email, name);
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
}
