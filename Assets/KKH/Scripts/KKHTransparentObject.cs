using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KKHTransparentObject : MonoBehaviour
{
    private Material _material;
    [SerializeField] private float _alpha = 0.5f;
    private bool _isFaded = false;
    private float _returnTime = 0.4f;
    private float _timer = 0f;
    private void Start()
    {
        _material = GetComponentInChildren<MeshRenderer>().material;
    }

    private void Update()
    {
        if (_isFaded)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                ReturnToDefault();
            }
        }
    }

    public void SetTransparent()
    {
        if (_isFaded)
        {
            _timer = _returnTime;
            return;
        }
            
        _isFaded = true;    
        Color color = _material.color;
        _material.color = new Color(color.r, color.g, color.b, _alpha);
    }

    private void ReturnToDefault()
    {
        _isFaded = false;
        _timer = _returnTime;
        Color color = _material.color;
        _material.color = new Color(color.r, color.g, color.b, 1);
    }
}
