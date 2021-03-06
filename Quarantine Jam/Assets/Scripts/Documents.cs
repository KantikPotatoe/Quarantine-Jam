﻿using TMPro;
using UnityEngine;

public class Documents : MonoBehaviour
{
    public GameObject panel;
    public string noteText;
    private TextMeshProUGUI _panelNoteText;
    private PlayerInteractionController _playerInteractionController;
    private TextMeshPro _textMeshPro;


    private void Start()
    {
        panel.SetActive(false);
        _panelNoteText = panel.GetComponentInChildren<TextMeshProUGUI>();
        _textMeshPro = GetComponentInChildren<TextMeshPro>();
        _textMeshPro.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _textMeshPro.enabled = true;
        if (!other.CompareTag("Player")) return;
        _playerInteractionController = other.GetComponent<PlayerInteractionController>();
        _playerInteractionController.GetActiveDocument(this);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _textMeshPro.enabled = false;
        if (!other.CompareTag("Player")) return;
        _playerInteractionController = other.GetComponent<PlayerInteractionController>();
        _playerInteractionController.CanRead = false;
    }

    public void Read()
    {
        _panelNoteText.text = noteText;
        panel.SetActive(true);
    }
}