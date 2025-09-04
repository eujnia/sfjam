using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour
{
    public AudioClip[] songs;   // lista de canciones desde el inspector
    private AudioSource audioSource;

    [SerializeField] float maxVolume = 0.2f;
    [SerializeField] float fadeDuration = 1f;

    void Start()
    {
        GameObject existingMusic = GameObject.Find("Music");
        if (existingMusic != null && existingMusic != gameObject)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.loop = true; // default
        audioSource.volume = 0;

        PlaySong("hike");
    }

    public void PlaySong(string songName)
    {
        AudioClip clip = System.Array.Find(songs, song => song.name == songName);
        if (clip == null) return;

        StopAllCoroutines();
        StartCoroutine(FadeToSong(clip));
    }

    private IEnumerator FadeToSong(AudioClip newClip)
    {
        // Fade out
        float startVolume = audioSource.volume;
        float time = 0f;

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
            audioSource.volume = Mathf.Lerp(0f, maxVolume, time / fadeDuration);
            yield return null;
        }

        audioSource.volume = maxVolume;
    }

    public void Stop()
    {
        StopAllCoroutines();
        audioSource.Stop();
    }
}
