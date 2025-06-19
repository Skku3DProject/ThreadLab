using UnityEngine;
using UnityEngine.UI; // UI ���� ������Ʈ�� ����ϱ� ���� �ʿ��մϴ�.
using System.Collections; // �ڷ�ƾ�� ����ϱ� ���� �ʿ��մϴ�.

public class ListScrollActivate : MonoBehaviour
{
    public ScrollRect targetScrollRect; // ������ Scroll Rect ������Ʈ
    public RectTransform contentRect;    // Scroll Rect�� Content RectTransform
    public RectTransform viewportRect;   // Scroll Rect�� Viewport RectTransform

    private bool isChecking = false; // �ߺ� üũ ���� �÷���

    void Start()
    {
        // ������Ʈ �ڵ� �Ҵ� (���� ���������� ���մϴ�)
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
                // Viewport�� �������� ���� ���, Content�� �θ�(ScrollRect �Ʒ��� Viewport)�� ã������ �õ�
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
            enabled = false; // ��ũ��Ʈ ��Ȱ��ȭ
            return;
        }

        // �ʱ� �� �� üũ
        CheckScrollStatus();

        // Content�� ũ�� ��ȭ�� �����ϱ� ���� �ڷ�ƾ ����
        // Layout Group�̳� Content Size Fitter�� ���� ũ�Ⱑ ���� �� ��� �����ϱ� ����
        StartCoroutine(CheckScrollStatusRoutine());
    }

    // ��ũ�� ���¸� üũ�ϴ� ���� �Լ�
    void CheckScrollStatus()
    {
        if (targetScrollRect == null || contentRect == null || viewportRect == null)
        {
            Debug.LogWarning("Required RectTransforms or ScrollRect not assigned. Cannot check scroll status.");
            return;
        }

        // Content�� ���� ����
        float contentHeight = contentRect.rect.height;
        // Viewport�� ���� ���� (��ũ�� ������ ������ ����)
        float viewportHeight = viewportRect.rect.height;

        // Content�� Viewport���� ũ�� ��ũ�� Ȱ��ȭ
        // �׷��� ������ ��ũ�� ��Ȱ��ȭ
        targetScrollRect.enabled = (contentHeight > viewportHeight);

        // ����� �α� (���� ����)
        // Debug.Log($"Content Height: {contentHeight}, Viewport Height: {viewportHeight}, Scroll Enabled: {targetScrollRect.enabled}");
    }

    // Content�� ũ�� ��ȭ�� ���������� üũ�ϴ� �ڷ�ƾ
    IEnumerator CheckScrollStatusRoutine()
    {
        while (true)
        {
            // UI ����� ���̾ƿ� ����� �Ϸ�� ������ ���
            // Ư�� Content Size Fitter�� Layout Group�� ����� ��� �ʿ��մϴ�.
            yield return new WaitForEndOfFrame();

            if (!isChecking)
            {
                isChecking = true;
                CheckScrollStatus();
                isChecking = false;
            }
            yield return null; // ���� �����ӱ��� ���
        }
    }

    // �ܺο��� ��ũ�� ���¸� ������ ������Ʈ�ؾ� �� �� ȣ�� ����
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
        yield return new WaitForEndOfFrame(); // ���̾ƿ� ������Ʈ ���
        CheckScrollStatus();
        isChecking = false;
    }
}
