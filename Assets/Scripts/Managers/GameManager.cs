using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private GameObject _player;
    
    private float _currentTime;
    [SerializeField] private float _deadTime;

    public TextMeshProUGUI ChickCountText;
    public Slider TimeSlider;

    private int _cageCount;
    private int _chickCount;

    private void Awake()
    {
        _currentTime = 0f;
        _deadTime = 120f;

        _cageCount = 0;
        _chickCount = 0;

        _cageCount = FindObjectsOfType<Cage>().Length;

        _player = GameObject.FindGameObjectWithTag("Player");

        if (ChickCountText == null) throw new NullReferenceException();
        if (TimeSlider == null) throw new NullReferenceException();
    }

    private void RefreshCurrentState()
    {
        _currentTime += Time.deltaTime;

        _chickCount = _player.GetComponent<PlayerBase>().ChickCount;

        _cageCount = FindObjectsOfType<Cage>().Length;

        CheckCage();
        CheckTime();
        UpdateUI();

    }

    private void CheckCage()
    {

    }

    private void CheckTime()
    {

    }

    private void UpdateUI()
    {
        float ratio = _currentTime / _deadTime;

        TimeSlider.value = ratio;
    }

    private void LateUpdate()
    {
        RefreshCurrentState();
    }
}
