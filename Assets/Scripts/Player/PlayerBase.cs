using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    public List<GameObject> followers;

    private void Awake()
    {
        followers = new List<GameObject>();
    }

    private void LocateFollowers()
    {
        foreach (GameObject f in followers)
        {

        }
    }
}
