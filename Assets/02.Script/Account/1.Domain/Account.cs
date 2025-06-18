using System;
using UnityEngine;

public class Account
{
    public readonly string Email;
    public readonly string NickName;

    public Account(string email, string nickName)
    {
        var emailSpec = new AccountEmailSpecification();
        var nameSpec = new AccountNameSpecification();

        if (!emailSpec.IsStatisfiedBy(email))
            throw new ArgumentException(emailSpec.ErrorMessage);

        if (!nameSpec.IsStatisfiedBy(nickName))
            throw new ArgumentException(nameSpec.ErrorMessage);

        Email = email;
        NickName = nickName;
    }
}
