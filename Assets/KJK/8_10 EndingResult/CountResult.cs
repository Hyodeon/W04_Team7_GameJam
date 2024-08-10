using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CountResult : MonoBehaviour
{
    [SerializeField] private PlayerBase _player;
    [SerializeField] private GameObject _result;
    [SerializeField] private GameObject[] _chickens;
    private int chickCount;
    void Awake()
    {
        _chickens = new GameObject[_result.transform.childCount];
        for (int i = 0; i < _result.transform.childCount; i++)
        {
            _chickens[i] = _result.transform.GetChild(i).gameObject;
        }
    }
    void OnEnable()
    {
        StartCoroutine(ShowResult());
        chickCount = _player.ChickCount;
    }

    IEnumerator ShowResult()
    {
        for (int i = 0; i < chickCount; i++)
        {
            _chickens[i].SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
