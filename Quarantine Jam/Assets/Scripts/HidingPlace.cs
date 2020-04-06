using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.Universal;
using Vignette = UnityEngine.Rendering.Universal.Vignette;

public class HidingPlace : MonoBehaviour
{
    private PlayerController _playerController;
    private TextMeshPro _textMeshPro;
    public Camera postProcessCamera;

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
        var vignette = ScriptableObject.CreateInstance<Vignette>();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        _playerController = other.GetComponent<PlayerController>();
        _playerController.CanHide = false;
        _playerController.Reveal();
        _textMeshPro.enabled = false;
    }

    public void HidingText()
    {
        _textMeshPro.text = "Don't move...";
    }
}