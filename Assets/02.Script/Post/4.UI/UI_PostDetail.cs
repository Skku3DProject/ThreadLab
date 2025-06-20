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

    public void ShowMainPost()
    {
        PostUIManager.Instance.ShowMainPost();
    }

    void OnClickMenu()
    {
        PostUIManager.Instance.MoveMenu(MenuButton.GetComponent<RectTransform>());
    }
}
