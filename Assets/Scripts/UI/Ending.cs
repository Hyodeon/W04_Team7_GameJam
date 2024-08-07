using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ending : MonoBehaviour
{
    [Header("Ending Infomation")]
    [SerializeField]
    private List<Sprite> _endingImages = new List<Sprite>();
    [SerializeField]
    private List<Image> _endingList = new List<Image>();

    public void SaveEnding(int num)
    {
        PlayerPrefs.SetInt(num.ToString(), 1);
    }

    public void ShowEnding(Image _gameEndingImg,  int num)
    {
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
            }
            else
            {
                if(PlayerPrefs.GetInt(i.ToString()) == 1)
                {
                    _endingList[i].sprite = _endingImages[i];
                }
            }
        }
    }
}
