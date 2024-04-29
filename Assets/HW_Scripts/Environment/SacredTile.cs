using UnityEngine;

public class SacredTile : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent<Player>(out _))
            gameObject.SetActive(false);
    }
}