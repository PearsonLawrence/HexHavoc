using UnityEngine;
using System.Collections;

public class BackgroundMusic : MonoBehaviour
{
    public AudioClip mainMenuMusicClip;
    public AudioClip arenaMusicClip;
    public float fadeDuration = 2.0f;

    private AudioSource mainMenuMusicSource;
    private AudioSource arenaMusicSource;

    private void Start()
    {
        mainMenuMusicSource = gameObject.AddComponent<AudioSource>();
        mainMenuMusicSource.clip = mainMenuMusicClip;
        mainMenuMusicSource.loop = true;
        mainMenuMusicSource.playOnAwake = false;
        mainMenuMusicSource.Play();
        
        arenaMusicSource = gameObject.AddComponent<AudioSource>();
        arenaMusicSource.clip = arenaMusicClip;
        arenaMusicSource.loop = true;
        arenaMusicSource.playOnAwake = false;
    }

    public void PlayArenaMusic()
    {
        StartCoroutine(FadeOut(mainMenuMusicSource, fadeDuration));
        StartCoroutine(FadeIn(arenaMusicSource, fadeDuration));
    }

    public void ReturnToMainMenu()
    {
        StartCoroutine(FadeOut(arenaMusicSource, fadeDuration));
        StartCoroutine(FadeIn(mainMenuMusicSource, fadeDuration));
    }

    private IEnumerator FadeOut(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;
        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, (Time.time - startTime) / duration);
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();
    }

    private IEnumerator FadeIn(AudioSource audioSource, float duration)
    {
        audioSource.Play();
        audioSource.volume = 0f;

        while (audioSource.volume < 1f)
        {
            audioSource.volume += Time.deltaTime / duration;
            yield return null;
        }

        audioSource.volume = 1f;
    }
}
