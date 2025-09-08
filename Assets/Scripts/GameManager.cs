using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    ColorExpansion[] colorExpansions;
    public float completionPercentage = 0f;

    BlackTransitionEffect blackTransitionEffect;
    void Start()
    {
        blackTransitionEffect = GameObject.Find("BlackTransition").GetComponent<BlackTransitionEffect>();
        blackTransitionEffect.GoTransparent(() => { });

        colorExpansions = FindObjectsOfType<ColorExpansion>();
    }

    // Update is called once per frame
    void Update()
    {
        ManageProgress();
    }

    private void ManageProgress()
    {
        int coloredCount = 0;
        foreach (ColorExpansion colorExpansion in colorExpansions)
        {
            if (colorExpansion.actualColor != Color.white)
            {
                coloredCount++;
            }
        }
        completionPercentage = (float)coloredCount / (float)colorExpansions.Length;
    }

    public void RestartLevel()
    {
        blackTransitionEffect.GoBlack(() =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        });
    }
}
