using UnityEngine;




[RequireComponent(typeof(GameSceneEvents))]
public class GameSceneManager : BaseBehaviour
{
    public static GameSceneManager Instance;
    public GameSceneEvents EventGameScene;


    protected override void Initialize()
    {
        base.Initialize();
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
    }

    private void OnEnable()
    {
        EventGameScene.OnGameOver += Event_GameOver;
    }

    private void OnDisable()
    {
        EventGameScene.OnGameOver -= Event_GameOver;
    }



    private void Event_GameOver(GameSceneEvents gameSceneEvents, GameOverEventArgs gameOverEventArgs)
    {

    }








#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        EventGameScene = GetComponent<GameSceneEvents>();

    }

#endif
}
