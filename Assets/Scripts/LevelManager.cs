using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] float sceneLoadDelay = 2f;
    ScoreKeeper scoreKeeper;

    void Awake()
    {
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
    }

    public void LoadGame()
    {
        scoreKeeper?.ResetScore();
        SceneManager.LoadScene(1);
    }

    public void LoadMainMenue()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadGameOver()
    {
        StartCoroutine(WaitAndLoad(2, sceneLoadDelay));
    }

    public void QuitGame()
    {
        Debug.Log("Quitting the game");
        Application.Quit();
    }

    IEnumerator WaitAndLoad(int sceneIndex, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneIndex);
    }
}
