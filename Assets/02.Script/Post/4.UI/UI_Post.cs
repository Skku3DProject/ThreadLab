using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Post : MonoBehaviour
{
    public TMP_InputField InputField;

    public List<UI_PostPopup> PostPopupList;

    public GameObject Content;
    public GameObject PostLostPrefab;

    public Button createButton;

    public void Start()
    {
        ShowMainPost();
        createButton.onClick.AddListener(() => _ = CreatePost());
    }

    public async Task CreatePost()
    {
        Debug.LogWarning("?????");
        GameObject postslot = Instantiate(PostLostPrefab, Content.transform);
        UI_PostSlot uI_PostSlot = postslot.GetComponent<UI_PostSlot>();
        uI_PostSlot.UI_Post = this;
        uI_PostSlot.PostText.text = InputField.text;

        await PostManager.Instance.AddPost(InputField.text);
        ShowMainPost();
    }

    public void ShowWrithPost()
    {
        foreach(UI_PostPopup popup in PostPopupList)
        {
           if( popup.PostPopup == EPostPopup.WrithPost)
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
        PostManager.Instance.InvokeEvent();
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
