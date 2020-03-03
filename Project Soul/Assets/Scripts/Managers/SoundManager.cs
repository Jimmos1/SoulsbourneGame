using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Requires GameManager initialization.
//Requires GameAssets in the scene.
public static class SoundManager 
{
    public enum Sound
    {
        SwordSheath,
        PlayerMove,
        PlayerAttack,
        PlayerHit,
        EnemyHit,
        EnemyDie,
        Treasure,
        ButtonOver,
        ButtonClick
    }

    private static Dictionary<Sound, float> soundTimerDictionary;
    private static GameObject oneShotGameObject;
    private static AudioSource oneShotAudioSource;

    public static void Initialize()
    {
        soundTimerDictionary = new Dictionary<Sound, float>();
        soundTimerDictionary[Sound.PlayerMove] = 0f;
        soundTimerDictionary[Sound.EnemyHit] = 0f;
        //TODO: Add rest of sound
    }

    //Positional sound method. Call with e.g. SoundManager.PlaySound(SoundManager.Sound.PlayerAttack, GetPosition());
    public static void PlaySound(Sound sound, Vector3 position)
    {
        if (CanPlaySound(sound))
        {
            if (CanPlaySound(sound))
            {
                GameObject soundGameObject = new GameObject("Sound");
                soundGameObject.transform.position = position;
                AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
                audioSource.clip = GetAudioClip(sound);

                //TODO: Set values for each sound case.
                //See ApplyAudioSourceSettings function for more info.
                //audioSource = ApplyAudioSourceSettings(sound, audioSource);
                audioSource.maxDistance = 100f;
                audioSource.spatialBlend = 1f;
                audioSource.rolloffMode = AudioRolloffMode.Linear;
                audioSource.dopplerLevel = 0f;
                audioSource.Play();

                Object.Destroy(soundGameObject, audioSource.clip.length); //TODO: Object pooling in optimization phase.
            }
        }
    }
    //Call with e.g. SoundManager.PlaySound(SoundManager.Sound.PlayerAttack);
    public static void PlaySound(Sound sound)
    {
        if (CanPlaySound(sound))
        {
            if(oneShotGameObject == null)
            {
                oneShotGameObject = new GameObject("One Shot Sound");
                oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>();
            }
            
            oneShotAudioSource.PlayOneShot(GetAudioClip(sound));
        }
    }

    private static bool CanPlaySound(Sound sound)
    {
        switch (sound)
        {
            default: return true;
            case Sound.PlayerMove: 
                if (soundTimerDictionary.ContainsKey(sound))
                {
                    float lastTimePlayed = soundTimerDictionary[sound];
                    float playerMoveTimerMax = .05f;
                    if (lastTimePlayed + playerMoveTimerMax < Time.time)
                    {
                        soundTimerDictionary[sound] = Time.time;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
        }
    }
    private static AudioClip GetAudioClip(Sound sound)
    {
        foreach (GameAssets.SoundAudioClip soundAudioClip in GameAssets.i.soundAudioClipArray)
        {
            if(soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }

        Debug.LogError("SoundManager -> Sound " + sound + " not found!");
        return null;
    }
    private static AudioSource ApplyAudioSourceSettings(Sound sound, AudioSource audioSource)
    {
        switch(sound) 
        {
            default:
                audioSource.maxDistance = 100f;
                audioSource.spatialBlend = 1f;
                audioSource.rolloffMode = AudioRolloffMode.Linear;
                audioSource.dopplerLevel = 0f;
                return audioSource;
            case Sound.PlayerMove:
                audioSource.maxDistance = 50f;
                audioSource.spatialBlend = 1f;
                audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
                audioSource.dopplerLevel = 0f;
                return audioSource;
            //etc
        }
    }

    //Button/UI extension method for playing sounds. Good shit.
    //public static void AddButtonSounds(this Button_UI button_UI)
    //{
    //    button_UI.ClickFunc += () => SoundManager.PlaySound(Sound.ButtonClick); //lambda is that you()=> ?
    //    button_UI.MouseOverOnceFunc += () => SoundManager.PlaySound(Sound.ButtonOver);
    //}
}
