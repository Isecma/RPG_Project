using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float healthPoints = 100f;

        bool isDead;

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            if (healthPoints == 0 && !isDead)
            {
                Die();
            }
        }

        void Die()
        {
            GetComponent<Animator>().SetTrigger("onDeath");
            isDead = true;
            CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
            if (capsuleCollider == null) return;
            capsuleCollider.enabled = false;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

    }
}