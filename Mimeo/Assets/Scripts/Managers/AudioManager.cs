using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] playlist;
    public AudioClip buttonHoverSound;
    public AudioClip buttonClickSound;
    public AudioSource audioSource;
    public AudioMixerGroup sfxMixer;
    
    private int _musicIndex = 0;
    
    public static AudioManager instance { get; private set; }
    
    private void Awake()
    {
        if (instance && instance != this) Destroy(this); 
        else instance = this;
        
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        audioSource.clip = playlist[0];
        audioSource.Play();
    }

    void Update()
    {
        if (!audioSource.isPlaying) PlayNextSong();
    }

    void PlayNextSong()
    {
        _musicIndex = (_musicIndex + 1) % playlist.Length;
        audioSource.clip = playlist[_musicIndex];
        audioSource.Play();
    }

    public AudioSource PlayClipAt(AudioClip clip, Vector3 pos)
    {
        GameObject tempGo = new GameObject("TempAudio");
        tempGo.transform.position = pos;
        AudioSource audioSource = tempGo.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.outputAudioMixerGroup = sfxMixer;
        audioSource.Play();
        Destroy(tempGo, clip.length);
        return audioSource;
    }

    public void OnButtonHover()
    {
        PlayClipAt(buttonHoverSound, new Vector3());
    }

    public void OnButtonClick()
    {
        PlayClipAt(buttonClickSound, new Vector3());
    }
}