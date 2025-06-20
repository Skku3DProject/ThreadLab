using System.Collections.Generic;
using UnityEngine;
using System;

public class PostUIManager : MonoBehaviourSingleton<PostUIManager>
{
    public UI_PostMain UI_PostMain;
    public UI_PostReWriting UI_ReWriting;
    public UI_PostWrithing UI_Writhing;
    public UI_PostDetail UI_Detail;
    public List<UI_PostPopup> PostPopupList;

    public RectTransform Menu_PanelRectTransform;

    public event Action OnShowPostDetail;
    protected override void Awake()
    {
        base.Awake();
    }
    public void MenuOnOff(RectTransform targetRectTransform)
    {
        if(Menu_PanelRectTransform.gameObject.activeSelf)
        {
            Menu_PanelRectTransform.gameObject.SetActive(false);
            return;
        }

        Menu_PanelRectTransform.gameObject.SetActive(true);

        Vector3 targetLocalPosition = Menu_PanelRectTransform.parent.InverseTransformPoint(targetRectTransform.position);

        // 버튼 아래쪽에 메뉴 표시 (오프셋 추가)
        Vector2 menuSize = Menu_PanelRectTransform.sizeDelta;
        Vector2 buttonSize = targetRectTransform.sizeDelta;

        targetLocalPosition.y -= (buttonSize.y / 2 + menuSize.y / 2 + 10f); // 버튼 아래 10px 여백

        // 경계 체크 후 조정
        Canvas canvas = Menu_PanelRectTransform.GetComponentInParent<Canvas>();
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        float margin = 10f;
        float leftBound = -canvasRect.rect.width / 2 + menuSize.x / 2 + margin;
        float rightBound = canvasRect.rect.width / 2 - menuSize.x / 2 - margin;
        float bottomBound = -canvasRect.rect.height / 2 + menuSize.y / 2 + margin;
        float topBound = canvasRect.rect.height / 2 - menuSize.y / 2 - margin;

        targetLocalPosition.x = Mathf.Clamp(targetLocalPosition.x, leftBound, rightBound);
        targetLocalPosition.y = Mathf.Clamp(targetLocalPosition.y, bottomBound, topBound);

        Menu_PanelRectTransform.localPosition = targetLocalPosition;
    }

    public void ShowWrithPost()
    {
        foreach (UI_PostPopup popup in PostPopupList)
        {
            if (popup.PostPopup == EPostPopup.WrithPost)
            {
                popup.gameObject.SetActive(true);
                continue;
            }
            popup.gameObject.SetActive(false);
        }

        Menu_PanelRectTransform.gameObject.SetActive(false);
    }
    public void ShowMainPost()
    {
        foreach (UI_PostPopup popup in PostPopupList)
        {
            if (popup.PostPopup == EPostPopup.MainPost)
            {
                popup.gameObject.SetActive(true);
                continue;
            }
            popup.gameObject.SetActive(false);
        }
        Menu_PanelRectTransform.gameObject.SetActive(false);
    }

    public void ShowDetailPost()
    {
        foreach (UI_PostPopup popup in PostPopupList)
        {
            if (popup.PostPopup == EPostPopup.DetailPost)
            {
                popup.gameObject.SetActive(true);
                continue;
            }
            popup.gameObject.SetActive(false);
        }
        Menu_PanelRectTransform.gameObject.SetActive(false);
        OnShowPostDetail?.Invoke();
    }

    public void ShowReWrithPost()
    {
        foreach (UI_PostPopup popup in PostPopupList)
        {
            if (popup.PostPopup == EPostPopup.ReWrithPost)
            {
                popup.gameObject.SetActive(true);
                continue;
            }
            popup.gameObject.SetActive(false);
        }
        Menu_PanelRectTransform.gameObject.SetActive(false);
    }
}
