using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class BowEnemy : Enemy
{
    [SerializeField] private int movesBeforeShooting = 3;
    [SerializeField] private TMP_Text text;
    [SerializeField] private Arrow arrow;
    [SerializeField] private SpriteRenderer spriteRenderer;
    public int move = 0;

    private void Update()
    {
        if (move > movesBeforeShooting && !hit && !player.GetComponent<Player>().hit)
        {
            player.GetComponent<Player>().hit = true;
            SpawnArrow();
        }

        text.text = Mathf.Clamp(movesBeforeShooting - move, 0, 100).ToString();
    }
    private void SpawnArrow()
    {
        Arrow arrowObject = GameObject.Instantiate(arrow, transform.position, Quaternion.identity);
        arrowObject.StartCoroutine(arrowObject.MoveToPlayer());
    }
    public static float Angle(Vector2 v)
    {
        return (Mathf.Atan2(v.y, v.x) / Mathf.PI) * 180;
    }
}
