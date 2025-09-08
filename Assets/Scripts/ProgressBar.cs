using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    Image image;
    GameManager gameManager;
    void Start()
    {
        image = GetComponent<Image>();
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        image.fillAmount = Mathf.Lerp(image.fillAmount, gameManager.completionPercentage, Time.deltaTime * 5f);
    }
}
