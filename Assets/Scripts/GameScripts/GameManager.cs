using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public LevelManager levelManager;
    public Player playerRef;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    public void DestroyEnemy(Enemy enemyToBeDestroyed)
    {
        levelManager.enemies.Remove(enemyToBeDestroyed);
        enemyToBeDestroyed.DestroyEnemy();
    }
}
