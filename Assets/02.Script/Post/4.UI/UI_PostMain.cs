using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Collections.Generic;

public class UI_PostMain : MonoBehaviour
{
    public GameObject EmptyImage;
    public GameObject PostPrefabTransfrom;
    public GameObject PostPrefab;

    private List<GameObject> _posts = new List<GameObject>();

    public void Start()
    {
        PostManager.Instance.OnDataChanged += Refrlash;

     
    }
    public void ShowWrithPost()
    {
        PostUIManager.Instance.ShowWrithPost();
    }
    public void Refrlash()
    {
        foreach (var post in _posts)
        {
            Destroy(post);
        }

        List<PostDTO> postDTOs = PostManager.Instance.PostList;
        foreach (var post in postDTOs)
        {
            GameObject postslot = Instantiate(PostPrefab, PostPrefabTransfrom.transform);
            _posts.Add(postslot);
            UI_PostSlot uI_PostSlot = postslot.GetComponent<UI_PostSlot>();
            uI_PostSlot.Refresh(post);
        }
        
        if (_posts.Count <= 0)
        {
            EmptyImage.SetActive(true);
        }
        else
        { 
            EmptyImage.SetActive(false);
        }
    }
}
