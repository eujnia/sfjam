using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().RestartLevel();
        }
    }
}
