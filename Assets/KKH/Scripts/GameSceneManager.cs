using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public int _chickCount;
    [SerializeField] private TextMeshProUGUI _cageCountText;
    [SerializeField] private GameObject _cutScene;
    protected override void Initialize()
    {
        base.Initialize();
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
    }

    private void Start()
    {
        CagesCount = GetCagesCount();
    }
    public int GetCagesCount()
    {
        return FindObjectsOfType<Cage>().Length;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _cutScene == null)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        CurTime += Time.deltaTime;
        ChickText.text = "X " + Player.ChickCount.ToString();
        TimeSlider.value = CurTime / TargetTime;
        _cageCountText.text = "X " + CagesCount.ToString();
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
