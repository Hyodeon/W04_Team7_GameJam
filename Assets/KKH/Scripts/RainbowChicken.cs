using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowChicken : BaseBehaviour
{
    [SerializeField] private Material _mat;
    [SerializeField] private MeshRenderer[] _meshRenderers;
    [SerializeField] private float _changeTime;

    private void Start()
    {
        StartCoroutine(CoColorChange());
    }
    private void Update()
    {

    }

    private IEnumerator CoColorChange()
    {
        Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        _mat.color = color;
        yield return new WaitForSeconds(_changeTime);
        StartCoroutine(CoColorChange());
    }



#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _meshRenderers = GetComponentsInChildren<MeshRenderer>();
    }
#endif
}

