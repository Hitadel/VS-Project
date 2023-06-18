using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ゲームのサウンドを管理する機能です。
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    // BGMの設定
    [Header("#BGM")]
    public AudioClip bgmClip;  
    public float bgmVolume;  
    AudioSource bgmPlayer;  
    AudioHighPassFilter bgmEffect;

    // 効果音の設定
    [Header("#SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex;

    // 効果音の列挙型
    public enum Sfx { Dead, Hit, LevelUp=3, Lose, Melee, Range=7, Select, Win }  

    void Awake()
    {
        instance = this;
        Init(); //　初期化
    }

    void Init()
    {
        // BGMプレイヤーの初期化
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

        // 効果音プレイヤーの初期化
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for (int i = 0; i < sfxPlayers.Length; i++)
        {
            sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].bypassListenerEffects = true;
            sfxPlayers[i].volume = sfxVolume;
        }
    }
    // BGMの再生
    public void PlayBgm(bool isPlay)
    {
        if (isPlay)
            bgmPlayer.Play();
        else
        {
            bgmPlayer.Stop();
        }
    }

    // レベルが上がるとBGMにエフェクトが入る
    public void EffectBgm(bool isPlay)
    {
        bgmEffect.enabled = isPlay;
    }


    // 効果音の再生
    public void PlaySfx(Sfx sfx)
    {
        for (int i = 0; i < sfxPlayers.Length; i++)
        {
            int loopIndex = (i + channelIndex) % sfxPlayers.Length;

            if(sfxPlayers[loopIndex].isPlaying)
                continue;

            int ranIndex = 0;
            if (sfx == Sfx.Hit || sfx == Sfx.Melee)
            {
                ranIndex = Random.Range(0,2);
            }

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx + ranIndex];
            sfxPlayers[loopIndex].Play();  
            break;
        }
    }
}
