using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    public override void Interact()
    {
        if (!_isActive)
            return;
        Ending.Instance.setResultCount(GameSceneManager.Instance.Player.ChickCount, GameSceneManager.Instance.CagesCount);
        if (GameSceneManager.Instance.GetCagesCount() == 0)
        {
            if (GameSceneManager.Instance.Player.ChickCount >= 55)
            {
                Ending.Instance.ShowEnding((int)EEndingList.Hero);
            }
            else if (GameSceneManager.Instance.Player.ChickCount > 0)
            {
                Ending.Instance.ShowEnding((int)EEndingList.Normal);
            }
            else
            {
                Ending.Instance.ShowEnding((int)EEndingList.Solo);
            }
        }
        else
        {
            if (GameSceneManager.Instance.Player.ChickCount == 0)
            {
                Ending.Instance.ShowEnding((int)EEndingList.Solo);
            }
            else
            {
                Ending.Instance.ShowEnding((int)EEndingList.Normal);
            }
        }
    }
}
