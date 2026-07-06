using UnityEngine;

/// <summary>
/// Central place for playing sound effects. All clips are optional (null-checked),
/// so the game runs fine even before you've added your own audio files —
/// just drag clips onto the fields in the Inspector when you have them.
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Clips (optional)")]
    public AudioClip catchCleanClip;
    public AudioClip catchPollutionClip;
    public AudioClip winClip;
    public AudioClip loseClip;

    private AudioSource source;

    private void Awake()
    {
        Instance = this;
        source = GetComponent<AudioSource>();
        if (source == null)
        {
            source = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlayCatchClean()
    {
        if (catchCleanClip != null) source.PlayOneShot(catchCleanClip);
    }

    public void PlayCatchPollution()
    {
        if (catchPollutionClip != null) source.PlayOneShot(catchPollutionClip);
    }

    public void PlayWin()
    {
        if (winClip != null) source.PlayOneShot(winClip);
    }

    public void PlayLose()
    {
        if (loseClip != null) source.PlayOneShot(loseClip);
    }
}
