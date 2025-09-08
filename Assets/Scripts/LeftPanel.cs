using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftPanel : MonoBehaviour
{
    float timer = 0f;
    Vector3 initialPosition;
    void Start()
    {
        initialPosition = transform.localPosition;
        transform.Find("GoToMenuButton").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(GoToMenu);
        transform.Find("GoToMenuButton").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => timer = 5f);
        transform.Find("MusicToggle").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => timer = 0f);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            timer = 5f;
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
