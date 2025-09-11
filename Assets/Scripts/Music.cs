using UnityEngine;
using System.Collections;

[System.Serializable]
public struct DiskData
{
    public Texture texture;
    public string songName;
    public int[] colorIDs;
}

public class Music : MonoBehaviour
{
    public static Music Instance { get; private set; }
    AudioClip[] songs;   // lista de canciones desde el inspector
    private AudioSource audioSource;

    public float maxVolume = 0.2f;
    [SerializeField] float fadeDuration = 1f;
    public DiskData[] disksData;
    bool fading = false;
    internal bool forceMute;

    void Start()
    {
        songs = Resources.LoadAll<AudioClip>("Music");
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.loop = true; // default
        audioSource.volume = 0;

        PlaySong("hike", false);
    }

    private void Update()
    {
        if (!fading) audioSource.volume = Mathf.Lerp(audioSource.volume, maxVolume * (Config.Instance.data.musicMuted ? 0f : 1f) * (forceMute ? 0f : 1f), Time.deltaTime * 5f);
    }

    public void PlaySong(string songName, bool fade = true)
    {
        AudioClip clip = System.Array.Find(songs, song => song.name == songName);
        if (clip == null || (audioSource.clip == clip && audioSource.isPlaying)) return;

        StopAllCoroutines();
        StartCoroutine(FadeToSong(clip, fade));
    }

    private IEnumerator FadeToSong(AudioClip newClip, bool fade)
    {
        if (!fade)
        {
            audioSource.clip = newClip;
            audioSource.volume = maxVolume * (Config.Instance.data.musicMuted ? 0f : 1f) * (forceMute ? 0f : 1f);
            audioSource.Play();
            yield break;
        }

        float startVolume = audioSource.volume;
        float time = 0f;
        fading = true;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, time / fadeDuration);
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.clip = newClip;
        audioSource.Play();

        // Fade in
        time = 0f;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, maxVolume * (Config.Instance.data.musicMuted ? 0f : 1f) * (forceMute ? 0f : 1f), time / fadeDuration);
            yield return null;
        }

        audioSource.volume = maxVolume * (Config.Instance.data.musicMuted ? 0f : 1f) * (forceMute ? 0f : 1f);
        fading = false;
    }

    public void Stop()
    {
        StopAllCoroutines();
        audioSource.Stop();
    }
}
