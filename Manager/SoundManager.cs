using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager 
{
    #region 열거형 및 변수
    public enum AudioType
    {
        Bgm,
        Sfx
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
    
    Dictionary<string, AudioSource> _sources = new Dictionary<string, AudioSource>();
    Dictionary<Type, AudioClip[]> _audioClips = new Dictionary<Type, AudioClip[]>();
    int loadingCount = 0;
    #endregion

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
        // bgm 소스는 루프 설정
        _sources[AudioType.Bgm.ToString()].loop = true;

        LoadAudioClips();
    }
    #endregion

    #region 리소스 로드
    public void LoadAudioClips()
    {
        LoadAudio(typeof(Bgms));
        LoadAudio(typeof(Sfxs));
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
    // 로드 완료 후에 실행할 내용 생기면 추가
    void OnLoadCompleted()
    {

    }
    #endregion

    #region 볼륨 설정
    float _sfxVolume = 1.0f;
    public void SetBgmVolume(float value) { _sources[AudioType.Bgm.ToString()].volume = value; }
    public void SetSfxVolume(float value) { _sfxVolume = value; }
    #endregion

    #region 재생 / 정지
    public void PlayBgm(Bgms bgm) { Play(AudioType.Bgm, typeof(Bgms), (int)bgm); }
    public void PlaySfx(Sfxs sfx, float amplifier = 1.0f) { Play(AudioType.Sfx, typeof(Sfxs), (int)sfx, amplifier); }

    void Play(AudioType audioType, Type type, int idx, float amplifier = 1.0f)
    {
        AudioClip clip = _audioClips[type][idx];
        AudioSource source = _sources[audioType.ToString()];

        switch (audioType)
        {
            case AudioType.Bgm:
                source.clip = clip;
                source.Play();
                break;
            case AudioType.Sfx:
                source.volume = _sfxVolume * amplifier;
                source.PlayOneShot(clip);
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