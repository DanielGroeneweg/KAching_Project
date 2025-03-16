using Unity.VisualScripting;
using UnityEngine;
public class Enemy : MonoBehaviour
{
    public int health = 1;
    protected Transform player;
    public bool hit;
    public SpriteRenderer grave;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
    }
    
    public virtual void HitByPlayer() {
        health--;

        if (health <= 0 )
        {
            DestroyEnemy();
            SpawnGrave();
        }
    }

    public virtual void Detected() {
        hit = true;
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject, 0.1f);
    }
    private void SpawnGrave()
    {
        GameObject.Instantiate(grave, transform.position, Quaternion.identity);
    }
}
