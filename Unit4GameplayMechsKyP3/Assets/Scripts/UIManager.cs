using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject pauseScreen;
    public GameObject gameOverScreen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1.0f;
    }

    public  void Unpause()
    {
        Time.timeScale = 1;
        pauseScreen.SetActive(false);
    }

    public void Pause ()
    {
        Time.timeScale = 0.01f;
        pauseScreen.SetActive(true);
    }

    public void GameOver()
    {
        GameObject.Find("SpawnManager").gameObject.SetActive(false);
        gameOverScreen.gameObject.SetActive(true);
    }
}
