using System;
using System.Collections.Generic;
using UnityEngine;
public class UI_Like : MonoBehaviour
{
    private List<UI_LikeSlot> _likeSlots = new List<UI_LikeSlot>();
    private List<PostDTO> _postDtos = new List<PostDTO>();

    private void Start()
    {
        _postDtos = PostManager.Instance.PostList;
    }

    private void LoadLikes()
    {
        string email = AccountManager.Instance.MyAccount.Email;
        foreach (var post in _postDtos)
        {
            
            for (int i = 0; i < _likeSlots.Count; i++)
            {
                _likeSlots[i].Refresh(
                    PostManager.Instance.LikePost(post.ID));
            }
        }
    }
}

