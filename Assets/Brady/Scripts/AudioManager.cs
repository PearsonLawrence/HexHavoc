using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    //audio clip ref
    [SerializeField] private AudioClip backgroundMenuMusic;
    [SerializeField] private AudioClip backgroundMatchMusic;
    [SerializeField] private AudioClip fireballSound;
    [SerializeField] private AudioClip windBlastSound;
    [SerializeField] private AudioClip waterShotSound;
    [SerializeField] private AudioClip earthSpearSound;
    [SerializeField] private AudioClip fireWallSound;
    [SerializeField] private AudioClip windWallSound;
    [SerializeField] private AudioClip waterWallSound;
    [SerializeField] private AudioClip earthWallSound;

    private AudioSource backgroundMusicSource;
    private AudioSource soundEffectSource;

    private void Awake()
    {
        //make sure its the only audio manager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //create audio sources
        backgroundMusicSource = gameObject.AddComponent<AudioSource>();
        soundEffectSource = gameObject.AddComponent<AudioSource>();

        //play background
        PlayBackgroundMenuMusic();
    }

    //Loop background
    public void PlayBackgroundMenuMusic()
    {
        backgroundMusicSource.clip = backgroundMenuMusic;
        backgroundMusicSource.loop = true; // Make background music loop
        backgroundMusicSource.Play();
    }

    //when match starts play  match background
    public void PlayBackgroundMatchMusic()
    {
        backgroundMusicSource.clip = backgroundMatchMusic;
        backgroundMusicSource.loop = true; // Make background music loop
        backgroundMusicSource.Play();
    }

    //play audio
    public void PlaySound(AudioClip clip)
    {
        soundEffectSource.PlayOneShot(clip);
    }

    //fire attack
    public void PlayFireballSound()
    {
        PlaySound(fireballSound);
    }

    //wind attack
    public void PlayWindBlastSound()
    {
        PlaySound(windBlastSound);
    }

    //water attack
    public void PlayWaterShotSound()
    {
        PlaySound(waterShotSound);
    }

    //earth attack
    public void PlayEarthSpearSound()
    {
        PlaySound(earthSpearSound);
    }

    //fire defense
    public void PlayFireWallSound()
    {
        PlaySound(fireWallSound);
    }

    //wind defense
    public void PlayWindWallSound()
    {
        PlaySound(windWallSound);
    }

    //water defense
    public void PlayWaterWallSound()
    {
        PlaySound(waterWallSound);
    }

    //earth defense
    public void PlayEarthWallSound()
    {
        PlaySound(earthWallSound);
    }
}
