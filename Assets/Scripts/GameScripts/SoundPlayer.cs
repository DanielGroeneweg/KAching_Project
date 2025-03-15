using System.Collections;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public static SoundPlayer instance;

    [SerializeField] private AudioClip[] dashSounds;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioSource prefab;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(this);
    }
    public void PlayDashound()
    {
        PlayAudio(dashSounds[Random.Range(0, dashSounds.Length)]);
    }
    public void PlayHitSound()
    {
        PlayAudio(hitSound);
    }
    private void PlayAudio(AudioClip clip)
    {
        AudioSource audio = GameObject.Instantiate(prefab);
        audio.clip = clip;
        audio.Play();
        StartCoroutine(DestroyAudio(audio.gameObject, clip.length));
    }
    private IEnumerator DestroyAudio(GameObject audioObject, float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        Destroy(audioObject);
    }
}
