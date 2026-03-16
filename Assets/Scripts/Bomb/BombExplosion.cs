using UnityEngine;

public class BombExplosion : MonoBehaviour
{
    public float timer = 2f;

    void Start()
    {
        Destroy(gameObject, timer);
    }
}