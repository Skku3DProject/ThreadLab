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
            account = new Account(email, name); // �� �˻� ����
        }
        catch
        {
            throw new Exception("ȸ������ ������ �ùٸ��� �ʽ��ϴ�.");
        }

        await SaveAccount(account);

        return account;
    }

    public async Task<Account> LoadAccount()
    {
        string uid = Auth.CurrentUser.UserId;

        var doc = await ExecuteAsync(() =>
            Firestore.Collection("accounts").Document(uid).GetSnapshotAsync(), "���� �ε�");

        if (!doc.Exists)
            throw new Exception("���� ����");

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
            Firestore.Collection("accounts").Document(uid).SetAsync(data), "���� ����");
    }

    public async Task<List<Account>> LoadAllAccounts()
    {
        var snapshot = await ExecuteAsync(() => Firestore.Collection("accounts").GetSnapshotAsync(), "��� ���� �ҷ�����");

        var accounts = new List<Account>();

        foreach (var doc in snapshot.Documents)
        {
            // �� �������� �ʵ� �б� (���� ������ ���� TryGetValue�� ��� ����)
            string email = doc.ContainsField("email") ? doc.GetValue<string>("email") : "";
            string name = doc.ContainsField("name") ? doc.GetValue<string>("name") : "";

            accounts.Add(new Account(email, name));
        }

        return accounts;
    }
}
