using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Manages audio playback within the application, including background music and sound effects.
/// This class follows the Singleton design pattern to ensure a single instance exists throughout the application.
/// It handles initializing the audio settings, playing background music from a playlist, and triggering sound effects for UI interactions.
/// </summary>
/// <author>GUIDEZ Maxence</author>
/// <date>2024-07-22</date>
public class AudioManager : MonoBehaviour
{
    public AudioClip[] playlist;
    public AudioClip buttonHoverSound;
    public AudioClip buttonClickSound;
    public AudioSource audioSource;
    public AudioMixerGroup sfxMixer;
    
    private int _musicIndex = 0;
    
    public static AudioManager instance { get; private set; }
    
    /// <summary>
    /// Initializes the AudioManager instance and ensures it is not destroyed when loading new scenes.
    /// If another instance of AudioManager exists, it destroys the new one to maintain a single instance.
    /// </summary>
    private void Awake()
    {
        if (instance && instance != this) Destroy(this); 
        else instance = this;
        
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Starts playing the first audio clip in the playlist.
    /// </summary>
    void Start()
    {
        audioSource.clip = playlist[0];
        audioSource.Play();
    }
    
    /// <summary>
    /// Checks if the current audio clip has finished playing and starts the next song in the playlist if needed.
    /// </summary>
    void Update()
    {
        if (!audioSource.isPlaying) PlayNextSong();
    }

    /// <summary>
    /// Plays the next song in the playlist, looping back to the start if necessary.
    /// This method updates the audio source with the next clip and starts playback.
    /// </summary>
    void PlayNextSong()
    {
        _musicIndex = (_musicIndex + 1) % playlist.Length;
        audioSource.clip = playlist[_musicIndex];
        audioSource.Play();
    }

    /// <summary>
    /// This method creates a temporary game object at a given position in 3D space with an AudioSource component to play the clip.
    /// The temporary game object is destroyed after the clip finishes playing.
    /// </summary>
    /// <param name="clip">The audio clip to play.</param>
    /// <param name="pos">The position in 3D space where the audio clip will be played.</param>
    /// <returns>The AudioSource component attached to the temporary game object.</returns>
    public AudioSource PlayClipAt(AudioClip clip, Vector3 pos)
    {
        GameObject tempGo = new GameObject("TempAudio");
        tempGo.transform.position = pos;
        AudioSource audioSource = tempGo.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.outputAudioMixerGroup = sfxMixer;
        audioSource.Play();
        DontDestroyOnLoad(tempGo);
        Destroy(tempGo, clip.length);
        return audioSource;
    }

    /// <summary>
    /// This method triggers the sound effect that indicates a button is being hovered over.
    /// </summary>
    public void OnButtonHover()
    {
        PlayClipAt(buttonHoverSound, new Vector3());
    }

    /// <summary>
    /// This method triggers the sound effect that occurs when a button is clicked.
    /// </summary>
    public void OnButtonClick()
    {
        PlayClipAt(buttonClickSound, new Vector3());
    }
}