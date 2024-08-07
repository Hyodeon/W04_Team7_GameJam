using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ending : MonoBehaviour
{
    public static Ending Instance;

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


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;


        Initialize();
    }


    public void SaveEnding(int num)
    {
        PlayerPrefs.SetInt(num.ToString(), 1);
    }
    public void ShowEnding(int num)
    {
        if (_gameEndingImg == null) return;
        _gameEndingImg.sprite = _endingImages[num];
        _gameEndingImg.gameObject.SetActive(true);
        SaveEnding(num);
        Time.timeScale = 0;
    }

    public void ShowEndingList()
    {
        for (int i = 0; i < _endingList.Count; i++)
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
                if (PlayerPrefs.GetInt(i.ToString()) == 1)
                {
                    _question[i].text = "";
                    _endingList[i].sprite = _endingImages[i];
                    _endingList[i].color = new Color(1, 1, 1, 1);
                }
            }
        }
    }


    void Initialize()
    {
        foreach (TextMeshProUGUI question in _question)
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

public enum EEndingList
{

    Psycho = 0,
    Sad = 1,
    Dinner = 2,
    Death = 3,
    Mass = 4,
    Normal = 5


}