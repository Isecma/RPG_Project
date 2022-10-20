using System.Collections;
using UnityEngine;

namespace RPG.Combat
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
            print(healthPoints);
        }

        void Die()
        {
            GetComponent<Animator>().SetTrigger("onDeath");
            isDead = true;
            GetComponent<CapsuleCollider>().enabled = false;
        }

    }
}