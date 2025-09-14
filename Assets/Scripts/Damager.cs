using UnityEngine;

public class Damager : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDamagable damagable))
        {
            damagable.OnDamaged();
            Debug.Log("Damage!!!!");
        }
    }
}
