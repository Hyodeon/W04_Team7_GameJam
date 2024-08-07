using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cage : Interactable
{
    [Header("Chick")]
    [SerializeField] private GameObject _chick;
    [SerializeField] private int _maxChickenCount;
    [SerializeField] private int _minChickenCount;
    private int _count;


    protected override void Initialize()
    {
        base.Initialize();
        _count = Random.Range(_minChickenCount, _maxChickenCount+1);
    }


    protected override void DestroyInteractable()
    {
        for (int i = 0; i < _count; i++)
        {
            Instantiate(_chick, transform.position + _offSet, Quaternion.identity);
        }
        GameObject go = Instantiate(_destroyedParticle, transform.position + _offSet, Quaternion.identity);
        go.transform.localScale = transform.localScale * _scaleRatio;
        Destroy(this.gameObject);
    }



}
