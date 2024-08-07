using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [Header("Time")]
    [SerializeField]
    private float _timeLimit;

    [Header("EndingUI")]
    [SerializeField]
    private Ending _endingUI;


    [Header("TimeSlider")]
    [SerializeField]
    private Slider _timeSlider;


    [Header("chicks Count")]
    [SerializeField]
    private TextMeshProUGUI _chicksCount;

    private void Awake()
    {
        
    }

    private void Update()
    {
        TimePasses();
        _timeSlider.value = Time.time / _timeLimit;
    }

    void TimePasses()
    {
        // Time.time is SceneLoad init 0 ?
        if (_timeLimit < Time.time)
        {
            // GameOver : TimeOver
            _endingUI.SaveEnding(0);
            _endingUI.ShowEnding(0);
        }
    }

    public void ChangeChicksCount(int count)
    {
        _chicksCount.text = count.ToString();
    }
}
