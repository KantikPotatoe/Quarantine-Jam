using System;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Button : MonoBehaviour
{
    [FormerlySerializedAs("_textMeshPro")] [SerializeField]
    private TextMeshProUGUI textMeshPro;

    // Start is called before the first frame update


    private void Update()
    {
        textMeshPro.color = IsMouseOver() ? Color.black : new Color(0, 255, 0);
    }

    private static bool IsMouseOver()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    // Update is called once per frame
}