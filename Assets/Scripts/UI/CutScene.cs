using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CutScene : MonoBehaviour
{
    [Header("CutSceneImage")]
    [SerializeField]
    private List<Sprite> _startImages = new List<Sprite>();

    [Header("showUI")]
    [SerializeField]
    private Image showImage;

    [Header("SceneLoader")]
    [SerializeField]
    private SceneLoader loader;

    [Header("GameSceneName")]
    [SerializeField]
    private string _gameSceneName;

    private int _showImageNum;

    void NextImage()
    {
        if(_startImages.Count > _showImageNum)
        {
            showImage.sprite = _startImages[_showImageNum];
            _showImageNum++;
        }
        else
        {
            loader.SceneLoad(_gameSceneName);
        }
    }

    IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(0.1f);
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(FadeIn());
    }

    private void Update()
    {
        if(Input.anyKeyDown)
        {
            NextImage();
        }
    }

    private void Start()
    {
        _showImageNum = 1;
        StartCoroutine(FadeOut());
    }
}
