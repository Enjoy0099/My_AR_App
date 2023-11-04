using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource EffectsSource;
    public AudioSource MusicSource;
    public AudioSource VFXSource;

    public float LowPitchRange = 0.95f;
    public float HightPitchRange = 1.05f;

    public static SoundManager instance = null;

    public AudioClip dart_Hit;
    public AudioClip dart_Throw;
    public AudioClip dart_Destroy;
    public AudioClip dart_Reload;
    public AudioClip object_Placed;
    public AudioClip plane_Scanning;
    public AudioClip dart_GameBackMusic;
    public AudioClip dart_DoubleScoreSound;
    public AudioClip dart_TrippleScoreSound;
    public AudioClip power_selectSound;
    public AudioClip power_PerfectSelectSound;
    public AudioClip game_NewRecordMadeSound;



    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        else if(instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        play_dartbackMusic();
    }

    public void Play(AudioClip clip)
    {
        if (clip)
        {
            EffectsSource.clip = clip;
            EffectsSource.Play();
        }
        
    }

    public void PlayMusic(AudioClip clip)
    {
        MusicSource.clip = clip;
        MusicSource.Play();
    }

    public void PlayVFX(AudioClip clip)
    {
        VFXSource.clip = clip;
        VFXSource.Play();
    }

    public void RandomSoundEffect(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(LowPitchRange, HightPitchRange);

        EffectsSource.pitch = randomPitch;
        EffectsSource.clip = clips[randomIndex];
        EffectsSource.Play();
    }

    public void play_ObjectPlacedSound()
    {
        Play(object_Placed);
    }

    public void play_dartHitSound()
    {
        Play(dart_Hit);
    }

    public void play_dartThrowSound()
    {
        Play(dart_Throw);
    }

    public void play_dartDestroySound()
    {
        Play(dart_Destroy);
    }

    public void play_dartReloadSound()
    {
        Play(dart_Reload);
    }

    public void play_dartbackMusic()
    {
        //Play(dart_GameBackMusic);
    }

    public void play_DoubleScoreSound()
    {
        PlayVFX(dart_DoubleScoreSound);
    }

    public void play_TrippleScoreSound()
    {
        PlayVFX(dart_TrippleScoreSound);
    }

    public void play_PowerSelectSound()
    {
        PlayVFX(power_selectSound);
    }

    public void play_PowerPerfectSelectSound()
    {
        PlayVFX(power_PerfectSelectSound);
    }

    public void play_NewRecordMadeSound()
    {
        PlayVFX(game_NewRecordMadeSound);
    }
}
