using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackTransitionEffect : MonoBehaviour
{
    UnityEngine.UI.Image image;
    public float duration = 3f;
    float timer = 0f;
    bool goToBlack = false;
    bool goToTransparent = false;
    Action onComplete = null;

    void Awake()
    {
        image = GetComponent<UnityEngine.UI.Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(goToBlack)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);
            image.color = new Color(0f, 0f, 0f, t);
            if (t >= 1f)
            {
                goToBlack = false;
                onComplete?.Invoke();
            }
        }
        else if (goToTransparent)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);
            image.color = new Color(0f, 0f, 0f, 1f - t);
            if (t >= 1f)
            {
                goToTransparent = false;
                onComplete?.Invoke();
            }
        }
    }

    internal void GoBlack(Action value)
    {
        timer = 0f;
        goToBlack = true;
        goToTransparent = false;
        onComplete = value;
    }

    internal void GoTransparent(Action value)
    {
        timer = 0f;
        goToBlack = false;
        goToTransparent = true;
        onComplete = value;
    }
}
