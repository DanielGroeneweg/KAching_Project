using UnityEngine;

public class PipeEnemy : Enemy
{
    public enum Directions { Left, Right, Up, Down }
    public Directions lookDirection;

    private Directions playerDirectionToEnemy;
    public override void HitByPlayer()
    {
        base.HitByPlayer();

        // else
        // {
        //     // Fucking horrible way of doing this btw
        //     player.GetComponent<Player>().Die();
        // }
    }

    public void CheckDirection() {
        if (player == null) return;

        if (lookDirection == Directions.Left || lookDirection == Directions.Right) {
            if (player.position.x < transform.position.x) playerDirectionToEnemy = Directions.Left;
            if (player.position.x > transform.position.x) playerDirectionToEnemy = Directions.Right;
        } else {
            if (player.position.y < transform.position.y) playerDirectionToEnemy = Directions.Down;
            if (player.position.y > transform.position.y) playerDirectionToEnemy = Directions.Up;
        }
    }
    public void Check()
    {
        if (playerDirectionToEnemy == lookDirection) player.GetComponent<Player>().hit = true;
    }
    public override void Detected() {
        if (playerDirectionToEnemy == lookDirection) player.GetComponent<Player>().hit = true;
        else hit = true;
    }
}