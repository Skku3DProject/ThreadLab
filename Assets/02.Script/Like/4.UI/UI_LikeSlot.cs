using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_LikeSlot : MonoBehaviour
{
    private string _id;
    public GameObject LikeIcon;
    public TextMeshProUGUI LikeCount;
    private int _likeCount;
    
    private void Start()
    {
        _id = GetComponentInParent<UI_PostSlot>().ID;
    }
    
    
    public void Refresh()
    {
        Refresh(PostManager.Instance.LikePost(_id));
        
    }

    public void Refresh(bool isLike)
    {
        // bool isLike = PostManager.Instance.LikePost(_id);

        IsLikeCheck(isLike);
        LikeCount.text = $"{PostManager.Instance.FindById(_id).Likes.Count}";
    }

    private void IsLikeCheck(bool isLike)
    {
        LikeIcon.SetActive(isLike);
    }
}
