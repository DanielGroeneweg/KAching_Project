using UnityEngine;

public class MusicLooping : MonoBehaviour
{
    public static MusicLooping instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
}
