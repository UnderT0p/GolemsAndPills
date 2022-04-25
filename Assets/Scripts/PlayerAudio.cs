using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private AudioClip moveAudio;
    [SerializeField] private AudioClip[] jumpAudio;
    [SerializeField] private AudioClip[] landingAudio;
    [SerializeField] private AudioClip[] crouchStartAudio;
    [SerializeField] private AudioClip[] crouchEndAudio;
    [SerializeField] private float[] pitchesMoveAudio;
    [SerializeField] private float[] volumesMoveAudio;
    [SerializeField] private AudioClip punchAudio;
    [SerializeField] private AudioClip shutAudio;
    [SerializeField] private AudioClip coinAudio;
    private float pitchMoveAudioNow;
    private float volumeMoveAudioNow; // [] volumesMoveAudio
    private bool playerCrouchNow = false;
    private AudioSource[] audioSource;
    private AudioSource audioSourceRun;
    private AudioSource audioSourceJumpAndCrouch;
    private AudioSource audioSourceShut;
    private void Start()
    {
        audioSource = GetComponentsInChildren<AudioSource>();
        audioSourceRun= System.Array.Find(GetComponentsInChildren<AudioSource>(), a => a.name == "RunAudio");
        audioSourceJumpAndCrouch = System.Array.Find(GetComponentsInChildren<AudioSource>(), a => a.name == "JumpAudio");
        audioSourceShut= System.Array.Find(GetComponentsInChildren<AudioSource>(), a => a.name == "ShutAudio");
        SubscribeEvents();
    }
    private void Update()
    {
        if(GameBehavior.GetInstance().PlayerHP<=0)
        {
            UnSubscribeEvents();
            PlayStopAudio();
        }
    }

    private void PlayJumpAudio() => PlayerSound(audioSourceJumpAndCrouch, jumpAudio[Random.Range(0, jumpAudio.Length-1)], 1f, GameBehavior.GetInstance().Volume);
    private void PlayMoveAudio(bool appruveRun)
    {
        CheckSpeedPlayer(appruveRun, playerCrouchNow);
        PlayerSound(audioSourceRun, moveAudio, pitchMoveAudioNow, volumeMoveAudioNow);
    }
    private void PlayStopAudio()
    {
        PlayerSound(audioSourceRun, null, pitchMoveAudioNow, volumeMoveAudioNow);
    }
    private void PlayCrouchStartAudio()
    { 
        PlayerSound(audioSourceJumpAndCrouch, crouchStartAudio[Random.Range(0, crouchStartAudio.Length - 1)], 1f, GameBehavior.GetInstance().Volume);
        playerCrouchNow = true;
    }
    private void PlayCrouchEndAudio()
    {
        PlayerSound(audioSourceJumpAndCrouch, crouchEndAudio[Random.Range(0, crouchEndAudio.Length - 1)], 1f, GameBehavior.GetInstance().Volume);
        playerCrouchNow = false;
    }
    private void PlayLandingAudio() => PlayerSound(audioSourceJumpAndCrouch, landingAudio[Random.Range(0, landingAudio.Length - 1)], 1f, GameBehavior.GetInstance().Volume);
    private void PlayPunch()
    {
        PlayerSound(audioSourceJumpAndCrouch, punchAudio, 1f, (GameBehavior.GetInstance().Volume)/5);
    }
    private void PlayShut()
    {
        PlayerSound(audioSourceShut, shutAudio, 1f, GameBehavior.GetInstance().Volume);
    }
    private void PlayCoin()
    {
        PlayerSound(audioSourceShut, coinAudio, 1f, GameBehavior.GetInstance().Volume);
    }
    private void PlayerSound(AudioSource audioSource, AudioClip Audio, float audioPitch, float volumeAudio)
    {
        audioSource.clip = Audio;
        audioSource.pitch = audioPitch;
        audioSource.volume = volumeAudio;
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        else if (Audio == null)
        {
            audioSource.Play();
        }
    }
    private void CheckSpeedPlayer(bool appruveRun,bool playerCrouchNow)
    {
        if (appruveRun)
        {
            pitchMoveAudioNow = pitchesMoveAudio[0];
            volumeMoveAudioNow = volumesMoveAudio[0];
        }
        else if (playerCrouchNow)
        {
            pitchMoveAudioNow = pitchesMoveAudio[1];
            volumeMoveAudioNow = volumesMoveAudio[1];
        }
        else
        {
            pitchMoveAudioNow = pitchesMoveAudio[2];
            volumeMoveAudioNow = volumesMoveAudio[2];
        }

    }
    private void SubscribeEvents()
    {
        GetComponent<PlayerJump>().Jumped += PlayJumpAudio;
        GetComponent<PlayerMove>().movePlayer += PlayMoveAudio;
        GetComponent<PlayerMove>().stopPlayer += PlayStopAudio;
        GetComponent<PlayerCrouch>().CrouchStart += PlayCrouchStartAudio;
        GetComponent<PlayerCrouch>().CrouchEnd += PlayCrouchEndAudio;
        GetComponent<PlayerGroundChecker>().Grounded += PlayLandingAudio;
        GetComponent<PlayerBehavior>().punchPlayer += PlayPunch;
        GetComponent<PlayerBehavior>().shutPlayer += PlayShut;
        GetComponent<PlayerBehavior>().coinPlayer += PlayCoin;
    }
    private void UnSubscribeEvents()
    {
        GetComponent<PlayerJump>().Jumped -= PlayJumpAudio;
        GetComponent<PlayerMove>().movePlayer -= PlayMoveAudio;
        GetComponent<PlayerMove>().stopPlayer -= PlayStopAudio;
        GetComponent<PlayerCrouch>().CrouchStart -= PlayCrouchStartAudio;
        GetComponent<PlayerCrouch>().CrouchEnd -= PlayCrouchEndAudio;
        GetComponent<PlayerGroundChecker>().Grounded -= PlayLandingAudio;
        GetComponent<PlayerBehavior>().punchPlayer -= PlayPunch;
        GetComponent<PlayerBehavior>().shutPlayer -= PlayShut;
        GetComponent<PlayerBehavior>().coinPlayer -= PlayCoin;
    }

}
