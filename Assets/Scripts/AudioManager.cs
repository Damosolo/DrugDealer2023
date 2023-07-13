using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource soundEffectSource;
    public AudioClip startTaskSound;
    public AudioClip taskInstructionSound;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void PlaySoundEffect(AudioClip soundClip)
    {
        soundEffectSource.PlayOneShot(soundClip);
    }
}
