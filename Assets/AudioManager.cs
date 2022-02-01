using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource music;
    public AudioClip gameMusic, bossMusic;
    public void PlayGameMusic(){
        music.Stop();
        music.clip = gameMusic;
        music.Play();
    }

    public void PlayBossMusic(){
        music.Stop();
        music.clip = bossMusic;
        music.Play();
    }

    public void PlaySFX(AudioClip clip, Vector3 pos, float volume = 1f){
        AudioSource.PlayClipAtPoint(clip, pos, volume);
    }
}
