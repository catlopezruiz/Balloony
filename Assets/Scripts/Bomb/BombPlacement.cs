using UnityEngine;

public class BombPlacement : MonoBehaviour
{
    public GameObject bombPrefab;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 pos = transform.position;
            pos.x = Mathf.Round(pos.x);
            pos.y = Mathf.Round(pos.y);

            Instantiate(bombPrefab, pos, Quaternion.identity);
        }
    }
}