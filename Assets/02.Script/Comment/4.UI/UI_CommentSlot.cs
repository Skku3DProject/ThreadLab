using Firebase.Firestore;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CommentSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _userName;
    [SerializeField] private TextMeshProUGUI _mainText;
    [SerializeField] private TextMeshProUGUI _timeText;

    public void Refresh(string username, string maintext, Timestamp timestamp)
    {
        _userName.text = username;
        _mainText.text = maintext;
        _timeText.text = timestamp.ToDateTime().ToString();
    }

}
