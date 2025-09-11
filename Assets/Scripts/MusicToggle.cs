using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicToggle : MonoBehaviour
{
    UnityEngine.UI.Image image;
    void Start()
    {
        image = GetComponent<UnityEngine.UI.Image>();
        if (!image) image = transform.Find("Image").GetComponent<UnityEngine.UI.Image>();
        UpdateSprite();
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            Config.Instance.ToggleMusicMute();
            UpdateSprite();
        });
    }
    
    private void UpdateSprite()
    {
        string spriteName = Config.Instance.data.musicMuted ? "Textures/musicOff" : "Textures/musicOn";
        image.sprite = Resources.Load<Sprite>(spriteName);
    }
}
