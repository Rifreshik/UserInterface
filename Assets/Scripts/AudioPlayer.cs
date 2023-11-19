using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
   public AudioSource AudioSource;

    public void PlayAudio(){
        if(!AudioSource.isPlaying){
            AudioSource.Play();
        }
   }
    public void PauseAudio(){
        if(AudioSource.isPlaying){
            AudioSource.Pause();
        }
   }
    public void StopAudio(){
         AudioSource.Stop();
   }
}
