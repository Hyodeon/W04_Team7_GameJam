using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneImages : BaseBehaviour
{
    [SerializeField] private GameObject go1;
    [SerializeField] private GameObject go2;
    private int _index;
    protected override void Awake()
    {
        base.Awake();
        Time.timeScale = 0;
        _index = 0;
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            if (_index == 0)
            {
                go1.SetActive(false);
                _index++;
            }
            else if (_index == 1)
            {
                go2.SetActive(false);
                Time.timeScale = 1;
                Destroy(this.gameObject);
            }
        }
    }
}
