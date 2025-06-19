using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TMP_InputField))]
public class AutoExpandInputField : MonoBehaviour
{
    public RectTransform inputFieldRect;
    public TMP_Text textComponent;
    public ScrollRect scrollRect;
    public float padding = 20f;

    private TMP_InputField inputField;
    private float lastHeight = -1f;

    void Awake()
    {
        inputField = GetComponent<TMP_InputField>();

        if (textComponent == null)
            textComponent = inputField.textComponent;

        if (inputFieldRect == null)
            inputFieldRect = inputField.GetComponent<RectTransform>();

        inputField.lineType = TMP_InputField.LineType.MultiLineNewline;
        inputField.verticalScrollbar = null;
    }

    void Update()
    {
        float preferredHeight = textComponent.preferredHeight + padding;

        if (Mathf.Abs(preferredHeight - lastHeight) > 1f)
        {
            Vector2 size = inputFieldRect.sizeDelta;
            size.y = preferredHeight;
            inputFieldRect.sizeDelta = size;
            lastHeight = preferredHeight;

            // 자동 스크롤
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0f;
        }
    }
}


