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
        ButtonClick,
        EstusDrink,
        Slash1,
        Slash2,
        Roll,
        CriticalHit,
        ParryLand,
        FallDown,
        Waterfall,
        Wind,
        Rain,
        ShieldSwing,
        GenericStep,
        LandOnGround
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
    public static void PlaySound(Sound sound, Vector3 position, bool useClipLength = true)
    {
        if (CanPlaySound(sound))
        {

            GameObject soundGameObject = new GameObject("Sound");
            soundGameObject.transform.position = position;
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            audioSource.clip = GetAudioClip(sound);

            //TODO: Set values for each sound case.
            //See ApplyAudioSourceSettings function for more info.
            audioSource = ApplyAudioSourceSettings(sound, audioSource);
            //audioSource.maxDistance = 100f;
            //audioSource.spatialBlend = 1f;
            //audioSource.rolloffMode = AudioRolloffMode.Linear;
            //audioSource.dopplerLevel = 0f;
            //audioSource.volume = 0.5f;
            //audioSource.pitch = Random.Range(0.8f, 1.2f);
            //audioSource.spread = 300f;
            audioSource.Play();

            Object.Destroy(soundGameObject, useClipLength ? audioSource.clip.length : Mathf.Infinity); //TODO: Object pooling in optimization phase.

        }
    }
    //Call with e.g. SoundManager.PlaySound(SoundManager.Sound.PlayerAttack);
    public static void PlaySound(Sound sound)
    {
        if (CanPlaySound(sound))
        {
            if (oneShotGameObject == null)
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
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }

        Debug.LogError("SoundManager -> Sound " + sound + " not found!");
        return null;
    }
    private static AudioSource ApplyAudioSourceSettings(Sound sound, AudioSource audioSource)
    {
        //this never happens
        switch (sound)
        {
            default:
                audioSource.spatialBlend = 1f;
                audioSource.rolloffMode = AudioRolloffMode.Linear;
                audioSource.maxDistance = 100f;
                audioSource.dopplerLevel = 0f;
                audioSource.volume = 0.5f;
                audioSource.pitch = Random.Range(0.8f, 1.2f);
                audioSource.spread = 300f;
                audioSource.spatialize = true;
                return audioSource;
            case Sound.PlayerMove:
                audioSource.spatialBlend = 1f;
                audioSource.rolloffMode = AudioRolloffMode.Linear;
                audioSource.maxDistance = 100f;
                audioSource.dopplerLevel = 0f;
                audioSource.pitch = Random.Range(0.8f, 1.2f);
                audioSource.volume = 0.3f;
                audioSource.spatialize = true;
                audioSource.spread = 300f;
                return audioSource;
            case Sound.Slash1:
                audioSource.spatialBlend = 0f;
                audioSource.rolloffMode = AudioRolloffMode.Linear;
                audioSource.minDistance = 0f;
                audioSource.maxDistance = 100f;
                audioSource.dopplerLevel = 0f;
                audioSource.volume = 1f;
                audioSource.spread = 180f;
                audioSource.pitch = Random.Range(0.8f, 1.2f);
                return audioSource;
            case Sound.Waterfall:
                audioSource.spatialBlend = 1f;
                audioSource.rolloffMode = AudioRolloffMode.Linear;
                audioSource.minDistance = 5f;
                audioSource.maxDistance = 15f;
                audioSource.dopplerLevel = 0f;
                audioSource.volume = 0.3f;
                audioSource.spread = 180f;
                audioSource.loop = true;
                return audioSource;
            case Sound.Rain:
                audioSource.spatialBlend = 1f;
                audioSource.rolloffMode = AudioRolloffMode.Linear;
                audioSource.minDistance = 5.5f;
                audioSource.maxDistance = 29f;
                audioSource.dopplerLevel = 0f;
                audioSource.volume = 0.4f;
                audioSource.spread = 100f;
                audioSource.loop = true;
                return audioSource;
            case Sound.Wind:
                audioSource.spatialBlend = 1f;
                audioSource.rolloffMode = AudioRolloffMode.Linear;
                audioSource.minDistance = 10f;
                audioSource.maxDistance = 110f;
                audioSource.dopplerLevel = 0f;
                audioSource.volume = 0.4f;
                audioSource.spread = 100f;
                audioSource.loop = true;
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
