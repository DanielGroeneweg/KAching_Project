using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public LevelManager levelManager;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    public void DestroyEnemy(Enemy enemyToBeDestroyed)
    {
        levelManager.enemies.Remove(enemyToBeDestroyed);
    }
}
