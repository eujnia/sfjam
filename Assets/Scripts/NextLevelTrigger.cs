using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelTrigger : MonoBehaviour
{
    GameManager gameManager;
    public int type;
    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (type == 0)
            {
                if (gameManager.level == 1 || gameManager.level == 3 || gameManager.level == 5 || gameManager.level == 9) gameManager.DoWhatGoesNext();
            }
            else
            {
                if (gameManager.level == 7) gameManager.DoWhatGoesNext();
            }
        }
    }
}
