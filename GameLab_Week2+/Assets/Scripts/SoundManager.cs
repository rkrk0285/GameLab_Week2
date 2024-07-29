using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);                
    }

    [Header("BGM")]
    public AudioSource SfxPlayer;

    [SerializeField]
    AudioClip[] sfxs;        

    public void PlaySfx(string name)
    {
        switch(name)
        {
            case "Bleed":
                SfxPlayer.clip = sfxs[0];
                break;
            case "NoEyeDog":
                SfxPlayer.clip = sfxs[1];
                break;
            case "Leviathan":
                SfxPlayer.clip = sfxs[2];
                break;
        }
        SfxPlayer.Play();        
    }
}
