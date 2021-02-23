using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static AudioManager current;
    [Header("环境声音")]
    public AudioClip ambientClip;       //环境音效
    public AudioClip musicClip;         //背景音乐

    [Header("FX特效")]
    public AudioClip deathFXClip;
    public AudioClip orbFXClip;
    public AudioClip doorFXClip;
    public AudioClip startLevelClip;
    public AudioClip winClip;

    [Header("Robbie的音效")]
    public AudioClip[] walkStepClips;         //走路音效
    public AudioClip[] crouchStepClips;       //下蹲音效
    public AudioClip jumpClip;                //跳跃音效
    public AudioClip deathClip;               //死亡音效

    public AudioClip jumpVoiceClip;           //跳跃时人物声音
    public AudioClip deathVoiceClip;          //死亡时人物声音
    public AudioClip orbVoiceClip;            //取得宝珠人物声音

    private AudioSource ambientSource;
    private AudioSource musicSource;
    private AudioSource fxSource;
    private AudioSource playerSource;
    private AudioSource voiceSource;

    public AudioMixerGroup ambientGroup, musicGroup, FXGroup, playerGroup, voiceGroup;

    private void Awake()
    {
        if(current != null)
        {
            Destroy(gameObject);
            return;
        }
        current = this;
        DontDestroyOnLoad(gameObject);
        ambientSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();
        fxSource = gameObject.AddComponent<AudioSource>();
        playerSource = gameObject.AddComponent<AudioSource>();
        voiceSource = gameObject.AddComponent<AudioSource>();

        ambientSource.outputAudioMixerGroup = ambientGroup;
        musicSource.outputAudioMixerGroup = musicGroup;
        fxSource.outputAudioMixerGroup = FXGroup;
        playerSource.outputAudioMixerGroup = playerGroup;
        voiceSource.outputAudioMixerGroup = voiceGroup;

        StartLevelAudio();
    }

    /// <summary>
    /// 环境背景音效
    /// </summary>
    private void StartLevelAudio()
    {
        //环境音效
        current.ambientSource.clip = current.ambientClip;
        current.ambientSource.loop = true;
        current.ambientSource.Play();
        //背景音效
        current.musicSource.clip = current.musicClip;
        current.musicSource.loop = true;
        current.musicSource.Play();

        current.fxSource.clip = current.startLevelClip;
        current.fxSource.Play();
    }

    /// <summary>
    ///胜利音乐 
    /// </summary>
    public static void PlayerWonAudio()
    {
        current.fxSource.clip = current.winClip;
        current.fxSource.Play();
    }

    /// <summary>
    /// 播放打开门的声音
    /// </summary>
    public static void PlayDoorOpenAudio()
    {
        current.fxSource.clip = current.doorFXClip;
        current.fxSource.PlayDelayed(1f);
    }
    /// <summary>
    /// 走路音效
    /// </summary>
    public static void PlayFootstepAudio()
    {
        int index = Random.Range(0, current.walkStepClips.Length);
        current.playerSource.clip = current.walkStepClips[index];
        current.playerSource.Play();
    }
    /// <summary>
    /// 蹲下走路音效
    /// </summary>
    public static void PlayCrouchFootstepAudio()
    {
        int index = Random.Range(0, current.crouchStepClips.Length);
        current.playerSource.clip = current.crouchStepClips[index];
        current.playerSource.Play();
    }
    /// <summary>
    /// 跳跃声音和人声
    /// </summary>
    public static void PlayJumpAudio()
    {
        //跳跃声音
        current.playerSource.clip = current.jumpClip;
        current.playerSource.Play();
        //跳跃人声
        current.voiceSource.clip = current.jumpVoiceClip;
        current.voiceSource.Play();
    }

    /// <summary>
    /// 死亡音效
    /// </summary>
    public static void PlayDeathAudio()
    {
        current.playerSource.clip = current.deathClip;
        current.playerSource.Play();

        current.voiceSource.clip = current.deathVoiceClip;
        current.voiceSource.Play();

        current.fxSource.clip = current.deathFXClip;
        current.fxSource.Play();
    }
    /// <summary>
    /// 取得宝珠音效
    /// </summary>
    public static void PlayOrbAudio()
    {
        current.fxSource.clip = current.orbFXClip;
        current.fxSource.Play();

        current.voiceSource.clip = current.orbVoiceClip;
        current.voiceSource.Play();

    }


}
