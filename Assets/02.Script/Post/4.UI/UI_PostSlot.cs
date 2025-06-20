using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PostSlot : MonoBehaviour
{
    public TextMeshProUGUI PostText;
    public TextMeshProUGUI PostNameText;
    public TextMeshProUGUI PostDayText;
    public TextMeshProUGUI LikeCount;

 
    public Button MenuButton;
    public int MaxHeight;
    private string _id;
    public string ID => _id;
    public void Start()
    {
        MenuButton.onClick.AddListener(OnClickMenu);
    }

    public void OnClickPostSlot()
    {
        PostManager.Instance.ShowCurruntPost(_id);
        PostUIManager.Instance.ShowDetailPost();
    }

    public void Refresh(PostDTO postDto)
    {
        _id = postDto.ID;

        SetText(postDto.Text);
        PostDayText.text = postDto.WriteTime.ToString();
        PostNameText.text = postDto.NickName;
        LikeCount.text = postDto.Likes?.Count.ToString() ?? "0";
    }


    public void SetText(string text)
    {
        PostText.text = text;

        // �ؽ�Ʈ ���� ������Ʈ �� ��Ȯ�� ���� ���
        PostText.ForceMeshUpdate();

        float preferredHeight = PostText.textBounds.size.y;

        // �ִ� ���� ���� ����
        float finalHeight = Mathf.Min(preferredHeight, MaxHeight);

        // ���� ����
        PostText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, finalHeight);
    }
    void OnClickMenu()
    {
        PostManager.Instance.ShowCurruntPost(_id);
        PostUIManager.Instance.MoveMenu(MenuButton.GetComponent<RectTransform>());
    }
}
