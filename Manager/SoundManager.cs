using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager 
{
    #region ������ �� ����
    public enum AudioType
    {
        Bgm,
        Sfx
    }
    public enum Bgms
    {
        Sound_Title,
        Sound_Main,
        Sound_Boss,
        Sound_BossClear
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

    #region �ʱ�ȭ
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
        // bgm �ҽ��� ���� ����
        _sources[AudioType.Bgm.ToString()].loop = true;

        LoadAudioClips();
    }
    #endregion

    #region ���ҽ� �ε�
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
    // �ε� �Ϸ� �Ŀ� ������ ���� ����� �߰�
    void OnLoadCompleted()
    {
        
    }
    #endregion

    #region ���� ����
    float _sfxVolume = 1.0f;
    public void SetBgmVolume(float value) { _sources[AudioType.Bgm.ToString()].volume = value; }
    public void SetSfxVolume(float value) { _sfxVolume = value; }
    #endregion

    #region ��� / ����
    public void PlayBgm(Bgms bgm) { Play(AudioType.Bgm, typeof(Bgms), (int)bgm); }
    public void PlaySfx(Sfxs sfx, float amplifier = 1.0f) { Play(AudioType.Sfx, typeof(Sfxs), (int)sfx, amplifier); }

    void Play(AudioType audioType, Type type, int idx, float amplifier = 1.0f)
    {
        AudioClip clip = _audioClips[type][idx];
        if (clip == null)
        {
            LoadAndPlay(audioType, type, idx, amplifier);
            return;
        }
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

    void LoadAndPlay(AudioType audioType, Type type, int idx, float amplifier)
    {
        string key = Enum.GetNames(type)[idx];
        AudioSource source = _sources[audioType.ToString()];

        Managers.Resc.Load<AudioClip>(key, (clip) =>
        {
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
            _audioClips[type][idx] = clip;
        });
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