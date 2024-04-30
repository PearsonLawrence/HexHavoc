using UnityEngine;
using System.Collections;

public class backgroundMusic : MonoBehaviour
{
    public AudioSource mainMenuMusic;
    public AudioSource arenaMusic;
    public float fadeDuration = 2.0f;

    private void Start()
    {
        // Start playing main menu music
        mainMenuMusic.Play();
    }

    public void PlayArenaMusic()
    {
        // Fade out main menu music
        StartCoroutine(FadeOut(mainMenuMusic, fadeDuration));

        // Fade in arena music
        StartCoroutine(FadeIn(arenaMusic, fadeDuration));
    }

    public void ReturnToMainMenu()
    {
        // Stop arena music and fade it out
        StartCoroutine(FadeOut(arenaMusic, fadeDuration));

        // Fade in main menu music
        StartCoroutine(FadeIn(mainMenuMusic, fadeDuration));
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
