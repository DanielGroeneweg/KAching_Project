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
    public bool moving;

    private List<Enemy> detectedEnemies = new List<Enemy>();
    private Path path;
    private void Start()
    {
        GameManager.instance.playerRef = this;
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && !hit && !moving) {
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
        moving = true;
        for (int i = 0; i < path.nodes.Count; i++) {
            CheckBowEnemies();
            UpdatePipeEnemies();

            Node node = path.nodes[i];

            SoundPlayer.instance.PlayDashound();
            PlayerPrefs.SetInt("Moves", PlayerPrefs.GetInt("Moves") + 1);
            yield return StartCoroutine(MoveToNodePosition(node.Position));
            if (hit) Die();
        }

        LevelManager.instance.ReachedEnd();
        PathCreator.instance.ResetPath();
        transform.localEulerAngles = Vector3.zero;
        if (!hit) spriteRenderer.sprite = standing;
        else
        {
            spriteRenderer.sprite = dead;
            transform.localScale = new Vector3(0.4f, 0.4f, 1);
        }
    }

    private IEnumerator MoveToNodePosition(Vector2 target)
    {

        // Look at node
        if (target.x > transform.position.x) spriteRenderer.flipX = true;
        else spriteRenderer.flipX = false;

        // Rotate to node
        transform.localEulerAngles = Vector3.zero;
        float angle = Angle((target - (Vector2)transform.position).normalized);
        Debug.Log(angle);

        if (!spriteRenderer.flipX) transform.localEulerAngles = new Vector3(0, 0, angle + 180);
        else transform.localEulerAngles = new Vector3(0, 0, angle);

        while ((Vector2)transform.position != target)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, Time.deltaTime * speed);
            yield return null;
            if (hit) Die();
            if (died)
            {
                transform.localEulerAngles = Vector3.zero;
                StopAllCoroutines();
            }
        }
    }
    public static float Angle(Vector2 v)
    {
        return (Mathf.Atan2(v.y, v.x) / Mathf.PI) * 180;
    }
    private void Die() {
        died = true;
        PathCreator.instance.ResetPath();
        LevelManager.instance.ReachedEnd();
        spriteRenderer.sprite = dead;
        transform.localScale = new Vector3(0.4f, 0.4f, 1);
    }
    private void CheckCollision()
    {
        RaycastHit2D rayhit = Physics2D.Raycast(transform.position, Vector2.zero);
        if (rayhit && rayhit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Enemy enemy = rayhit.collider.gameObject.GetComponent<Enemy>();
            if (!detectedEnemies.Contains(enemy))
            {
                SoundPlayer.instance.PlayHitSound();
                enemy.Detected();
                detectedEnemies.Add(enemy);
                if (enemy.hit)
                {
                    enemy.HitByPlayer();
                    if (enemy.health <= 0) LevelManager.instance.enemies.Remove(enemy);
                }
                if (hit) Die();
            }
            
        }
    }
}
