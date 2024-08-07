using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ending : MonoBehaviour
{
    [Header("SaveEnding Infomation")]
    [SerializeField]
    private List<Sprite> _endingImages = new List<Sprite>();
    [SerializeField]
    private List<Image> _endingList = new List<Image>();
    [SerializeField]
    private List<TextMeshProUGUI> _question = new List<TextMeshProUGUI>();

    [Header("Ending Infomation")]
    [SerializeField]
    private Image _gameEndingImg;


    public void SaveEnding(int num)
    {
        PlayerPrefs.SetInt(num.ToString(), 1);
    }
    public void ShowEnding(int num)
    {
        if (_gameEndingImg == null) return;
        _gameEndingImg.gameObject.SetActive(true);
        _gameEndingImg.sprite = _endingImages[num];
    }

    public void ShowEndingList()
    {
        for(int i =0 ; i < _endingList.Count; i++)
        {
            // non data
            if (!PlayerPrefs.HasKey(i.ToString()))
            {
                PlayerPrefs.SetInt(i.ToString(), 0);
                _question[i].text = "?";
                _endingList[i].sprite = null;
                _endingList[i].color = new Color(1, 1, 1, 0);
            }
            else
            {
                if(PlayerPrefs.GetInt(i.ToString()) == 1)
                {
                    _question[i].text = "";
                    _endingList[i].sprite = _endingImages[i];
                    _endingList[i].color = new Color(1, 1, 1, 1);
                }
            }
        }
    }

    private void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        foreach(TextMeshProUGUI question in _question)
        {
            question.text = "?";
        }
    }

    // Test Function
    public void DataResetButton()
    {
        PlayerPrefs.DeleteAll();
    }
}
