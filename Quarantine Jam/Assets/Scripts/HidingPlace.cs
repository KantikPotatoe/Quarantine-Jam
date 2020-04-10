using TMPro;
using UnityEngine;

public class HidingPlace : MonoBehaviour
{
    private PlayerController _playerController;
    private TextMeshPro _textMeshPro;

    private void Start()
    {
        _textMeshPro = GetComponentInChildren<TextMeshPro>();
        _textMeshPro.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        _playerController = other.GetComponent<PlayerController>();
        _playerController.CanHide = true;
        _textMeshPro.enabled = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        _playerController = other.GetComponent<PlayerController>();
        _playerController.CanHide = false;
        _playerController.Reveal();
        _textMeshPro.enabled = false;
    }
}