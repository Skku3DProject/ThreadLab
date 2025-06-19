using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PostSlot : MonoBehaviour
{
    public TextMeshProUGUI PostText;
    public int MaxHeight;

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
