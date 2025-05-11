using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class FirstPersonAudio : MonoBehaviour
{
    public FirstPersonMovement character;
    public GroundCheck groundCheck;

    [Header("Step")]
    public AudioSource stepAudio;
    public AudioSource grassStepAudio;
    public AudioSource waterStepAudio;
    public AudioSource mudStepAudio;
    public AudioSource cornStepAudio;
    public AudioSource runningAudio;
    [Tooltip("Minimum velocity for moving audio to play")]
    public float velocityThreshold = .01f;
    Vector2 lastCharacterPosition;
    Vector2 CurrentCharacterPosition => new Vector2(character.transform.position.x, character.transform.position.z);

    [Header("Landing")]
    public AudioSource landingAudio;
    public AudioClip[] landingSFX;

    [Header("Jump")]
    public Jump jump;
    public AudioSource jumpAudio;
    public AudioClip[] jumpSFX;

    [Header("Crouch")]
    public Crouch crouch;
    public AudioSource crouchStartAudio, crouchedAudio, crouchEndAudio;
    public AudioClip[] crouchStartSFX, crouchEndSFX;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    [Header("Snapshots")]
    public AudioMixerSnapshot defaultSnapshot;
    public AudioMixerSnapshot waterSnapshot;

    AudioSource[] MovingAudios => new AudioSource[] { stepAudio, runningAudio, crouchedAudio, grassStepAudio, waterStepAudio, mudStepAudio, cornStepAudio };

    void Reset()
    {
        character = GetComponentInParent<FirstPersonMovement>();
        groundCheck = (transform.parent ?? transform).GetComponentInChildren<GroundCheck>();

        stepAudio = GetOrCreateAudioSource("Step Audio");
        grassStepAudio = GetOrCreateAudioSource("Grass Step Audio");
        waterStepAudio = GetOrCreateAudioSource("Water Step Audio");
        mudStepAudio = GetOrCreateAudioSource("Mud Step Audio");
        mudStepAudio = GetOrCreateAudioSource("Corn Step Audio");
        runningAudio = GetOrCreateAudioSource("Running Audio");
        landingAudio = GetOrCreateAudioSource("Landing Audio");

        jump = GetComponentInParent<Jump>();
        if (jump)
        {
            jumpAudio = GetOrCreateAudioSource("Jump Audio");
        }

        crouch = GetComponentInParent<Crouch>();
        if (crouch)
        {
            crouchStartAudio = GetOrCreateAudioSource("Crouch Start Audio");
            crouchedAudio = GetOrCreateAudioSource("Crouched Audio");
            crouchEndAudio = GetOrCreateAudioSource("Crouch End Audio");
        }
    }

    void OnEnable() => SubscribeToEvents();

    void OnDisable() => UnsubscribeToEvents();

    void FixedUpdate()
    {
        float velocity = Vector3.Distance(CurrentCharacterPosition, lastCharacterPosition);

        if (velocity >= velocityThreshold && groundCheck && groundCheck.isGrounded)
        {
            if (crouch && crouch.IsCrouched)
            {
                SetPlayingMovingAudio(crouchedAudio);
                defaultSnapshot?.TransitionTo(0.2f); // Mantén el snapshot por defecto
            }
            else if (character.IsRunning)
            {
                SetPlayingMovingAudio(runningAudio);
                defaultSnapshot?.TransitionTo(0.2f);
            }
            else
            {
                string surfaceTag = GetSurfaceTag();
                switch (surfaceTag)
                {
                    case "GrassSteps":
                        SetPlayingMovingAudio(grassStepAudio);
                        defaultSnapshot?.TransitionTo(0.2f);
                        break;
                    case "WaterSteps":
                        SetPlayingMovingAudio(waterStepAudio);
                        waterSnapshot?.TransitionTo(0.2f);
                        break;
                    case "MudSteps":
                        SetPlayingMovingAudio(mudStepAudio);
                        defaultSnapshot?.TransitionTo(0.2f);
                        break;
                    case "CornSteps":
                        SetPlayingMovingAudio(cornStepAudio);
                        defaultSnapshot?.TransitionTo(0.2f);
                        break;
                    default:
                        SetPlayingMovingAudio(stepAudio);
                        defaultSnapshot?.TransitionTo(0.2f);
                        break;
                }
            }
        }
        else
        {
            SetPlayingMovingAudio(null);
            defaultSnapshot?.TransitionTo(0.2f);
        }

        lastCharacterPosition = CurrentCharacterPosition;
    }

    void SetPlayingMovingAudio(AudioSource audioToPlay)
    {
        foreach (var audio in MovingAudios.Where(audio => audio != audioToPlay && audio != null))
        {
            audio.Pause();
        }

        if (audioToPlay && !audioToPlay.isPlaying)
        {
            audioToPlay.Play();
        }
    }

    string GetSurfaceTag()
    {
        RaycastHit hit;
        Vector3 rayOrigin = character.transform.position + Vector3.up * 0.1f;
        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, 2f))
        {
            return hit.collider.tag;
        }
        return "Default";
    }

    void PlayLandingAudio() => PlayRandomClip(landingAudio, landingSFX);
    void PlayJumpAudio() => PlayRandomClip(jumpAudio, jumpSFX);
    void PlayCrouchStartAudio() => PlayRandomClip(crouchStartAudio, crouchStartSFX);
    void PlayCrouchEndAudio() => PlayRandomClip(crouchEndAudio, crouchEndSFX);

    void SubscribeToEvents()
    {
        groundCheck.Grounded += PlayLandingAudio;

        if (jump)
        {
            jump.Jumped += PlayJumpAudio;
        }

        if (crouch)
        {
            crouch.CrouchStart += PlayCrouchStartAudio;
            crouch.CrouchEnd += PlayCrouchEndAudio;
        }
    }

    void UnsubscribeToEvents()
    {
        groundCheck.Grounded -= PlayLandingAudio;

        if (jump)
        {
            jump.Jumped -= PlayJumpAudio;
        }

        if (crouch)
        {
            crouch.CrouchStart -= PlayCrouchStartAudio;
            crouch.CrouchEnd -= PlayCrouchEndAudio;
        }
    }

    AudioSource GetOrCreateAudioSource(string name)
    {
        AudioSource result = System.Array.Find(GetComponentsInChildren<AudioSource>(), a => a.name == name);
        if (result)
            return result;

        result = new GameObject(name).AddComponent<AudioSource>();
        result.spatialBlend = 1;
        result.playOnAwake = false;
        result.transform.SetParent(transform, false);
        return result;
    }

    static void PlayRandomClip(AudioSource audio, AudioClip[] clips)
    {
        if (!audio || clips.Length <= 0)
            return;

        AudioClip clip = clips[Random.Range(0, clips.Length)];
        if (clips.Length > 1)
            while (clip == audio.clip)
                clip = clips[Random.Range(0, clips.Length)];

        audio.clip = clip;
        audio.Play();
    }
}
