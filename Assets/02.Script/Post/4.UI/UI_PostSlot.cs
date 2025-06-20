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
    public int MaxHeight;
    public Button PostButton;
    public Button MenuButton;
    public Button DeleteButton;

    private string _id;

    public void Start()
    {
        PostButton.onClick.AddListener(() => OnClickPostSlot());
        DeleteButton.onClick.AddListener(() => _ = OnClickDelete());
    }

    public void OnClickPostSlot()
    {
        PostManager.Instance.ShowCurruntPost(_id);
        PostUiManager.Instance.ShowDetailPost();
    }

    public async Task OnClickDelete()
    {
        await PostManager.Instance.DeletePost(_id);
      //  PostUiManager.Instance.ShowMainPost();
    }

    public void Refresh(PostDTO postDto)
    {
        _id = postDto.ID;

        PostText.text = postDto.Text;
        PostDayText.text = postDto.WriteTime.ToString();
        PostNameText.text = postDto.NickName;
        LikeCount.text = postDto.Likes.Count.ToString();
    }


    public void SetText(string text)
    {
        PostText.text = text;

        // 텍스트 강제 업데이트 후 정확한 높이 계산
        PostText.ForceMeshUpdate();

        float preferredHeight = PostText.textBounds.size.y;

        // 최대 높이 제한 적용
        float finalHeight = Mathf.Min(preferredHeight, MaxHeight);

        // 높이 적용
        PostText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, finalHeight);
    }
}
