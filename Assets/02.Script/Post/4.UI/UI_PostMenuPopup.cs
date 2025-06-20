using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UI_PostMenuPopup : MonoBehaviour
{
    public Button ReWrithButton;
    public Button DeleteButton;
    public void Start()
    {
        ReWrithButton.onClick.AddListener(() => OnClickReWrith());
        DeleteButton.onClick.AddListener(() => _ = OnClickDelete());
    }

    public async Task OnClickDelete()
    {
        await PostManager.Instance.DeletePost();
        ShowMainPost();
    }
 
    // �����ϱ�
    public void OnClickReWrith() 
    {
        if(AccountManager.Instance.MyAccount.Email != PostManager.Instance.CurrentPost.Email)
        {
            return;
        }

        PostUIManager.Instance.ShowReWrithPost();
        gameObject.SetActive(false);
    }

    // �����ϱ� 
    public void ShowMainPost() 
    {
        PostUIManager.Instance.ShowMainPost();
        gameObject.SetActive(false);
    }

}
