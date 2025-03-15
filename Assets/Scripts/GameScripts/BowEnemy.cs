using TMPro;
using UnityEngine;

public class BowEnemy : Enemy
{
    [SerializeField] private int movesBeforeShooting = 3;
    [SerializeField] private TMP_Text text;
    public int move = 0;

    private void Update()
    {
        if (move > movesBeforeShooting && !hit)
        {
            player.GetComponent<Player>().hit = true;
        }

        text.text = Mathf.Clamp(movesBeforeShooting - move, 0, 100).ToString();
    }
}
