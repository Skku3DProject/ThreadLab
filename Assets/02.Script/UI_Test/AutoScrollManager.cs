using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro 관련 클래스를 사용하기 위해 필요

public class AutoScrollManager : MonoBehaviour
{
    // 유니티 에디터에서 할당할 UI 요소들
    public TMP_InputField inputField;
    public ScrollRect scrollView;
    public RectTransform contentRectTransform;
    public RectTransform viewportRectTransform;

    [Header("InputField Height Settings")]
    public float minInputFieldHeight = 50f; // InputField의 최소 높이 (텍스트가 없을 때)
    public float inputFieldPadding = 10f;   // InputField 텍스트 주변의 여백 (위아래)

    [Header("Scroll Activation Settings")]
    public float scrollActivationThreshold = 300f; // Content의 높이가 이 값을 초과하면 스크롤 활성화

    [Header("Bottom Padding Settings")]
    public float bottomPadding = 100f; // Content 하단에 추가할 여백

    private float _initialContentHeightExcludingInputField;

    void Start()
    {
        if (inputField == null || scrollView == null || contentRectTransform == null || viewportRectTransform == null)
        {
            Debug.LogError("필수 UI 요소가 할당되지 않았습니다. 스크립트 필드를 채워주세요.");
            enabled = false;
            return;
        }

        // InputField 리스너 연결
        inputField.onValueChanged.AddListener(OnInputFieldValueChanged);

        // InputField 레이아웃 강제 갱신 후 초기 콘텐츠 높이 계산
        LayoutRebuilder.ForceRebuildLayoutImmediate(inputField.GetComponent<RectTransform>());
        _initialContentHeightExcludingInputField = contentRectTransform.sizeDelta.y - inputField.GetComponent<RectTransform>().sizeDelta.y;

        // 하단 여백 추가
        AddBottomPadding(bottomPadding);

        // 초기 텍스트 있을 경우 갱신
        OnInputFieldValueChanged(inputField.text);
    }

    // 텍스트가 바뀔 때 호출됨
    void OnInputFieldValueChanged(string newText)
    {
        float preferredTextHeight = inputField.textComponent.preferredHeight;
        float newInputFieldHeight = Mathf.Max(minInputFieldHeight, preferredTextHeight + inputFieldPadding);

        RectTransform inputFieldRect = inputField.GetComponent<RectTransform>();
        inputFieldRect.sizeDelta = new Vector2(inputFieldRect.sizeDelta.x, newInputFieldHeight);

        LayoutRebuilder.ForceRebuildLayoutImmediate(contentRectTransform);

        UpdateScrollState();
    }

    // 스크롤 여부와 위치 갱신
    private void UpdateScrollState()
    {
        float currentContentHeight = contentRectTransform.sizeDelta.y;

        if (currentContentHeight > scrollActivationThreshold)
        {
            scrollView.vertical = true;
            Canvas.ForceUpdateCanvases(); // 즉시 스크롤 반영
            scrollView.verticalNormalizedPosition = 0f; // 맨 아래로
        }
        else
        {
            scrollView.vertical = false;
            scrollView.verticalNormalizedPosition = 1f; // 맨 위로
        }
    }

    // Content의 맨 아래 여백을 추가하는 함수
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
using TMPro; // TextMeshPro 관련 클래스를 사용하기 위해 필요

public class AutoScrollManager : MonoBehaviour
{
    // 유니티 에디터에서 할당할 UI 요소들
    public TMP_InputField inputField;
    public ScrollRect scrollView;
    public RectTransform contentRectTransform;
    public RectTransform viewportRectTransform;

    [Header("InputField Height Settings")]
    public float minInputFieldHeight = 50f; // InputField의 최소 높이 (텍스트가 없을 때)
    public float inputFieldPadding = 10f; // InputField 텍스트 주변의 여백 (위아래)

    [Header("Scroll Activation Settings")]
    public float scrollActivationThreshold = 300f; // Content의 높이가 이 값을 초과하면 스크롤 활성화

    private float _initialContentHeightExcludingInputField; // InputField를 제외한 Content의 초기 높이

    void Start()
    {
        // 필수 UI 요소들이 할당되었는지 확인
        if (inputField == null || scrollView == null || contentRectTransform == null || viewportRectTransform == null)
        {
            Debug.LogError("필수 UI 요소가 할당되지 않았습니다. 스크립트 필드를 채워주세요.");
            enabled = false; // 스크립트 비활성화
            return;
        }

        // InputField의 OnValueChanged 이벤트에 함수 연결
        // 텍스트가 변경될 때마다 OnInputFieldValueChanged 함수가 호출됩니다.
        inputField.onValueChanged.AddListener(OnInputFieldValueChanged);

        // InputField를 제외한 Content의 초기 높이 계산
        // (예: UI Image 등 다른 자식 요소들의 높이)
        // Content의 초기 높이에서 InputField의 초기 높이를 빼서 계산합니다.
        // 초기 InputField의 높이도 LayoutRebuilder로 강제 업데이트 후 가져오는 것이 안전합니다.
        LayoutRebuilder.ForceRebuildLayoutImmediate(inputField.GetComponent<RectTransform>());
        _initialContentHeightExcludingInputField = contentRectTransform.sizeDelta.y - inputField.GetComponent<RectTransform>().sizeDelta.y;

        // 초기 스크롤 상태 업데이트
        // (Start 시점에 이미 텍스트가 있을 경우를 대비)
        OnInputFieldValueChanged(inputField.text);
    }

    // InputField의 텍스트가 변경될 때 호출되는 함수
    void OnInputFieldValueChanged(string newText)
    {
        // 1. TMP_InputField의 높이 조절
        // TextMeshProUGUI 컴포넌트가 텍스트를 표시하는 데 필요한 실제 높이를 계산합니다.
        // 이때, InputField의 현재 너비를 기준으로 줄바꿈이 적용된 높이가 계산됩니다.
        float preferredTextHeight = inputField.textComponent.preferredHeight;

        // InputField의 최종 높이를 계산합니다. (최소 높이 또는 텍스트 높이 + 패딩 중 큰 값)
        float newInputFieldHeight = Mathf.Max(minInputFieldHeight, preferredTextHeight + inputFieldPadding);

        // InputField의 RectTransform 높이 업데이트
        RectTransform inputFieldRect = inputField.GetComponent<RectTransform>();
        inputFieldRect.sizeDelta = new Vector2(inputFieldRect.sizeDelta.x, newInputFieldHeight);

        // 2. Content의 높이 재계산 강제
        // InputField의 높이가 변경되었으므로, Content의 Vertical Layout Group과
        // Content Size Fitter가 Content의 총 높이를 다시 계산하도록 강제합니다.
        // 이 과정이 없으면 레이아웃 업데이트가 지연되어 스크롤이 제대로 동작하지 않을 수 있습니다.
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentRectTransform);

        // 3. 스크롤 상태 업데이트
        UpdateScrollState();
    }

    // 스크롤 활성화/비활성화 상태를 업데이트하는 함수
    private void UpdateScrollState()
    {
        // Content의 현재 총 높이
        float currentContentHeight = contentRectTransform.sizeDelta.y;

        // Content의 높이가 설정한 임계값(scrollActivationThreshold)을 초과하면 스크롤 활성화
        if (currentContentHeight > scrollActivationThreshold)
        {
            scrollView.vertical = true; // 세로 스크롤 활성화
            // 스크롤이 활성화되면 자동으로 가장 아래로 스크롤합니다.
            // verticalNormalizedPosition은 0이 최하단, 1이 최상단입니다.
            scrollView.verticalNormalizedPosition = 0f;
        }
        else
        {
            scrollView.vertical = false; // 세로 스크롤 비활성화
            // 스크롤이 비활성화되면 Content를 항상 상단에 위치시킵니다.
            scrollView.verticalNormalizedPosition = 1f;
        }
    }

    // 오브젝트가 파괴될 때 이벤트 리스너를 해제하여 메모리 누수 방지
    void OnDestroy()
    {
        if (inputField != null)
        {
            inputField.onValueChanged.RemoveListener(OnInputFieldValueChanged);
        }
    }
}*/