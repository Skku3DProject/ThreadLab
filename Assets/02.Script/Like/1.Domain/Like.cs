using Unity.VisualScripting;
using System;
using Firebase.Firestore;
using System.Collections.Generic;
[FirestoreData]
public class Like
{
  
    public readonly string ID;
    public readonly string Email;
    public readonly string NickName;

    public Like(string id, string email, string nickName)
    {
        if(string.IsNullOrEmpty(id))
        {
            throw new Exception("id는 비어있을 수 없습니다.");
        }

        var emailSpec = new AccountEmailSpecification();
        var nameSpec = new AccountNameSpecification();

        if (!emailSpec.IsStatisfiedBy(email))
            throw new ArgumentException(emailSpec.ErrorMessage);

        if (!nameSpec.IsStatisfiedBy(nickName))
            throw new ArgumentException(nameSpec.ErrorMessage);

        ID = id;
        Email = email;
        NickName = nickName;
    }
    public Like(Dictionary<string, object> mapData)
    {
        ID = mapData.ContainsKey("ID") ? mapData["ID"] as string : null;
        Email = mapData.ContainsKey("Email") ? mapData["Email"] as string : null;
        NickName = mapData.ContainsKey("NickName") ? mapData["NickName"] as string : null;
    }

    public LikeDTO ToDTO()
    {
        return new LikeDTO(this);
    }

    public Dictionary<string, object> ToDictionary()
    {
        return new Dictionary<string, object>
        {
            { "ID", ID },
            { "Email", Email },
            { "NickName", NickName },
        };
    }
}
