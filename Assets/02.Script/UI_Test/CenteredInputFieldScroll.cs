using UnityEngine;
using UnityEngine.UI; // UnityEngine.UI는 ScrollRect와 Scrollbar를 위해 필요합니다.
using TMPro; // TextMeshPro 관련 컴포넌트를 사용하기 위해 필요합니다.

public class CenterInputFieldScroll : MonoBehaviour
{
    public TMP_InputField tmpInputField; // 인스펙터에서 TMP_InputField를 연결하세요.
    public ScrollRect scrollRect;       // 인스펙터에서 Scroll Rect를 연결하세요.

    void Start()
    {
        // TMP_InputField와 ScrollRect가 연결되었는지 확인
        if (tmpInputField == null)
        {
            Debug.LogError("TMP_InputField가 연결되지 않았습니다. 인스펙터에서 설정해주세요.");
            return;
        }
        if (scrollRect == null)
        {
            Debug.LogError("ScrollRect가 연결되지 않았습니다. 인스펙터에서 설정해주세요.");
            return;
        }

        // InputField의 텍스트 변경 이벤트에 리스너 추가
        tmpInputField.onValueChanged.AddListener(OnInputFieldValueChanged);

        // ScrollRect의 Content에 TMP_InputField의 Text (TMP) 컴포넌트가 연결되어 있는지 확인
        // 일반적으로 TMP InputField를 생성하면 자식으로 Text (TMP)가 자동으로 생성됩니다.
        // 그리고 이 Text (TMP)에 ContentSizeFitter를 추가하고 Preferred Size로 설정해야 합니다.
        if (scrollRect.content == null)
        {
            Debug.LogWarning("Scroll Rect의 Content가 연결되지 않았습니다. TMP_InputField의 자식인 Text (TMP)를 연결해야 합니다.");
        }
    }

    void OnInputFieldValueChanged(string newText)
    {
        // 텍스트 내용이 변경될 때마다 Scroll Rect의 Content 크기를 다시 계산합니다.
        // 이는 Content에 ContentSizeFitter가 부착되어 있을 경우,
        // 텍스트 내용에 따라 Content의 높이가 자동으로 조절되도록 합니다.
        // 이 후 ScrollRect가 이 변경된 Content 높이를 기반으로 스크롤 가능 여부를 판단합니다.
        if (scrollRect.content != null)
        {
            // RectTransform의 레이아웃을 즉시 재빌드하여 크기 변경을 반영합니다.
            // 이 호출은 ContentSizeFitter가 Content의 크기를 정확히 계산하도록 돕습니다.
            LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content.GetComponent<RectTransform>());
        }
    }
}

/*using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro를 사용하는 경우 추가

public class CenteredInputFieldScroll : MonoBehaviour
{
    // === Inspector에서 할당해야 할 요소들 ===
    [Tooltip("InputField를 감싸고 있는 ScrollRect 컴포넌트입니다.")]
    public ScrollRect scrollRect;
    [Tooltip("ScrollRect의 Content RectTransform입니다. InputField가 이 Content의 자식이어야 합니다.")]
    public RectTransform contentRectTransform;
    [Tooltip("현재 스크립트가 붙어있는 InputField의 RectTransform입니다.")]
    public RectTransform inputFieldRectTransform; // 이 스크립트가 붙어있는 InputField의 RectTransform

    // === 내부적으로 사용할 InputField 컴포넌트 ===
    private InputField legacyInputField;
    private TMP_InputField tmpInputField;

    void Awake()
    {
        // Start()에서 실행될 경우, UI 레이아웃 업데이트가 완전히 끝나지 않을 수 있으므로 Awake()에서 초기화합니다.
        // 사용하고 있는 InputField 타입에 따라 컴포넌트를 가져오고 이벤트 리스너를 추가합니다.
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

        // 필수 컴포넌트 할당 여부 확인
        if (scrollRect == null)
        {
            Debug.LogError("CenteredInputFieldScroll: ScrollRect가 할당되지 않았습니다! 스크립트가 작동하지 않습니다.", this);
        }
        if (contentRectTransform == null)
        {
            Debug.LogError("CenteredInputFieldScroll: Content RectTransform이 할당되지 않았습니다! 스크립트가 작동하지 않습니다.", this);
        }
        if (inputFieldRectTransform == null)
        {
            Debug.LogError("CenteredInputFieldScroll: InputField RectTransform이 할당되지 않았습니다! 스크립트가 작동하지 않습니다.", this);
        }
    }

    void OnInputValueChanged(string newText)
    {
        // 텍스트 변경 시 다음 프레임에 스크롤 위치를 조정하도록 코루틴 호출
        // Content Size Fitter 등이 크기를 업데이트한 후에 스크롤을 조정해야 하기 때문에 한 프레임을 기다립니다.
        StartCoroutine(AdjustScrollPositionNextFrame());
    }

    /// <summary>
    /// UI 레이아웃 업데이트 이후 InputField의 중앙에 맞춰 스크롤 위치를 조정합니다.
    /// </summary>
    System.Collections.IEnumerator AdjustScrollPositionNextFrame()
    {
        // UI 레이아웃 시스템이 모든 크기 조절을 마칠 때까지 기다립니다.
        // Canvas.ForceUpdateCanvases()를 사용하는 방법도 있지만, yield return null이 더 부드러울 수 있습니다.
        yield return null;

        // 필수 컴포넌트가 null인지 다시 확인합니다. (플레이 중 컴포넌트가 사라질 수도 있기 때문)
        if (scrollRect == null || contentRectTransform == null || inputFieldRectTransform == null || scrollRect.viewport == null)
        {
            Debug.LogWarning("CenteredInputFieldScroll: 필수 컴포넌트 중 일부가 null이어서 스크롤 조정을 건너뜝니다.");
            yield break;
        }

        // Content의 현재 전체 높이
        float contentHeight = contentRectTransform.rect.height;
        // ScrollRect의 Viewport 높이 (실제로 보이는 영역)
        float viewportHeight = scrollRect.viewport.rect.height;

        // InputField의 Content RectTransform 내에서의 상대적인 Y 위치를 계산합니다.
        // InputField의 피벗(pivot)이 어디에 있든 Content의 로컬 좌표계 기준으로 InputField의 상단 Y를 찾습니다.
        // RectTransform.InverseTransformPoint를 사용하여 월드 좌표를 로컬 좌표로 변환합니다.
        // 여기서는 InputField의 중심 Y를 기준으로 계산하는 것이 더 직관적입니다.
        float inputFieldCenterYInContent = contentRectTransform.InverseTransformPoint(inputFieldRectTransform.position).y;

        // 스크롤 가능한 전체 범위 (Content가 Viewport보다 클 때만 스크롤 가능)
        float scrollableRange = contentHeight - viewportHeight;

        // 스크롤할 필요가 없는 경우 (Content가 Viewport보다 작거나 같을 때)
        if (scrollableRange <= 0)
        {
            // 스크롤이 필요 없을 때는 맨 위에 위치시킵니다 (정규화된 위치 1.0).
            // InputField가 단 한 줄이라도 화면에 보이기 위함입니다.
            scrollRect.verticalNormalizedPosition = 1f;
            yield break;
        }

        // 목표 스크롤 위치 계산: InputField의 중심이 Viewport의 중심에 오도록 합니다.
        // ScrollRect.verticalNormalizedPosition은 0 (하단)에서 1 (상단)까지의 값입니다.
        // Viewport의 중심은 Viewport 높이의 절반입니다.
        // Content의 하단을 0으로 봤을 때, InputField의 중심이 Viewport의 중심에 오려면
        // Content가 얼마나 위로 올라가야 하는지를 계산합니다.
        float targetScrollY = inputFieldCenterYInContent - (viewportHeight * 0.5f);

        // 계산된 targetScrollY를 정규화된 스크롤 위치 (0 ~ 1)로 변환합니다.
        // 스크롤 Rect의 verticalNormalizedPosition은 상단이 1.0, 하단이 0.0입니다.
        // Content가 위로 올라갈수록 (스크롤 바가 아래로 내려갈수록) verticalNormalizedPosition은 작아집니다.
        // 따라서 1에서 (targetScrollY / scrollableRange)를 빼줍니다.
        float normalizedPosition = 1f - (targetScrollY / scrollableRange);

        // 스크롤 위치를 0과 1 사이로 강제 제한합니다.
        normalizedPosition = Mathf.Clamp01(normalizedPosition);

        // 최종 스크롤 위치를 적용합니다.
        scrollRect.verticalNormalizedPosition = normalizedPosition;
    }

    // 컴포넌트가 비활성화되거나 파괴될 때 이벤트 리스너를 제거하여 메모리 누수를 방지합니다.
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