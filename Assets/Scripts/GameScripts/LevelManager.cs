using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance; 

    public List<Enemy> enemies;

    public UnityEvent PlayerDied;
    public UnityEvent LevelComplete;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy (gameObject);
    }

    void Start()
    {
        enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None).ToList();
    }

    public void ReachedEnd()
    {
        if (enemies.Count > 0)
        {
            PlayerDied?.Invoke();
        }

        else
        {
            LevelComplete?.Invoke();
        }
    }
}
