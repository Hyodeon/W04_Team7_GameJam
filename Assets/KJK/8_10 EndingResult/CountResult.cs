using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CountResult : MonoBehaviour
{
    [SerializeField] private PlayerBase _player;
    [SerializeField] private GameObject _result;
    [SerializeField] private GameObject[] _chickens;
    private int _chickCount;
    private int _cageCount;
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
        _chickCount = Ending.Instance.GetChickCount();
        _cageCount = Ending.Instance.GetCageCount();
        Debug.Log(_chickCount);
        StartCoroutine(ShowResult());
        
    }

    IEnumerator ShowResult()
    {
        for (int i = 0; i < _chickCount; i++)
        {
            _chickens[i].SetActive(true);
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(1f);
        Debug.Log(11-_cageCount);
        for(int i = _chickCount; i < (11-_cageCount)*5; i++)
        {
            _chickens[i].SetActive(true);
            _chickens[i].GetComponent<Image>().color = new Color(1, 0, 0, 1);
            yield return new WaitForSeconds(0.2f);
        }
    }
}
