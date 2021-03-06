﻿using UnityEngine;

public class PickUp : MonoBehaviour
{
    public KeyColor color;
    public PlayerInteractionController player;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        player.PickUpKeyCard((int) color);
        Destroy(gameObject);
    }
}