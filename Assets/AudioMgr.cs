using System.Collections.Generic;
using UnityEngine;

public class AudioMgr : MonoBehaviour {
    [SerializeField]
    private AudioSource source;
    [SerializeField]
    private List<AudioClip> popSounds;
    [SerializeField]
    private AudioClip dropSound;
    [SerializeField]
    private AudioClip winSound;
    [SerializeField]
    private AudioClip loseSound;
    [SerializeField]
    private AudioClip startSound;
    [SerializeField]
    private AudioClip bonusSound;
    public void PlayPopSound() {
        PlaySound(popSounds[Random.Range(0,popSounds.Count)]);
    }

    private void PlaySound(AudioClip clip) {
        source.PlayOneShot(clip);
    }

    public void PlayDropSound() {
        PlaySound(dropSound);
    }
    public void PlayWinSound() {
        source.Stop();
        PlaySound(winSound);
    }
    public void PlayLoseSound() {
        PlaySound(loseSound);
    }
    public void PlayStartSound() {
        PlaySound(startSound);
    }
    public void PlayBonusSound() {
        PlaySound(bonusSound);
    }
}
