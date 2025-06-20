using System.Collections.Generic;
using UnityEngine;

public class PostUIManager : MonoBehaviourSingleton<PostUIManager>
{
    public UI_PostMain UI_PostMain;
    public UI_PostReWriting UI_ReWriting;
    public UI_PostWrithing UI_Writhing;
    public UI_PostDetail UI_Detail;
    public List<UI_PostPopup> PostPopupList;

    public RectTransform Menu_PanelRectTransform;

    protected override void Awake()
    {
        base.Awake();
    }


    public void MoveMenu(RectTransform RectTransform)
    {
        Menu_PanelRectTransform.gameObject.SetActive(true);
        if (Menu_PanelRectTransform == null)
        {
            Debug.LogError("Target UI RectTransform이 할당되지 않았습니다. Inspector에서 할당해주세요.");
            return;
        }

        Menu_PanelRectTransform.localPosition = RectTransform.localPosition;
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
    }
}
