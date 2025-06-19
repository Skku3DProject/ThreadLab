using TMPro; 
using UnityEngine;
using UnityEngine.UI; 


public class WritingContent : MonoBehaviour
{
    [Header("DialogWindowParts")]
    public RectTransform MiddlePart;
    public TextMeshProUGUI DialogText; 

    [Header("DialogWindowSettings")]
    public float TopPadding = 30f; 
    public float LeftRightMargin = 40f; 
    public float BottomMargin = 0f; 

    private LayoutElement _middleLayout;

    void Awake()
    {
        _middleLayout = MiddlePart.GetComponent<LayoutElement>();

    }

    void Update()
    {
        if (DialogText == null || _middleLayout == null) return;

        UpdateBubble();
    }

    public void UpdateBubble()
    {

        DialogText.margin = new Vector4(LeftRightMargin, TopPadding, LeftRightMargin, BottomMargin);

        LayoutRebuilder.ForceRebuildLayoutImmediate(DialogText.rectTransform);

        float targetHeight = DialogText.preferredHeight + DialogText.margin.y + DialogText.margin.w;

        _middleLayout.preferredHeight = targetHeight;
    }


    public void SetDialogText(string text)
    {
        DialogText.text = text;
        UpdateBubble();
    }
}
