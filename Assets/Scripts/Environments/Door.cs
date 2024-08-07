using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    public override void Interact()
    {
        if (GameSceneManager.Instance.GetCagesCount() == 0)
        {
            if (GameSceneManager.Instance.Player.ChickCount == 0)
            {
                Ending.Instance.ShowEnding((int)EEndingList.Psycho);
            }
            else
            {
                Ending.Instance.ShowEnding((int)EEndingList.Mass);
            }
        }
        else
        {
            if (GameSceneManager.Instance.Player.ChickCount == 0)
            {
                Ending.Instance.ShowEnding((int)EEndingList.Sad);
            }
            else
            {
                Ending.Instance.ShowEnding((int)EEndingList.Normal);
            }
        }
    }
}
