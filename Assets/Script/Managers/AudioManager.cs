using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header(" Elements")]
    [SerializeField] private AudioSource mergeSource;
    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private AudioSource buttonClickSource;

    [Header(" Sounds ")]
    [SerializeField] private AudioClip[] mergeClips;
    [SerializeField] private AudioClip buttonClickClip;


    private void Awake()
    {
        instance = this;

        MergeManager.onMergeProcessed += MergeProcessedCallback;
        SettingsManager.onSFXValueChanged += SFXValueChangedCallback;
        SettingsManager.onMusicValueChanged += MusicValueChangedCallback;
    }

    private void OnDestroy()
    {
        MergeManager.onMergeProcessed -= MergeProcessedCallback;
        SettingsManager.onSFXValueChanged -= SFXValueChangedCallback;
        SettingsManager.onMusicValueChanged -= MusicValueChangedCallback;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {

    }



    private void MergeProcessedCallback(BloopType bloopType, Vector2 mergePos)
    {
        PlayMergeSound();
    }

    public void PlayMergeSound()
    {
        //mergeSource.pitch = Random.Range(0.8f, 1.2f);
        mergeSource.clip = mergeClips[Random.Range(0, mergeClips.Length)];
        mergeSource.Play();
    }

    public void PlayClickSound()
    {
        buttonClickSource.clip = buttonClickClip;
        buttonClickSource.Play();
    }

    private void SFXValueChangedCallback(bool sfxActive)
    {
        mergeSource.mute = !sfxActive;
    }

    private void MusicValueChangedCallback(bool musicActive)
    {
        backgroundMusic.mute = !musicActive;
    }
}