using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractionController : MonoBehaviour
{
    public bool CanHide { get; set; }
    private bool CanOpenDoor { get; set; }
    public bool CanRead { get; set; }
    public bool IsReading { get; private set; }

    private Door _activeDoor;
    private KeyColor _activeDoorColor;
    private Documents _activeDocument;

    private readonly bool[] _keys = new bool[Enum.GetValues(typeof(KeyColor)).Length];
    public GameObject[] slots;

    private void Awake()
    {
        IsReading = false;
    }

    public void Interact()
    {
        if (CanHide)
        {
            Hide();
        }

        if (CanOpenDoor)
        {
            UseKey((int) _activeDoorColor, _activeDoor);
        }

        if (!CanRead) return;
        _activeDocument.Read();
        CanRead = false;
        IsReading = true;
    }


    public void PickUpKeyCard(int color)
    {
        _keys[color] = true;
        Debug.Log("Picked up the " + (KeyColor) color + " card.");
        slots[color].GetComponent<Image>().color += new Color(0, 0, 0, 1);
    }

    private void UseKey(int color, Door pDoor)
    {
        _keys[color] = false;
        Debug.Log("Used the " + (KeyColor) color + " card.");
        slots[color].GetComponent<Image>().color -= new Color(0, 0, 0, 1);
        pDoor.OpenDoor();
    }

    public bool HasKeyOfColor(KeyColor color)
    {
        return _keys[(int) color];
    }

    private void Hide()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        CanHide = false;
    }

    public void Reveal()
    {
        GetComponent<SpriteRenderer>().enabled = true;
    }

    public void GetActiveDoor(KeyColor color, Door pDoor)
    {
        _activeDoor = pDoor;
        _activeDoorColor = color;
        CanOpenDoor = true;
    }

    public void GetActiveDocument(Documents documents)
    {
        _activeDocument = documents;
        CanRead = true;
    }
}