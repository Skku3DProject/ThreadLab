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

    private List<GameObject> _posts = new List<GameObject>();

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
        foreach (var post in _posts)
        {
            Destroy(post);
        }    

        List<PostDTO> postDTOs = PostManager.Instance.PostList;
        foreach(var post in postDTOs)
        {
            GameObject postslot = Instantiate(PostLostPrefab, Content.transform);
            _posts.Add(postslot);
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
        Debug.Log("ShowMain");
    }

    public void ShowDetailPost()
    {
        PostUiManager.Instance.ShowDetailPost();
    }

}
