using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffSwitch : Interactable
{
    private bool _enabled = false;
    [SerializeField] private GameObject[] _traps;
    public override void Interact()
    {
        if (!_isActive || _enabled)
            return;


        for (int i = 0; i < _traps.Length; i++)
        {
            Destroy(_traps[i]);
        }
        DestroyInteractable();
    }
}
