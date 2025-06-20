using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using TMPro;

public class UI_PostWrithing : MonoBehaviour
{
    public TMP_InputField InputField;

    public Button CreateButton;

    public void Start()
    {
        CreateButton.onClick.AddListener(() => _ = CreatePost());
    }
    public async Task CreatePost()
    {
        await PostManager.Instance.AddPost(InputField.text);
        PostUIManager.Instance.ShowMainPost();
        InputField.text = "";
    }
    public void ShowMainPost()
    {
        PostUIManager.Instance.ShowMainPost();
        InputField.text = "";
    }
}
