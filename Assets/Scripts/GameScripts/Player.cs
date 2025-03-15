using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite running;

    [HideInInspector] public bool hit;

    private List<Enemy> detectedEnemies = new List<Enemy>();
    private Path path;
    private void Start()
    {
        GameManager.instance.playerRef = this;
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            StartCoroutine(ExecutePath());
        }
    }
    private void CheckBowEnemies()
    {
        spriteRenderer.sprite = running;

        foreach (Enemy enemy in GameManager.instance.levelManager.enemies)
        {
            if (enemy.gameObject.TryGetComponent<BowEnemy>(out BowEnemy bowEnemy))
            {
                if (bowEnemy != null)
                {
                    bowEnemy.move++;
                }
            }
        }
    }
    private void UpdatePipeEnemies()
    {
        foreach (Enemy enemy in GameManager.instance.levelManager.enemies)
        {
            if (enemy.gameObject.TryGetComponent<PipeEnemy>(out PipeEnemy pipeEnemy))
            {
                if (pipeEnemy != null)
                {
                    pipeEnemy.CheckDirection();
                }
            }
        }
    }
    private IEnumerator ExecutePath() {
        path = PathCreator.instance.GetPath();

        if (path.nodes.Count == 0) {
            yield break;
        }

        for (int i = 0; i < path.nodes.Count; i++) {
            CheckBowEnemies();
            UpdatePipeEnemies();

            Node node = path.nodes[i];

            Vector2 pos = transform.position;

            RaycastHit2D[] hitList = Physics2D.RaycastAll(pos, node.Position - pos, Vector2.Distance(node.Position, pos));
            foreach (RaycastHit2D hitObject in hitList)
            {
                foreach(Enemy enemy in GameManager.instance.levelManager.enemies)
                {
                    if (hitObject.collider.gameObject == enemy.gameObject)
                    {
                        enemy.Detected();
                        detectedEnemies.Add(enemy);
                    }
                }
            }
            SoundPlayer.instance.PlayDashound();
            PlayerPrefs.SetInt("Moves", PlayerPrefs.GetInt("Moves") + 1);
            yield return StartCoroutine(MoveToNodePosition(node.Position));
            if (hit) Die();
            else ExecuteEnemies();
        }

        
        LevelManager.instance.ReachedEnd();
        PathCreator.instance.ResetPath();
    }

    private IEnumerator MoveToNodePosition(Vector2 target) {
        while ((Vector2)transform.position != target)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, Time.deltaTime * speed);
            yield return null;
        }
    }

    private void ExecuteEnemies()
    {
        for (int i = detectedEnemies.Count - 1; i >= 0; i--)
        {
            Enemy e = detectedEnemies[i];
            if (e.hit)
            {
                e.HitByPlayer();
                if (LevelManager.instance.enemies.Contains(e))
                {
                    LevelManager.instance.enemies.Remove(e);
                    detectedEnemies.Remove(e);
                }
            }
        }
    }

    private void Die() {
        PathCreator.instance.ResetPath();
        LevelManager.instance.ReachedEnd();
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            SoundPlayer.instance.PlayHitSound();
        }
    }
}
