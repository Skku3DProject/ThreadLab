using System.Collections.Generic;
using UnityEngine;

public class PostUiManager : MonoBehaviourSingleton<PostUiManager>
{
    public List<UI_PostPopup> PostPopupList;
    protected override void Awake()
    {
        base.Awake();
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
}
