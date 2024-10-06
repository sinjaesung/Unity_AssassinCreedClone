using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public bool startGame;

    public static MainMenu instance;

    #region singleton
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);//다른씬으로 갈때 현재 오브젝트 삭제하지않는다
        }

        else
            Destroy(gameObject);//다른 씬 갔다가 왔을때 awake또 실행되는데, 이때 awake로 새로 만들어지는데 새로만들어지는것만 삭제
    }
    #endregion

    public void OnStartButton()
    {
        Debug.Log("Starting");
        startGame = true;
        SceneManager.LoadScene("MainScene");
    }

    public void OnQuitButton()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }
}
