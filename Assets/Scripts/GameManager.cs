using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    ColorExpansion[] colorExpansions;
    BlackTransitionEffect blackTransitionEffect;
    MouseLook mouseLook;
    public float completionPercentage = 0f;
    public int level = 1;

    PlayerMovement playerMovement;
    MoveAlongSpline moveAlongSpline;

    void Start()
    {
        blackTransitionEffect = GameObject.Find("BlackTransition").GetComponent<BlackTransitionEffect>();
        DeactivateAll();
        blackTransitionEffect.GoTransparent(() => { });

        mouseLook = GameObject.Find("MouseLook").GetComponent<MouseLook>();

        colorExpansions = FindObjectsOfType<ColorExpansion>();
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        moveAlongSpline = GameObject.Find("Player").GetComponent<MoveAlongSpline>();
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

    public void DoWhatGoesNext()
    {
        if (level == 1 || level == 3 || level == 5 || level == 9)
        {
            if (level == 9)
            {
                Music.Instance.forceMute = false;
                Music.Instance.PlaySong("mocchi");
            }
            blackTransitionEffect.GoBlack(() =>
            {
                AdvanceLevel();
            });
        }
        else if (level == 7)
        {
            playerMovement.maxPlayerSpeed = 0f;
            GameObject.Find("QueTrajiste").transform.GetChild(0).gameObject.SetActive(true);
            mouseLook.SetCursorLocked(false);
            level++;
        }
        else if (level == 8)
        {
            playerMovement.maxPlayerSpeed = 2f;
            GameObject.Find("QueTrajiste").transform.GetChild(0).gameObject.SetActive(false);
            GameObject.Find("Bridge").transform.GetChild(0).gameObject.SetActive(true);
            mouseLook.SetCursorLocked(true);
            moveAlongSpline.percentageRange = new Vector2(moveAlongSpline.percentageRange.x, 0.98f);
            level++;
        }
    }

    public void AdvanceLevel()
    {
        level++;
        if (level == 2 || level == 4 || level == 6 || level == 10)
        {
            GameObject.Find("ImageOptions").GetComponent<ImageOptions>().UpdateSprites();
            mouseLook.SetCursorLocked(false);
            return;
        }
        else if (level == 3)
        {
            Transform trees = GameObject.Find("Trees").transform;
            foreach (Transform child in trees)
            {
                child.gameObject.SetActive(true);
            }
            Transform Grass1 = GameObject.Find("Grass1").transform;
            foreach (Transform child in Grass1)
            {
                child.gameObject.SetActive(true);
            }

            playerMovement.maxPlayerSpeed = 4f;
            moveAlongSpline.distancePercentage = 0.2f;

            ResetColors(0.1f);
            mouseLook.SetCursorLocked(true);
        }
        else if (level == 5)
        {
            Transform Grass2 = GameObject.Find("Grass2").transform;
            foreach (Transform child in Grass2)
            {
                child.gameObject.SetActive(true);
            }
            Transform Picnic = GameObject.Find("Picnic").transform;
            foreach (Transform child in Picnic)
            {
                child.gameObject.SetActive(true);
            }
            Transform Stores = GameObject.Find("Stores").transform;
            foreach (Transform child in Stores)
            {
                child.gameObject.SetActive(true);
            }
            Transform City = GameObject.Find("City").transform;
            foreach (Transform child in City)
            {
                child.gameObject.SetActive(true);
            }

            moveAlongSpline.distancePercentage = 0.2f;

            ResetColors(0.3f);
            mouseLook.SetCursorLocked(true);
        }
        else if (level == 7)
        {
            moveAlongSpline.distancePercentage = 0.2f;
            Music.Instance.forceMute = true;
            GameObject.Find("Bridge").transform.GetChild(0).gameObject.SetActive(false);
            GameObject.Find("RoadSpline").GetComponent<MeshRenderer>().enabled = false;
            moveAlongSpline.percentageRange = new Vector2(moveAlongSpline.percentageRange.x, 0.83f);

            ResetColors(1f, true);


            mouseLook.SetCursorLocked(true);
        }
        else if (level == 11)
        {
            moveAlongSpline.distancePercentage = 0.2f;
            playerMovement.maxPlayerSpeed = 4f;
            GameObject.Find("RoadSpline").GetComponent<MeshRenderer>().enabled = true;
            GameObject.Find("House1").SetActive(false);

            ResetColors(1f);
            mouseLook.SetCursorLocked(true);
        }
    }

    private void ResetColors(float v, bool grayScale = false)
    {
        foreach (ColorExpansion colorExpansion in colorExpansions)
        {
            float rnd = UnityEngine.Random.Range(0f, 1f);
            if (rnd > v || colorExpansion.gameObject.name == "Disco") continue;

            if (grayScale)
            {
                float val = UnityEngine.Random.value;
                Color col = new Color(val, val, val);
                colorExpansion.StartEffect(
                    colorExpansion.transform.position,
                    col
                );
            }
            else
            {
                Color col = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
                colorExpansion.StartEffect(
                    colorExpansion.transform.position,
                    col
                );

            }
        }
        colorExpansions = FindObjectsOfType<ColorExpansion>();
    }

    private void DeactivateAll()
    {
        Transform trees = GameObject.Find("Trees").transform;
        foreach (Transform child in trees)
        {
            child.gameObject.SetActive(false);
        }
        Transform Grass1 = GameObject.Find("Grass1").transform;
        foreach (Transform child in Grass1)
        {
            child.gameObject.SetActive(false);
        }

        Transform Grass2 = GameObject.Find("Grass2").transform;
        foreach (Transform child in Grass2)
        {
            child.gameObject.SetActive(false);
        }
        Transform Picnic = GameObject.Find("Picnic").transform;
        foreach (Transform child in Picnic)
        {
            child.gameObject.SetActive(false);
        }
        Transform Stores = GameObject.Find("Stores").transform;
        foreach (Transform child in Stores)
        {
            child.gameObject.SetActive(false);
        }
        Transform City = GameObject.Find("City").transform;
        foreach (Transform child in City)
        {
            child.gameObject.SetActive(false);
        }
    }
}
