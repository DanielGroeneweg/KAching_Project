using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private bool reachedPlayer = false;
    private float speed = 30f;
    public IEnumerator MoveToPlayer()
    {
        Vector3 playerPos = GameManager.instance.playerRef.transform.position;
        Vector3 direction = (playerPos - transform.position).normalized;
        float angle = Angle(direction);
        transform.localEulerAngles = new Vector3(0, 0, angle);
        while (!reachedPlayer)
        {
            transform.position += direction * Time.deltaTime * speed;
            if ((playerPos - transform.position).magnitude <= 0.5f) reachedPlayer = true;
            yield return null;
        }
        StopCoroutine(MoveToPlayer());
        Destroy(gameObject);
    }
    public static float Angle(Vector2 v)
    {
        return (Mathf.Atan2(v.y, v.x) / Mathf.PI) * 180;
    }
}
