using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance; 

    public List<Enemy> enemies;
    public List<BowEnemy> bowEnemies;
    public List<PipeEnemy> pipeEnemies;

    // im lazy
    public string nextScene;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy (gameObject);
    }

    void Start()
    {
        enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None).ToList();
        bowEnemies = FindObjectsByType<BowEnemy>(FindObjectsSortMode.None).ToList();
        pipeEnemies = FindObjectsByType<PipeEnemy>(FindObjectsSortMode.None).ToList();
    }

    public void ReachedEnd()
    {
        if (enemies.Count > 0)
        {
            SceneManager.LoadScene("GameOver");
        }

        else
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}
