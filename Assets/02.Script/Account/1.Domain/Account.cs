using UnityEngine;

public class Account
{
    public readonly string Email;
    public readonly string NickName;

    public Account(string email, string nickName)
    {
        Email = email;
        NickName = nickName;
    }
}
