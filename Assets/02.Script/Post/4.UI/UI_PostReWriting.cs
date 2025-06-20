using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using TMPro;

public class UI_PostReWriting : MonoBehaviour
{
    public TMP_InputField ReWrithInputField;
    public Button ReWriteButton;
    public void Start()
    {
        ReWriteButton.onClick.AddListener(() => _ = OnClickReWrite());
    }
    private void OnEnable()
    {
        RefrlashInputText();
    }

    public void RefrlashInputText()
    {
        ReWrithInputField.text = PostManager.Instance.CurrentPost.Text;
    }

    public void ShowMainPost()
    {
        PostUIManager.Instance.ShowMainPost();
    }
    public async Task OnClickReWrite()
    {
        await PostManager.Instance.UpdateWrite(ReWrithInputField.text);
    }
}
