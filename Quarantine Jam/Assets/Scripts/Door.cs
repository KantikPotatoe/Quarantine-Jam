using UnityEngine;

public class Door : MonoBehaviour
{
    public KeyColor doorColor;

    private void OpenDoor()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        var player = other.GetComponent<PlayerController>();
        if (player.HasKeyOfColor(doorColor)) OpenDoor();
    }
}