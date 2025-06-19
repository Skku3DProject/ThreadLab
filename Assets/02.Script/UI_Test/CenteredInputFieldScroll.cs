using UnityEngine;
using UnityEngine.UI; // UnityEngine.UI�� ScrollRect�� Scrollbar�� ���� �ʿ��մϴ�.
using TMPro; // TextMeshPro ���� ������Ʈ�� ����ϱ� ���� �ʿ��մϴ�.

public class CenterInputFieldScroll : MonoBehaviour
{
    public TMP_InputField tmpInputField; // �ν����Ϳ��� TMP_InputField�� �����ϼ���.
    public ScrollRect scrollRect;       // �ν����Ϳ��� Scroll Rect�� �����ϼ���.

    void Start()
    {
        // TMP_InputField�� ScrollRect�� ����Ǿ����� Ȯ��
        if (tmpInputField == null)
        {
            Debug.LogError("TMP_InputField�� ������� �ʾҽ��ϴ�. �ν����Ϳ��� �������ּ���.");
            return;
        }
        if (scrollRect == null)
        {
            Debug.LogError("ScrollRect�� ������� �ʾҽ��ϴ�. �ν����Ϳ��� �������ּ���.");
            return;
        }

        // InputField�� �ؽ�Ʈ ���� �̺�Ʈ�� ������ �߰�
        tmpInputField.onValueChanged.AddListener(OnInputFieldValueChanged);

        // ScrollRect�� Content�� TMP_InputField�� Text (TMP) ������Ʈ�� ����Ǿ� �ִ��� Ȯ��
        // �Ϲ������� TMP InputField�� �����ϸ� �ڽ����� Text (TMP)�� �ڵ����� �����˴ϴ�.
        // �׸��� �� Text (TMP)�� ContentSizeFitter�� �߰��ϰ� Preferred Size�� �����ؾ� �մϴ�.
        if (scrollRect.content == null)
        {
            Debug.LogWarning("Scroll Rect�� Content�� ������� �ʾҽ��ϴ�. TMP_InputField�� �ڽ��� Text (TMP)�� �����ؾ� �մϴ�.");
        }
    }

    void OnInputFieldValueChanged(string newText)
    {
        // �ؽ�Ʈ ������ ����� ������ Scroll Rect�� Content ũ�⸦ �ٽ� ����մϴ�.
        // �̴� Content�� ContentSizeFitter�� �����Ǿ� ���� ���,
        // �ؽ�Ʈ ���뿡 ���� Content�� ���̰� �ڵ����� �����ǵ��� �մϴ�.
        // �� �� ScrollRect�� �� ����� Content ���̸� ������� ��ũ�� ���� ���θ� �Ǵ��մϴ�.
        if (scrollRect.content != null)
        {
            // RectTransform�� ���̾ƿ��� ��� ������Ͽ� ũ�� ������ �ݿ��մϴ�.
            // �� ȣ���� ContentSizeFitter�� Content�� ũ�⸦ ��Ȯ�� ����ϵ��� �����ϴ�.
            LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content.GetComponent<RectTransform>());
        }
    }
}

/*using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro�� ����ϴ� ��� �߰�

public class CenteredInputFieldScroll : MonoBehaviour
{
    // === Inspector���� �Ҵ��ؾ� �� ��ҵ� ===
    [Tooltip("InputField�� ���ΰ� �ִ� ScrollRect ������Ʈ�Դϴ�.")]
    public ScrollRect scrollRect;
    [Tooltip("ScrollRect�� Content RectTransform�Դϴ�. InputField�� �� Content�� �ڽ��̾�� �մϴ�.")]
    public RectTransform contentRectTransform;
    [Tooltip("���� ��ũ��Ʈ�� �پ��ִ� InputField�� RectTransform�Դϴ�.")]
    public RectTransform inputFieldRectTransform; // �� ��ũ��Ʈ�� �پ��ִ� InputField�� RectTransform

    // === ���������� ����� InputField ������Ʈ ===
    private InputField legacyInputField;
    private TMP_InputField tmpInputField;

    void Awake()
    {
        // Start()���� ����� ���, UI ���̾ƿ� ������Ʈ�� ������ ������ ���� �� �����Ƿ� Awake()���� �ʱ�ȭ�մϴ�.
        // ����ϰ� �ִ� InputField Ÿ�Կ� ���� ������Ʈ�� �������� �̺�Ʈ �����ʸ� �߰��մϴ�.
        legacyInputField = GetComponent<InputField>();
        if (legacyInputField != null)
        {
            legacyInputField.onValueChanged.AddListener(OnInputValueChanged);
        }

        tmpInputField = GetComponent<TMP_InputField>();
        if (tmpInputField != null)
        {
            tmpInputField.onValueChanged.AddListener(OnInputValueChanged);
        }

        // �ʼ� ������Ʈ �Ҵ� ���� Ȯ��
        if (scrollRect == null)
        {
            Debug.LogError("CenteredInputFieldScroll: ScrollRect�� �Ҵ���� �ʾҽ��ϴ�! ��ũ��Ʈ�� �۵����� �ʽ��ϴ�.", this);
        }
        if (contentRectTransform == null)
        {
            Debug.LogError("CenteredInputFieldScroll: Content RectTransform�� �Ҵ���� �ʾҽ��ϴ�! ��ũ��Ʈ�� �۵����� �ʽ��ϴ�.", this);
        }
        if (inputFieldRectTransform == null)
        {
            Debug.LogError("CenteredInputFieldScroll: InputField RectTransform�� �Ҵ���� �ʾҽ��ϴ�! ��ũ��Ʈ�� �۵����� �ʽ��ϴ�.", this);
        }
    }

    void OnInputValueChanged(string newText)
    {
        // �ؽ�Ʈ ���� �� ���� �����ӿ� ��ũ�� ��ġ�� �����ϵ��� �ڷ�ƾ ȣ��
        // Content Size Fitter ���� ũ�⸦ ������Ʈ�� �Ŀ� ��ũ���� �����ؾ� �ϱ� ������ �� �������� ��ٸ��ϴ�.
        StartCoroutine(AdjustScrollPositionNextFrame());
    }

    /// <summary>
    /// UI ���̾ƿ� ������Ʈ ���� InputField�� �߾ӿ� ���� ��ũ�� ��ġ�� �����մϴ�.
    /// </summary>
    System.Collections.IEnumerator AdjustScrollPositionNextFrame()
    {
        // UI ���̾ƿ� �ý����� ��� ũ�� ������ ��ĥ ������ ��ٸ��ϴ�.
        // Canvas.ForceUpdateCanvases()�� ����ϴ� ����� ������, yield return null�� �� �ε巯�� �� �ֽ��ϴ�.
        yield return null;

        // �ʼ� ������Ʈ�� null���� �ٽ� Ȯ���մϴ�. (�÷��� �� ������Ʈ�� ����� ���� �ֱ� ����)
        if (scrollRect == null || contentRectTransform == null || inputFieldRectTransform == null || scrollRect.viewport == null)
        {
            Debug.LogWarning("CenteredInputFieldScroll: �ʼ� ������Ʈ �� �Ϻΰ� null�̾ ��ũ�� ������ �ǳʍ��ϴ�.");
            yield break;
        }

        // Content�� ���� ��ü ����
        float contentHeight = contentRectTransform.rect.height;
        // ScrollRect�� Viewport ���� (������ ���̴� ����)
        float viewportHeight = scrollRect.viewport.rect.height;

        // InputField�� Content RectTransform �������� ������� Y ��ġ�� ����մϴ�.
        // InputField�� �ǹ�(pivot)�� ��� �ֵ� Content�� ���� ��ǥ�� �������� InputField�� ��� Y�� ã���ϴ�.
        // RectTransform.InverseTransformPoint�� ����Ͽ� ���� ��ǥ�� ���� ��ǥ�� ��ȯ�մϴ�.
        // ���⼭�� InputField�� �߽� Y�� �������� ����ϴ� ���� �� �������Դϴ�.
        float inputFieldCenterYInContent = contentRectTransform.InverseTransformPoint(inputFieldRectTransform.position).y;

        // ��ũ�� ������ ��ü ���� (Content�� Viewport���� Ŭ ���� ��ũ�� ����)
        float scrollableRange = contentHeight - viewportHeight;

        // ��ũ���� �ʿ䰡 ���� ��� (Content�� Viewport���� �۰ų� ���� ��)
        if (scrollableRange <= 0)
        {
            // ��ũ���� �ʿ� ���� ���� �� ���� ��ġ��ŵ�ϴ� (����ȭ�� ��ġ 1.0).
            // InputField�� �� �� ���̶� ȭ�鿡 ���̱� �����Դϴ�.
            scrollRect.verticalNormalizedPosition = 1f;
            yield break;
        }

        // ��ǥ ��ũ�� ��ġ ���: InputField�� �߽��� Viewport�� �߽ɿ� ������ �մϴ�.
        // ScrollRect.verticalNormalizedPosition�� 0 (�ϴ�)���� 1 (���)������ ���Դϴ�.
        // Viewport�� �߽��� Viewport ������ �����Դϴ�.
        // Content�� �ϴ��� 0���� ���� ��, InputField�� �߽��� Viewport�� �߽ɿ� ������
        // Content�� �󸶳� ���� �ö󰡾� �ϴ����� ����մϴ�.
        float targetScrollY = inputFieldCenterYInContent - (viewportHeight * 0.5f);

        // ���� targetScrollY�� ����ȭ�� ��ũ�� ��ġ (0 ~ 1)�� ��ȯ�մϴ�.
        // ��ũ�� Rect�� verticalNormalizedPosition�� ����� 1.0, �ϴ��� 0.0�Դϴ�.
        // Content�� ���� �ö󰥼��� (��ũ�� �ٰ� �Ʒ��� ����������) verticalNormalizedPosition�� �۾����ϴ�.
        // ���� 1���� (targetScrollY / scrollableRange)�� ���ݴϴ�.
        float normalizedPosition = 1f - (targetScrollY / scrollableRange);

        // ��ũ�� ��ġ�� 0�� 1 ���̷� ���� �����մϴ�.
        normalizedPosition = Mathf.Clamp01(normalizedPosition);

        // ���� ��ũ�� ��ġ�� �����մϴ�.
        scrollRect.verticalNormalizedPosition = normalizedPosition;
    }

    // ������Ʈ�� ��Ȱ��ȭ�ǰų� �ı��� �� �̺�Ʈ �����ʸ� �����Ͽ� �޸� ������ �����մϴ�.
    void OnDestroy()
    {
        if (legacyInputField != null)
        {
            legacyInputField.onValueChanged.RemoveListener(OnInputValueChanged);
        }
        if (tmpInputField != null)
        {
            tmpInputField.onValueChanged.RemoveListener(OnInputValueChanged);
        }
    }
}
*/