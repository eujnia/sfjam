using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageOptions : MonoBehaviour
{
    UnityEngine.UI.Image op1;
    UnityEngine.UI.Image op2;
    UnityEngine.UI.Image op3;
    GameManager gameManager;
    Subtitles subtitles;
    void Start()
    {
        op1 = transform.Find("Op1").GetComponent<UnityEngine.UI.Image>();
        op2 = transform.Find("Op2").GetComponent<UnityEngine.UI.Image>();
        op3 = transform.Find("Op3").GetComponent<UnityEngine.UI.Image>();

        op1.gameObject.SetActive(false);
        op2.gameObject.SetActive(false);
        op3.gameObject.SetActive(false);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        subtitles = GameObject.Find("Subtitles").GetComponent<Subtitles>();

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void UpdateSprites()
    {

        if (gameManager.level == 2)
        {
            op1.gameObject.SetActive(true);
            op2.gameObject.SetActive(true);
            op3.gameObject.SetActive(true);
            op1.sprite = Resources.Load<Sprite>("Textures/vinoBlanco");
            op2.sprite = Resources.Load<Sprite>("Textures/toro");
            op3.sprite = Resources.Load<Sprite>("Textures/whisky");
        }
        else if (gameManager.level == 4)
        {
            op1.gameObject.SetActive(true);
            op2.gameObject.SetActive(true);
            op3.gameObject.SetActive(true);
            op1.sprite = Resources.Load<Sprite>("Textures/milanesa");
            op2.sprite = Resources.Load<Sprite>("Textures/ramen");
            op3.sprite = Resources.Load<Sprite>("Textures/bife");
        }
        else if (gameManager.level == 6)
        {
            op1.gameObject.SetActive(true);
            op2.gameObject.SetActive(true);
            op3.gameObject.SetActive(true);
            op1.sprite = Resources.Load<Sprite>("Textures/porro");
            op2.sprite = Resources.Load<Sprite>("Textures/puchos");
            op3.sprite = Resources.Load<Sprite>("Textures/parlante");
        }
        else if (gameManager.level == 10)
        {
            op1.gameObject.SetActive(false);
            op2.gameObject.SetActive(true);
            op3.gameObject.SetActive(false);

            float a = Mathf.Max(
                 Subtitles.Similarity(Config.Instance.data.otherName.ToLower(), "juan"),
                    Subtitles.Similarity(Config.Instance.data.otherName.ToLower(), "euge")
            );
            float b = Mathf.Max(
                 Subtitles.Similarity(Config.Instance.data.myName.ToLower(), "juan"),
                    Subtitles.Similarity(Config.Instance.data.myName.ToLower(), "euge")
            );
            if (a > 0f && b > 0f)
            {
                op2.sprite = Resources.Load<Sprite>("Textures/eujejuan");
            }
            else
            {
                print("No son ellos");
                op2.sprite = Resources.Load<Sprite>("Textures/eujejuan");
            }
        }
        else
        {
            op1.gameObject.SetActive(false);
            op2.gameObject.SetActive(false);
            op3.gameObject.SetActive(false);
        }
    }
}
