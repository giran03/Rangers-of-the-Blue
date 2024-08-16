using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class Audio
{
    public string clipName;
    public AudioClip audioClip;
}

public class AudioManager : MonoBehaviour
{
    [Header("Configs")]
    [SerializeField] Audio[] musicSounds, sfxSounds;
    [SerializeField] AudioSource musicSource, sfxSource;

    public static AudioManager Instance;
    AudioSource currentSfxLooping;
    AudioSource currentMusic;
    bool playedMusic;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
        {
            DontDestroyOnLoad(this);
            Instance = this;
        }
    }

    private void Update()
    {
        if(GameSceneManager.Instance.CurrentScene() == "_Menu")
        {
            currentMusic.volume = .5f;
        }
        else
        {
            currentMusic.volume = .5f;
        }
    }

    private void Start() => PlayMusic("Menu BGM");

    public void Button_GameBGM()
    {
        Debug.Log($"Credits Screen");
        PlayMusic("Game BGM");
    }

    public void Button_MenuBGM()
    {
        Debug.Log($"Main Menu");
        PlayMusic("Menu BGM");
    }

    public void PlayMusic(string clipName)
    {
        Debug.Log($"Called play music");
        Audio sound = Array.Find(musicSounds, x => x.clipName == clipName);
        if (sound != null)
        {
            musicSource.clip = sound.audioClip;
            musicSource.loop = true;
            musicSource.Play();
            currentMusic = musicSource;
        }
    }

    public void PlaySFX(string clipName, bool loop = false)
    {
        Audio audio = Array.Find(sfxSounds, x => x.clipName == clipName);
        if (audio != null)
        {
            sfxSource.loop = loop;

            Debug.Log($"Playing SFX: {audio.clipName}");

            if (loop)
            {
                if (currentSfxLooping != null)
                    currentSfxLooping.Stop();

                sfxSource.clip = audio.audioClip;
                sfxSource.Play();
                currentSfxLooping = sfxSource;
            }
            else
            {
                sfxSource.clip = audio.audioClip;
                // sfxSource.volume = 1f;
                AudioSource.PlayClipAtPoint(sfxSource.clip, Camera.main.transform.position);
                StartCoroutine(Cooldown(audio.audioClip));
            }
        }
    }

    public void PlayDialogue(AudioClip[] dialogueArray, int currentIndex)
    {
        Debug.Log($"Playing Dialogue of {dialogueArray[currentIndex]}");

        sfxSource.clip = dialogueArray[currentIndex];
        sfxSource.Play();
        // AudioSource.PlayClipAtPoint(sfxSource.clip, Camera.main.transform.position);
        // StartCoroutine(Cooldown(dialogueArray[currentIndex]));
    }

    public void StopMusic()
    {
        if (currentMusic != null)
        {
            currentMusic.Stop();
            currentMusic = null;
        }
    }

    public void StopSFX()
    {
        sfxSource.Stop();
    }

    public void StopSFX_Loop()
    {
        if (currentSfxLooping != null)
        {
            currentSfxLooping.Stop();
            currentSfxLooping = null;
        }
    }

    IEnumerator Cooldown(AudioClip audioClip)
    {
        yield return new WaitForSeconds(audioClip.length);
        sfxSource.clip = null;
    }
}
