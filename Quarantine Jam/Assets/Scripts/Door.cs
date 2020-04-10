using TMPro;
using UnityEngine;

public class Door : MonoBehaviour
{
    public KeyColor doorColor;
    private TextMeshPro _textMeshPro;

    private void Start()
    {
        _textMeshPro = GetComponentInChildren<TextMeshPro>();
        _textMeshPro.enabled = false;
    }

    public void OpenDoor()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _textMeshPro.enabled = true;
        if (!other.CompareTag("Player")) return;
        var player = other.GetComponent<PlayerInteractionController>();
        if (!player.HasKeyOfColor(doorColor)) return;
        player.GetActiveDoor(doorColor, this);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _textMeshPro.enabled = false;
    }
}