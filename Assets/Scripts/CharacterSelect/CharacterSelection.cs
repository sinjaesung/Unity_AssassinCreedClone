using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    public GameObject[] characters;
    public int selectedCharacter = 0;

    [SerializeField] private string loadSceneName;

    public static CharacterSelection instance;
    public bool startGame;

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
        StartGame();
    }

    public void OnQuitButton()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }

    public void NextCharacter()
    {
        characters[selectedCharacter].SetActive(false);
        selectedCharacter = (selectedCharacter + 1) % characters.Length;
        characters[selectedCharacter].SetActive(true);
    }

    public void PreviousCharacter()
    {
        characters[selectedCharacter].SetActive(false);
        selectedCharacter--;
        if(selectedCharacter < 0)
        {
            selectedCharacter += characters.Length;
        }
        characters[selectedCharacter].SetActive(true);
    }

    public void StartGame()
    {
        startGame = true;

        PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
        //SceneManager.LoadScene(loadSceneName, LoadSceneMode.Single);
        SceneManager.LoadScene(loadSceneName);
    }
}
