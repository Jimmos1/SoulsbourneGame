using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection; //read about this one

/*
 * This is a supporting class for managers. 
 * Contains many references to sounds in v1 for easy access in sound manager.
 */
public class GameAssets : MonoBehaviour
{
    private static GameAssets _i;

    public static GameAssets i
    {
        get
        {
            if (_i == null) _i = Instantiate(Resources.Load<GameAssets>("GameAssets")); //TODO: Load it into the managers object
            return _i;
        }
    }

    public SoundAudioClip[] soundAudioClipArray;

    [System.Serializable]
    public class SoundAudioClip
    {
        public SoundManager.Sound sound;
        public AudioClip audioClip;
    }
}
