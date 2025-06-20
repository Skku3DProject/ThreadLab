using System;
using UnityEngine;

public class UI_LikeDetail : MonoBehaviour
{
    private string _id;
    public GameObject LikeIcon;
    
    private void Start()
    {
        _id = PostManager.Instance.CurrentPost.ID;
    }

    public void Refresh()
    {
        Refresh(PostManager.Instance.LikePost(_id));
        
    }
    private void IsLikeCheck(bool isLike)
    {
        LikeIcon.SetActive(isLike);
    }
    public void Refresh(bool isLike)
    {
        // bool isLike = PostManager.Instance.LikePost(_id);

        IsLikeCheck(isLike);
    }

}
