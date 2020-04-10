using TMPro;
using UnityEngine;

public class HidingPlace : MonoBehaviour
{
    private PlayerInteractionController _playerInteractionController;
    private TextMeshPro _textMeshPro;

    private void Start()
    {
        _textMeshPro = GetComponentInChildren<TextMeshPro>();
        _textMeshPro.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        _playerInteractionController = other.GetComponent<PlayerInteractionController>();
        _playerInteractionController.CanHide = true;
        _textMeshPro.enabled = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        _playerInteractionController = other.GetComponent<PlayerInteractionController>();
        _playerInteractionController.CanHide = false;
        _playerInteractionController.Reveal();
        _textMeshPro.enabled = false;
    }
}