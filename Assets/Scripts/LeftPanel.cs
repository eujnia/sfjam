using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftPanel : MonoBehaviour
{
    float timer = 0f;
    Vector3 initialPosition;
    GameManager gameManager;
    void Start()
    {
        initialPosition = transform.localPosition;
        transform.Find("GoToMenuButton").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(GoToMenu);
        transform.Find("GoToMenuButton").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => timer = 3f);
        transform.Find("MusicToggle").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => timer = 0f);
        transform.Find("SensitivityToggle").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => timer = 3f);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.level == 2 || gameManager.level == 4 || gameManager.level == 6 || gameManager.level == 10)
        {
            return;
        }
        timer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            timer = 3f;
        }

        if (timer > 0f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition + new Vector3(100f, 0f, 0f), Time.deltaTime * 10f);
            GameObject.Find("MouseLook").GetComponent<MouseLook>().SetCursorLocked(false);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition, Time.deltaTime * 10f);
            GameObject.Find("MouseLook").GetComponent<MouseLook>().SetCursorLocked(true);
        }
    }


    public void GoToMenu()
    {
        Music.Instance.PlaySong("hike");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }

    internal void ClosePanel()
    {
        timer = 0f;
    }
}
