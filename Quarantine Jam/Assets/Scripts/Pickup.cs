using System;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public PlayerController player;
    public KeyCards color;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        player.PickUpKeyCard((int) color);
        Destroy(gameObject);
    }
}