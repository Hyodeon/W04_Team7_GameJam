using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneManager : BaseBehaviour
{
    public static GameSceneManager Instance;

    public PlayerBase Player;
    public TextMeshProUGUI ChickText;
    public Slider TimeSlider;
    public int CagesCount = 0;
    public float CurTime = 0;
    public float TargetTime = 90;

    protected override void Initialize()
    {
        base.Initialize();
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
    }

    public int GetCagesCount()
    {
        return FindObjectsOfType<Cage>().Length;
    }

    private void Update()
    {
        CurTime += Time.deltaTime;
        ChickText.text = "X " + Player.ChickCount.ToString();
        TimeSlider.value = CurTime / TargetTime;
        if (CurTime >= TargetTime)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        Ending.Instance.ShowEnding((int)EEndingList.Dinner);
    }




#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();

    }

#endif
}
