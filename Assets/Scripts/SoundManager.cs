using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public enum Clip
    {
        SELECTION,
        RESET,
        HIT,
        SPAWN,

        DEPRIMUS,
        AGILLIUS,
        CRIZATUS,
        DILIUS,
        CONFUSCIUS,
        INGINIUS,
        FOOLUS,
        ORDINARIUS,
        CURIOSUS,
        DISGUSTUS,
        CHANSUS,
        GHINIONUS,
        FERICITUS,
        OBSEDATUS,
        HLIZATUS,
        NERVOSUS
    }

    private static AudioSource src;
    private static Dictionary<Clip, AudioClip> clips;

    void Start()
    {
        src = GetComponent<AudioSource>();
        clips = new Dictionary<Clip, AudioClip>();

        Array clipTypes = Enum.GetValues(typeof(Clip));
        
        for(int i = 0; i < clipTypes.Length; ++i)
        {
            Clip type = (Clip) clipTypes.GetValue(i);
            clips.Add(type, Resources.Load<AudioClip>("Audio/" + type.ToString()[0] + type.ToString().Substring(1).ToLower()));
        }
    }

    public static void Play(Clip clip) => src.PlayOneShot(clips[clip]);
}
