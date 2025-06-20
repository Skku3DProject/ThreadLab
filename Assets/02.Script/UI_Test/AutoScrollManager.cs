using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro ���� Ŭ������ ����ϱ� ���� �ʿ�

public class AutoScrollManager : MonoBehaviour
{
    // ����Ƽ �����Ϳ��� �Ҵ��� UI ��ҵ�
    public TMP_InputField inputField;
    public ScrollRect scrollView;
    public RectTransform contentRectTransform;
    public RectTransform viewportRectTransform;

    [Header("InputField Height Settings")]
    public float minInputFieldHeight = 50f; // InputField�� �ּ� ���� (�ؽ�Ʈ�� ���� ��)
    public float inputFieldPadding = 10f;   // InputField �ؽ�Ʈ �ֺ��� ���� (���Ʒ�)

    [Header("Scroll Activation Settings")]
    public float scrollActivationThreshold = 300f; // Content�� ���̰� �� ���� �ʰ��ϸ� ��ũ�� Ȱ��ȭ

    [Header("Bottom Padding Settings")]
    public float bottomPadding = 100f; // Content �ϴܿ� �߰��� ����

    private float _initialContentHeightExcludingInputField;

    void Start()
    {
        if (inputField == null || scrollView == null || contentRectTransform == null || viewportRectTransform == null)
        {
            Debug.LogError("�ʼ� UI ��Ұ� �Ҵ���� �ʾҽ��ϴ�. ��ũ��Ʈ �ʵ带 ä���ּ���.");
            enabled = false;
            return;
        }

        // InputField ������ ����
        inputField.onValueChanged.AddListener(OnInputFieldValueChanged);

        // InputField ���̾ƿ� ���� ���� �� �ʱ� ������ ���� ���
        LayoutRebuilder.ForceRebuildLayoutImmediate(inputField.GetComponent<RectTransform>());
        _initialContentHeightExcludingInputField = contentRectTransform.sizeDelta.y - inputField.GetComponent<RectTransform>().sizeDelta.y;

        // �ϴ� ���� �߰�
        AddBottomPadding(bottomPadding);

        // �ʱ� �ؽ�Ʈ ���� ��� ����
        OnInputFieldValueChanged(inputField.text);
    }

    // �ؽ�Ʈ�� �ٲ� �� ȣ���
    void OnInputFieldValueChanged(string newText)
    {
        float preferredTextHeight = inputField.textComponent.preferredHeight;
        float newInputFieldHeight = Mathf.Max(minInputFieldHeight, preferredTextHeight + inputFieldPadding);

        RectTransform inputFieldRect = inputField.GetComponent<RectTransform>();
        inputFieldRect.sizeDelta = new Vector2(inputFieldRect.sizeDelta.x, newInputFieldHeight);

        LayoutRebuilder.ForceRebuildLayoutImmediate(contentRectTransform);

        UpdateScrollState();
    }

    // ��ũ�� ���ο� ��ġ ����
    private void UpdateScrollState()
    {
        float currentContentHeight = contentRectTransform.sizeDelta.y;

        if (currentContentHeight > scrollActivationThreshold)
        {
            scrollView.vertical = true;
            Canvas.ForceUpdateCanvases(); // ��� ��ũ�� �ݿ�
            scrollView.verticalNormalizedPosition = 0f; // �� �Ʒ���
        }
        else
        {
            scrollView.vertical = false;
            scrollView.verticalNormalizedPosition = 1f; // �� ����
        }
    }

    // Content�� �� �Ʒ� ������ �߰��ϴ� �Լ�
    private void AddBottomPadding(float paddingHeight)
    {
        GameObject spacer = new GameObject("BottomPadding", typeof(RectTransform));
        spacer.transform.SetParent(contentRectTransform, false);

        RectTransform spacerRect = spacer.GetComponent<RectTransform>();
        spacerRect.sizeDelta = new Vector2(0, paddingHeight);
        spacerRect.anchorMin = new Vector2(0, 0);
        spacerRect.anchorMax = new Vector2(1, 0);
        spacerRect.pivot = new Vector2(0.5f, 0);

        LayoutElement layoutElement = spacer.AddComponent<LayoutElement>();
        layoutElement.minHeight = paddingHeight;
    }

    void OnDestroy()
    {
        if (inputField != null)
        {
            inputField.onValueChanged.RemoveListener(OnInputFieldValueChanged);
        }
    }
}

/*using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro ���� Ŭ������ ����ϱ� ���� �ʿ�

public class AutoScrollManager : MonoBehaviour
{
    // ����Ƽ �����Ϳ��� �Ҵ��� UI ��ҵ�
    public TMP_InputField inputField;
    public ScrollRect scrollView;
    public RectTransform contentRectTransform;
    public RectTransform viewportRectTransform;

    [Header("InputField Height Settings")]
    public float minInputFieldHeight = 50f; // InputField�� �ּ� ���� (�ؽ�Ʈ�� ���� ��)
    public float inputFieldPadding = 10f; // InputField �ؽ�Ʈ �ֺ��� ���� (���Ʒ�)

    [Header("Scroll Activation Settings")]
    public float scrollActivationThreshold = 300f; // Content�� ���̰� �� ���� �ʰ��ϸ� ��ũ�� Ȱ��ȭ

    private float _initialContentHeightExcludingInputField; // InputField�� ������ Content�� �ʱ� ����

    void Start()
    {
        // �ʼ� UI ��ҵ��� �Ҵ�Ǿ����� Ȯ��
        if (inputField == null || scrollView == null || contentRectTransform == null || viewportRectTransform == null)
        {
            Debug.LogError("�ʼ� UI ��Ұ� �Ҵ���� �ʾҽ��ϴ�. ��ũ��Ʈ �ʵ带 ä���ּ���.");
            enabled = false; // ��ũ��Ʈ ��Ȱ��ȭ
            return;
        }

        // InputField�� OnValueChanged �̺�Ʈ�� �Լ� ����
        // �ؽ�Ʈ�� ����� ������ OnInputFieldValueChanged �Լ��� ȣ��˴ϴ�.
        inputField.onValueChanged.AddListener(OnInputFieldValueChanged);

        // InputField�� ������ Content�� �ʱ� ���� ���
        // (��: UI Image �� �ٸ� �ڽ� ��ҵ��� ����)
        // Content�� �ʱ� ���̿��� InputField�� �ʱ� ���̸� ���� ����մϴ�.
        // �ʱ� InputField�� ���̵� LayoutRebuilder�� ���� ������Ʈ �� �������� ���� �����մϴ�.
        LayoutRebuilder.ForceRebuildLayoutImmediate(inputField.GetComponent<RectTransform>());
        _initialContentHeightExcludingInputField = contentRectTransform.sizeDelta.y - inputField.GetComponent<RectTransform>().sizeDelta.y;

        // �ʱ� ��ũ�� ���� ������Ʈ
        // (Start ������ �̹� �ؽ�Ʈ�� ���� ��츦 ���)
        OnInputFieldValueChanged(inputField.text);
    }

    // InputField�� �ؽ�Ʈ�� ����� �� ȣ��Ǵ� �Լ�
    void OnInputFieldValueChanged(string newText)
    {
        // 1. TMP_InputField�� ���� ����
        // TextMeshProUGUI ������Ʈ�� �ؽ�Ʈ�� ǥ���ϴ� �� �ʿ��� ���� ���̸� ����մϴ�.
        // �̶�, InputField�� ���� �ʺ� �������� �ٹٲ��� ����� ���̰� ���˴ϴ�.
        float preferredTextHeight = inputField.textComponent.preferredHeight;

        // InputField�� ���� ���̸� ����մϴ�. (�ּ� ���� �Ǵ� �ؽ�Ʈ ���� + �е� �� ū ��)
        float newInputFieldHeight = Mathf.Max(minInputFieldHeight, preferredTextHeight + inputFieldPadding);

        // InputField�� RectTransform ���� ������Ʈ
        RectTransform inputFieldRect = inputField.GetComponent<RectTransform>();
        inputFieldRect.sizeDelta = new Vector2(inputFieldRect.sizeDelta.x, newInputFieldHeight);

        // 2. Content�� ���� ���� ����
        // InputField�� ���̰� ����Ǿ����Ƿ�, Content�� Vertical Layout Group��
        // Content Size Fitter�� Content�� �� ���̸� �ٽ� ����ϵ��� �����մϴ�.
        // �� ������ ������ ���̾ƿ� ������Ʈ�� �����Ǿ� ��ũ���� ����� �������� ���� �� �ֽ��ϴ�.
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentRectTransform);

        // 3. ��ũ�� ���� ������Ʈ
        UpdateScrollState();
    }

    // ��ũ�� Ȱ��ȭ/��Ȱ��ȭ ���¸� ������Ʈ�ϴ� �Լ�
    private void UpdateScrollState()
    {
        // Content�� ���� �� ����
        float currentContentHeight = contentRectTransform.sizeDelta.y;

        // Content�� ���̰� ������ �Ӱ谪(scrollActivationThreshold)�� �ʰ��ϸ� ��ũ�� Ȱ��ȭ
        if (currentContentHeight > scrollActivationThreshold)
        {
            scrollView.vertical = true; // ���� ��ũ�� Ȱ��ȭ
            // ��ũ���� Ȱ��ȭ�Ǹ� �ڵ����� ���� �Ʒ��� ��ũ���մϴ�.
            // verticalNormalizedPosition�� 0�� ���ϴ�, 1�� �ֻ���Դϴ�.
            scrollView.verticalNormalizedPosition = 0f;
        }
        else
        {
            scrollView.vertical = false; // ���� ��ũ�� ��Ȱ��ȭ
            // ��ũ���� ��Ȱ��ȭ�Ǹ� Content�� �׻� ��ܿ� ��ġ��ŵ�ϴ�.
            scrollView.verticalNormalizedPosition = 1f;
        }
    }

    // ������Ʈ�� �ı��� �� �̺�Ʈ �����ʸ� �����Ͽ� �޸� ���� ����
    void OnDestroy()
    {
        if (inputField != null)
        {
            inputField.onValueChanged.RemoveListener(OnInputFieldValueChanged);
        }
    }
}*/