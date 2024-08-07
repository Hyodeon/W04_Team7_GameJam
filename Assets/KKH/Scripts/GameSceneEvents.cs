using System;
using UnityEngine;

public class GameSceneEvents : MonoBehaviour
{
    public Action<GameSceneEvents, GameOverEventArgs> OnGameOver;



    public void CallGameOver(GameEndType endType)
    {
        OnGameOver?.Invoke(this, new GameOverEventArgs() { endType = endType });
    }

}



public class GameOverEventArgs : EventArgs
{
    public GameEndType endType;
}



public enum GameEndType
{


}