using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PostDetail : MonoBehaviour
{
    public TextMeshProUGUI PostText;
    public TextMeshProUGUI PostNameText;
    public TextMeshProUGUI PostDayText;
    public TextMeshProUGUI LikeCount;

    public Button MenuButton;

    

    private void Awake()
    {
        MenuButton.onClick.AddListener(OnClickMenu);
    }

    private void Start()
    {
        PostUIManager.Instance.OnShowPostDetail += ShowDetail;
    }

    public void ShowMainPost()
    {
        PostUIManager.Instance.ShowMainPost();
    }

    public void ShowDetail()
    {
        PostDTO postDto = PostManager.Instance.CurrentPost;
        PostText.text = postDto.Text;
        PostDayText.text = postDto.WriteTime.ToString();
        PostNameText.text = postDto.NickName;
        LikeCount.text = postDto.Likes?.Count.ToString() ?? "0";
    }

    void OnClickMenu()
    {
        RectTransform rectTransform = MenuButton.GetComponent<RectTransform>();
        
        PostUIManager.Instance.MenuOnOff(rectTransform);
    }
}
