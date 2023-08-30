using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager 
{
    #region 열거형
    public enum AudioType
    {
        Bgm,
        Sfx,
        Voice
    }
    public enum Bgms
    {
        Sound_Title,
        Sound_Main
    }
    public enum Sfxs
    {
        Sound_Upgrade,
        Sound_BtnClick,
        Sound_Attack
    }
    public enum Voices
    {

    }
    #endregion
    Dictionary<string, AudioSource> _sources = new Dictionary<string, AudioSource>();
    Dictionary<Type, AudioClip[]> _audioClips = new Dictionary<Type, AudioClip[]>();
    int loadingCount = 0;

    #region 초기화
    public void Init()
    {
        GameObject source = new GameObject() { name = "AudioSources" };
        UnityEngine.Object.DontDestroyOnLoad(source);

        string[] names = Enum.GetNames(typeof(AudioType));
        for(int i = 0; i < names.Length; i++)
        {
            GameObject go = new GameObject(names[i]);
            go.AddComponent<AudioSource>();
            go.transform.SetParent(source.transform);
            _sources.Add(names[i], go.GetComponent<AudioSource>());
        }

        LoadAudioClips();
    }
    #endregion

    #region 리소스 로드
    public void LoadAudioClips()
    {
        LoadAudio(typeof(Bgms));
        LoadAudio(typeof(Sfxs));
        LoadAudio(typeof(Voices));
    }

    void LoadAudio(Type type)
    {
        string[] names = Enum.GetNames(type);
        _audioClips.Add(type, new AudioClip[names.Length]);
        
        for (int i = 0; i < names.Length; i++)
        {
            loadingCount++;
            Managers.Resc.LoadByIdx<AudioClip>(names[i], i, (audio, idx) =>
            {
                _audioClips[type][idx] = audio;
                CheckAudioLoading();
            });
        }
    }

    void CheckAudioLoading()
    {
        loadingCount--;
        if (loadingCount == 0)
            OnLoadCompleted();
    }

    void OnLoadCompleted()
    {

    }
    #endregion

    #region 재생 / 정지
    public void PlayBgm(Bgms bgm, float volume = 1.0f) { Play(AudioType.Bgm, typeof(Bgms), (int)bgm, volume); }
    public void PlaySfx(Sfxs sfx, float volume = 1.0f) { Play(AudioType.Sfx, typeof(Sfxs), (int)sfx, volume); }
    public void PlayVoice(Voices voice, float volume = 1.0f) { Play(AudioType.Voice, typeof(Voices), (int)voice, volume); }

    void Play(AudioType audioType, Type type, int idx, float volume = 1.0f)
    {
        AudioClip clip = _audioClips[type][idx];
        AudioSource source = _sources[audioType.ToString()];

        switch (audioType)
        {
            case AudioType.Bgm:
                source.clip = clip;
                source.volume = volume;
                source.Play();
                break;
            case AudioType.Sfx:
                source.volume = volume;
                source.PlayOneShot(clip);
                break;
            case AudioType.Voice:
                source.clip = clip;
                source.volume = volume;
                source.Play();
                break;
        }
    }

    public void StopBgm()
    {
        _sources[AudioType.Bgm.ToString()].Stop();
    }

    public void StopAllSound()
    {
        foreach (AudioSource source in _sources.Values)
            source.Stop();
    }
    #endregion
}
