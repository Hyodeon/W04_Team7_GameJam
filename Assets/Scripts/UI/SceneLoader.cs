using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private string _loadSceneName;

    public void SceneLoad(string _sceneName)
    {
        SceneManager.LoadScene(_sceneName);
    }
}
