using Firebase.Firestore;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Post
{
    public readonly string ID;
    public readonly string Email;
    public readonly string NickName;
    public string Text { get; private set; }
    public readonly DateTime WriteTime;
    public readonly List<Like> Likes;

    //public string LimitText { get; private set; }
    //public readonly int LimitTextCount;

    public Post( string id, string email, string nickName, string text, DateTime writeTime, List<Like> likes)
    {
        //if (string.IsNullOrEmpty(id))
        //{
        //    throw new Exception("id�� ������� �� �����ϴ�.");
        //}

        //var emailSpec = new AccountEmailSpecification();
        //var nameSpec = new AccountNameSpecification();

        //if (!emailSpec.IsStatisfiedBy(email))
        //{
        //    throw new ArgumentException(emailSpec.ErrorMessage);
        //}

        //if (!nameSpec.IsStatisfiedBy(nickName))
        //{
        //    throw new ArgumentException(nameSpec.ErrorMessage);
        //}

        //// text �� �ʿ�
        //if (string.IsNullOrEmpty(text))
        //{
        //    throw new Exception("text�� ������� �� �����ϴ�.");
        //}

        //if (writeTime == new DateTime())
        //{
        //    throw new Exception("writeTime�� ������� �� �����ϴ�.");
        //}

        //if (likes != null)
        //{
        //    Likes = new List<Like>(likes);
        //}

        ID = id;
        Email = email;
        NickName = nickName;
        Text = text;
        WriteTime = writeTime;
    }
    public Post(Dictionary<string, object> mapData)
    {
        ID = mapData.ContainsKey("ID") ? mapData["ID"] as string : null;
        Email = mapData.ContainsKey("Email") ? mapData["Email"] as string : null;
        NickName = mapData.ContainsKey("NickName") ? mapData["NickName"] as string : null;
        Text = mapData.ContainsKey("Text") ? mapData["Text"] as string : null;

        // WriteTime �Ľ�
        WriteTime = DateTime.MinValue; // �⺻�� ����
        if (mapData.ContainsKey("WriteTime"))
        {
            DateTime.TryParse(mapData["WriteTime"] as string, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.RoundtripKind, out WriteTime);
        }

        // Likes ����Ʈ �Ľ�
        Likes = new List<Like>();
        if (mapData.ContainsKey("Likes") && mapData["Likes"] is List<object> rawLikes)
        {
            foreach (var rawLike in rawLikes)
            {
                if (rawLike is Dictionary<string, object> likeMap)
                {
                    Likes.Add(new Like(likeMap)); // Like Ŭ�������� Map ������ �ʿ�
                }
            }
        }
    }

    public PostDTO ToDTO()
    {
        return new PostDTO(this);
    }

   
}
