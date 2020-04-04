using System;
using UnityEngine;

public class Door : MonoBehaviour
{
    private bool _isLocked;
    public KeyColor doorColor;

    private void Start()
    {
        _isLocked = true;
    }
}