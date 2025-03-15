using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private MovementType movementType;
    [SerializeField] private float attackRadius;

    public bool hit;


    private List<Enemy> detectedEnemies = new List<Enemy>();
    private Path path;

    public enum MovementType {
        INSTANT,
        GRADUAL
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            StartCoroutine(ExecutePath());
        }
    }

    private IEnumerator ExecutePath() {
        path = PathCreator.instance.GetPath();

        if (path.nodes.Count == 0) {
            Debug.LogWarning("Nuh uh list is empty");
            yield break;
        }

        for (int i = 0; i < path.nodes.Count; i++) {
            Node node = path.nodes[i];
            if (GameManager.instance.levelManager.bowEnemies.Count > 0)
            {
                foreach (BowEnemy bowEnemy in GameManager.instance.levelManager.bowEnemies)
                {
                    bowEnemy.move++;
                }
            }

            if (GameManager.instance.levelManager.pipeEnemies.Count > 0)
            {
                foreach (PipeEnemy pipeEnemy in GameManager.instance.levelManager.pipeEnemies)
                {
                    pipeEnemy.CheckDirection();
                }
            }

            Vector2 pos = transform.position;
            RaycastHit2D[] hitList = Physics2D.RaycastAll(pos, node.Position - pos, Vector2.Distance(node.Position, pos));
            foreach (RaycastHit2D hitObject in hitList)
            {
                if (hitObject.collider.CompareTag("PipeEnemy"))
                {
                    hitObject.collider.gameObject.GetComponent<PipeEnemy>().Check();
                }
            }

            //Debug.Log("Node: " + node.Position);
            yield return StartCoroutine(MoveToNodePosition(node.Position));
            if (hit) Die();
        }

        ExecuteEnemies();
        LevelManager.instance.ReachedEnd();
    }

    private IEnumerator MoveToNodePosition(Vector2 target) {
        switch (movementType) {
            case MovementType.INSTANT:
                transform.position = target;
                yield return new WaitForSeconds(.1f);
                break;

            case MovementType.GRADUAL:
                while ((Vector2)transform.position != target) {
                    transform.position = Vector2.MoveTowards(transform.position, target, Time.deltaTime * speed);
                    yield return null;
                }   
                break;
        }

        EnemyCheck();
    }

    private void EnemyCheck() {
        LayerMask enemyLayer = LayerMask.GetMask("Enemy");

        // Change to OverlapCircle if only 1 enemy can get attacked at once
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, attackRadius, enemyLayer);

        foreach (Collider2D enemyCol in enemies) {
            Enemy enemy = enemyCol.GetComponent<Enemy>();   
            enemy.Detected();

            detectedEnemies.Add(enemy);
        }
    }

    private void ExecuteEnemies() {
        detectedEnemies.ForEach(e => { if (e.hit) {
            e.HitByPlayer();
            if (LevelManager.instance.enemies.Contains(e)) LevelManager.instance.enemies.Remove(e);
        }});

        PathCreator.instance.ResetPath();
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    private void Die() {
        Debug.Log("u got killed man :c");
        PathCreator.instance.ResetPath();
        LevelManager.instance.ReachedEnd();
        Destroy(gameObject);
    }
}
