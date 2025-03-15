using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;

    [HideInInspector] public bool hit;

    private List<Enemy> detectedEnemies = new List<Enemy>();
    private Path path;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            StartCoroutine(ExecutePath());
        }
    }
    private void CheckBowEnemies()
    {
        if (GameManager.instance.levelManager.bowEnemies.Count > 0)
        {
            foreach (BowEnemy bowEnemy in GameManager.instance.levelManager.bowEnemies)
            {
                bowEnemy.move++;
            }
        }
    }
    private void UpdatePipeEnemies()
    {
        foreach (PipeEnemy pipeEnemy in GameManager.instance.levelManager.pipeEnemies)
        {
            pipeEnemy.CheckDirection();
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

            Debug.DrawRay(pos, node.Position - pos, Color.red, 100);

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

            yield return StartCoroutine(MoveToNodePosition(node.Position));
            if (hit) Die();
        }

        ExecuteEnemies();
        LevelManager.instance.ReachedEnd();
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
        detectedEnemies.ForEach(e =>
        {
            if (e.hit)
            {
                e.HitByPlayer();
                if (LevelManager.instance.enemies.Contains(e)) LevelManager.instance.enemies.Remove(e);
            }
        });

        PathCreator.instance.ResetPath();
    }

    private void Die() {
        PathCreator.instance.ResetPath();
        LevelManager.instance.ReachedEnd();
        Destroy(gameObject);
    }
}
