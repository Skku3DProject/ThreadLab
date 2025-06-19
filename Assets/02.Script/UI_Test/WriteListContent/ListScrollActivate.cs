using UnityEngine;
using UnityEngine.UI; // UI 관련 컴포넌트를 사용하기 위해 필요합니다.
using System.Collections; // 코루틴을 사용하기 위해 필요합니다.

public class ListScrollActivate : MonoBehaviour
{
    public ScrollRect targetScrollRect; // 제어할 Scroll Rect 컴포넌트
    public RectTransform contentRect;    // Scroll Rect의 Content RectTransform
    public RectTransform viewportRect;   // Scroll Rect의 Viewport RectTransform

    private bool isChecking = false; // 중복 체크 방지 플래그

    void Start()
    {
        // 컴포넌트 자동 할당 (선택 사항이지만 편리합니다)
        if (targetScrollRect == null)
        {
            targetScrollRect = GetComponent<ScrollRect>();
        }
        if (targetScrollRect != null)
        {
            if (contentRect == null)
            {
                contentRect = targetScrollRect.content;
            }
            if (viewportRect == null && targetScrollRect.viewport != null)
            {
                viewportRect = targetScrollRect.viewport;
            }
            else if (viewportRect == null && targetScrollRect.viewport == null)
            {
                // Viewport가 설정되지 않은 경우, Content의 부모(ScrollRect 아래의 Viewport)를 찾으려고 시도
                Transform currentParent = contentRect.parent;
                if (currentParent != null && currentParent.GetComponent<Mask>() != null)
                {
                    viewportRect = currentParent.GetComponent<RectTransform>();
                }
                else
                {
                    Debug.LogWarning("Viewport RectTransform not assigned and could not be found. Please assign it manually.");
                }
            }
        }
        else
        {
            Debug.LogError("ScrollRect component not found on this GameObject. Please assign it or add the script to the ScrollView GameObject.");
            enabled = false; // 스크립트 비활성화
            return;
        }

        // 초기 한 번 체크
        CheckScrollStatus();

        // Content의 크기 변화를 감지하기 위해 코루틴 시작
        // Layout Group이나 Content Size Fitter에 의해 크기가 변할 때 즉시 감지하기 위함
        StartCoroutine(CheckScrollStatusRoutine());
    }

    // 스크롤 상태를 체크하는 메인 함수
    void CheckScrollStatus()
    {
        if (targetScrollRect == null || contentRect == null || viewportRect == null)
        {
            Debug.LogWarning("Required RectTransforms or ScrollRect not assigned. Cannot check scroll status.");
            return;
        }

        // Content의 실제 높이
        float contentHeight = contentRect.rect.height;
        // Viewport의 실제 높이 (스크롤 가능한 영역의 높이)
        float viewportHeight = viewportRect.rect.height;

        // Content가 Viewport보다 크면 스크롤 활성화
        // 그렇지 않으면 스크롤 비활성화
        targetScrollRect.enabled = (contentHeight > viewportHeight);

        // 디버그 로그 (선택 사항)
        // Debug.Log($"Content Height: {contentHeight}, Viewport Height: {viewportHeight}, Scroll Enabled: {targetScrollRect.enabled}");
    }

    // Content의 크기 변화를 지속적으로 체크하는 코루틴
    IEnumerator CheckScrollStatusRoutine()
    {
        while (true)
        {
            // UI 요소의 레이아웃 계산이 완료될 때까지 대기
            // 특히 Content Size Fitter나 Layout Group이 적용된 경우 필요합니다.
            yield return new WaitForEndOfFrame();

            if (!isChecking)
            {
                isChecking = true;
                CheckScrollStatus();
                isChecking = false;
            }
            yield return null; // 다음 프레임까지 대기
        }
    }

    // 외부에서 스크롤 상태를 강제로 업데이트해야 할 때 호출 가능
    public void ForceCheckScrollStatus()
    {
        if (!isChecking)
        {
            StartCoroutine(CheckSingleCheckRoutine());
        }
    }

    private IEnumerator CheckSingleCheckRoutine()
    {
        isChecking = true;
        yield return new WaitForEndOfFrame(); // 레이아웃 업데이트 대기
        CheckScrollStatus();
        isChecking = false;
    }
}
