using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public class UI_Comments : MonoBehaviour
{
    [SerializeField] private Transform _slotRoot;
    [SerializeField] private GameObject _slotPrefab;

    private List<UI_CommentSlot> _commentSlots = new List<UI_CommentSlot>();

    private void Start()
    {
        CommentManager.Instance.OnDataChanged += RefreshComments;
        PostManager.Instance.OnShowDetail += RefreshComments;
    }

    public void RefreshComments(string postId)
    {
        // ���� ����
        foreach (var slot in _commentSlots)
        {
            Destroy(slot.gameObject);
        }
        _commentSlots.Clear();

        var comments = CommentManager.Instance.GetCommentsByPostId(postId); //���޵� postId ���

        foreach (var comment in comments)
        {
            var slotGO = Instantiate(_slotPrefab, _slotRoot);
            var slot = slotGO.GetComponent<UI_CommentSlot>();

            slot.Init(comment, this); // �θ� UI ����
            _commentSlots.Add(slot);
        }
    }
}
