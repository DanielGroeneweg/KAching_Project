using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite standing;
    [SerializeField] private Sprite running;
    [SerializeField] private Sprite dead;

    [HideInInspector] public bool hit;
    private bool died;

    private List<Enemy> detectedEnemies = new List<Enemy>();
    private Path path;
    private void Start()
    {
        GameManager.instance.playerRef = this;
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && !hit) {
            spriteRenderer.sprite = running;
            StartCoroutine(ExecutePath());
        }
        CheckCollision();
    }
    private void CheckBowEnemies()
    {
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

            SoundPlayer.instance.PlayDashound();
            PlayerPrefs.SetInt("Moves", PlayerPrefs.GetInt("Moves") + 1);
            yield return StartCoroutine(MoveToNodePosition(node.Position));
            if (hit) Die();
            else ExecuteEnemies();
        }

        LevelManager.instance.ReachedEnd();
        PathCreator.instance.ResetPath();
        if (!hit) spriteRenderer.sprite = standing;
        else spriteRenderer.sprite = dead;
    }

    private IEnumerator MoveToNodePosition(Vector2 target) {

        // Look at node
        if (target.x > transform.position.x) spriteRenderer.flipX = true;
        else spriteRenderer.flipX = false;

        transform.localEulerAngles = Vector3.zero;
        float angle = Vector2.Angle(transform.position, target);
        transform.localEulerAngles = new Vector3(0, 0, angle);

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
        spriteRenderer.sprite = dead;
    }
    private void CheckCollision()
    {
        RaycastHit2D rayhit = Physics2D.Raycast(transform.position, Vector2.zero);
        if (rayhit && rayhit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            SoundPlayer.instance.PlayHitSound();
            Enemy enemy = rayhit.collider.gameObject.GetComponent<Enemy>();
            enemy.Detected();
            detectedEnemies.Add(enemy);
            if (hit) Die();
        }
    }
}
