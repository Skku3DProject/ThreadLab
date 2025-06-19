using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PostSlot : MonoBehaviour
{
    public TextMeshProUGUI PostText;
    public int MaxHeight;
    public Button button;
    public UI_Post UI_Post;
    public void Start()
    {
        button.onClick.AddListener(() =>UI_Post.ShowDetailPost());
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
}
