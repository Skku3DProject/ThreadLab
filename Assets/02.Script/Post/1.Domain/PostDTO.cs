using System.Collections.Generic;
using System;
using UnityEngine;

public class PostDTO 
{
    public readonly string Title;
    public readonly string ID;
    public readonly string Email;
    public readonly string NickName;
    public readonly string Text;
    public readonly DateTime WriteTime;
    public readonly List<Like> Likes;

    public PostDTO(Post post)
    {
        Title = post.Title;
        ID = post.ID;
        Email = post.Email;
        NickName = post.NickName;
        Text = post.Text;
        WriteTime = post.WriteTime;
        Likes = post.Likes;
    }

   

    public Dictionary<string, object> ToDictionary()
    {
        // Likes 리스트의 각 Like 객체를 Map으로 변환
        List<Dictionary<string, object>> likesAsDictionaries = new List<Dictionary<string, object>>();
        if (Likes != null)
        {
            foreach (var like in Likes)
            {
                likesAsDictionaries.Add(like.ToDictionary()); // Like 클래스에도 ToDictionary() 메서드가 필요
            }
        }

        return new Dictionary<string, object>
        {
            { "Title", Title },
            { "ID", ID },
            { "Email", Email },
            { "NickName", NickName },
            { "Text", Text },
            { "WriteTime", WriteTime.ToUniversalTime().ToString("o") }, // ISO 8601 형식으로 변환하여 저장 (UTC)
            { "Likes", likesAsDictionaries }
        };
    }
}
