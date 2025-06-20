using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Post : MonoBehaviour
{
    public TMP_InputField InputField;

    public GameObject Content;
    public GameObject PostLostPrefab;

    public Button createButton;

    public void Start()
    {
        ShowMainPost();
        createButton.onClick.AddListener(() => _ = CreatePost());
        PostManager.Instance.OnDataChanged += Refrlash;
    }

    public async Task CreatePost()
    {
        //Debug.LogWarning("?????");
        //GameObject postslot = Instantiate(PostLostPrefab, Content.transform);
        //UI_PostSlot uI_PostSlot = postslot.GetComponent<UI_PostSlot>();
        //uI_PostSlot.PostText.text = InputField.text;

        await PostManager.Instance.AddPost(InputField.text);
        ShowMainPost();
    }

    public void Refrlash()
    {
        List<PostDTO> postDTOs = PostManager.Instance.PostList;
        foreach(var post in postDTOs)
        {
            GameObject postslot = Instantiate(PostLostPrefab, Content.transform);
            UI_PostSlot uI_PostSlot = postslot.GetComponent<UI_PostSlot>();
            uI_PostSlot.Refresh(post);
        }
        ShowMainPost();
    }
    public void ShowWrithPost()
    {
        PostUiManager.Instance.ShowWrithPost();
    }
    public void ShowMainPost()
    {
        PostUiManager.Instance.ShowMainPost();
    }

    public void ShowDetailPost()
    {
        PostUiManager.Instance.ShowDetailPost();
    }

}
