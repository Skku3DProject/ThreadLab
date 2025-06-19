using UnityEngine;

public class LikeDTO 
{
    public readonly string ID;
    public readonly string Email;
    public readonly string NickName;

    public LikeDTO (Like like)
    {
        ID = like.ID;
        Email = like.Email;
        NickName = like.NickName;
    }
}
