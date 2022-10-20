using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] float timeBetweenAttacks = 1f;

        public float timeSinceLastAttack = 0f;

        Move move;
        Health target;
        Animator animator;

        void Start()
        {
            move = GetComponent<Move>();
            animator = GetComponent<Animator>();
        }


        void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if(target == null) return;
            if (target.IsDead()) return;

            if (target != null && !GetIsInRange())
            {
                move.MoveTo(target.transform.position);
                
            }
            else
            {
                move.Cancel();
                AttackBehaviour();
            }
        }

        void AttackBehaviour()
        {
            animator.ResetTrigger("onStopAttack");
            transform.LookAt(target.transform);
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                //This will trigger the Hit() event
                animator.SetTrigger("onAttack");
                timeSinceLastAttack = 0f;
            }
            
        }

        //Animation Event
        void Hit()
        {
            if (target == null) return;
            target.TakeDamage(weaponDamage);
        }

        bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            animator.ResetTrigger("onAttack");
            animator.SetTrigger("onStopAttack");
            target = null;        
        }
    }
}
