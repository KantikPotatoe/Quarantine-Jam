using TMPro;
using UnityEngine;

public class Documents : MonoBehaviour
{
    public GameObject panel;
    public string noteText;
    private TextMeshProUGUI _panelNoteText;
    private PlayerController _playerController;

    private void Start()
    {
        panel.SetActive(false);
        _panelNoteText = panel.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        _playerController = other.GetComponent<PlayerController>();
        _panelNoteText.text = noteText;
        panel.SetActive(true);
        _playerController.IsReading = true;
    }
}