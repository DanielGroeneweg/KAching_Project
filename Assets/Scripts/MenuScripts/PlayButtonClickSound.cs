using UnityEngine;

public class PlayButtonClickSound : MonoBehaviour
{
    public void PlaySound()
    {
        GameObject.Find("Audio").GetComponent<AudioSource>().Play();
    }
}
