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
        [SerializeField] float timeBetweenAttacks = 1f;

        public float timeSinceLastAttack = 0f;

        Move move;
        Transform target;
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

            if (target != null && !GetIsInRange())
            {
                move.MoveTo(target.position);
                
            }
            else
            {
                move.Cancel();
                AttackBehaviour();
            }
        }

        void AttackBehaviour()
        {           
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                animator.SetTrigger("onAttack");
                timeSinceLastAttack = 0f;
            }
            
        }

        bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.position) < weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.transform;
        }

        public void Cancel()
        {
            target = null;
        }


        //Animation Event
        void Hit()
        {

        }
    }
}
