using Unity.VisualScripting;
using UnityEngine;
public class Enemy : MonoBehaviour
{
    [SerializeField] protected int health = 1;
    protected Transform player;
    public bool hit;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
    }
    
    public virtual void HitByPlayer() {
        health--;

        if (health <= 0 )
        {
            DestroyEnemy();
        }
    }

    public virtual void Detected() {
        hit = true;
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
