using System;
using System.IO;
using UnityEngine;

[System.Serializable]
public class ConfigData
{
    public bool musicMuted = false;
    public bool sfxMuted = false;
}

public class Config : MonoBehaviour
{
    public static Config Instance { get; private set; }
    public ConfigData data;

    private string configPath;

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        configPath = Path.Combine(Application.persistentDataPath, "config.json");
        LoadConfig();
    }

    private void LoadConfig()
    {
        if (File.Exists(configPath))
        {
            string json = File.ReadAllText(configPath);
            data = JsonUtility.FromJson<ConfigData>(json);
        }
        else
        {
            data = new ConfigData(); // valores por defecto
            SaveConfig();
        }
    }

    public void SaveConfig()
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(configPath, json);
    }

    public void ToggleMusicMute()
    {
        data.musicMuted = !data.musicMuted;
        SaveConfig();
    }
}
