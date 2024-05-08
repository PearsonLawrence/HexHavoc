 //Written by Braden Turner
 //creates functions to call that will play audio, would probally interact with match manager
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

    [SerializeField] private AudioClip AmatchStartSound;

    [SerializeField] private AudioClip AelmChooseSound;

    [SerializeField] private AudioClip Around1;

    [SerializeField] private AudioClip Around2;

    [SerializeField] private AudioClip Around3;

    [SerializeField] private AudioClip AfinishSound; 

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

    public void PlayAmatchStartsound()
    {
        PlaySound(AmatchStartSound);
    }

    public void PlayAelmChoose()
    {
        PlaySound(AelmChooseSound);
    }

    public void PlayAround1()
    {
        PlaySound(Around1);
    }

    public void PlayAround2()
    {
        PlaySound(Around2);
    }

    public void PlayAround3()
    {
        PlaySound(Around3);
    }

    public void PlayAfinish()
    {
        PlaySound(AfinishSound);
    }

}
