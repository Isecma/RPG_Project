using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using UnityEngine.AI;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float timeToInvestigate = 10f;
        [SerializeField] float patrolSpeed = 2f;
        [SerializeField] float attackSpeed = 3.5f;
        [SerializeField] PatrolPath patrolPath;


        Vector3 guardLocation;

        float timeSinceLastSawPlayer = Mathf.Infinity;
        int currentIndexWaypoint = 0;

        GameObject player;
        Fighter fighter;
        Health health;
        Move move;
        NavMeshAgent navMeshAgent;

        void Start()
        {
            guardLocation = transform.position;

            player = GameObject.FindGameObjectWithTag("Player");
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            move = GetComponent<Move>();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            if (health.IsDead()) return;

            if (InAttackRangeToPlayer())
            {
                timeSinceLastSawPlayer = 0;
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < timeToInvestigate)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }
            timeSinceLastSawPlayer +=Time.deltaTime;
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            navMeshAgent.speed = attackSpeed;
            fighter.Attack(player);
        }

        void PatrolBehaviour()
        {
            Vector3 nextPosition = guardLocation;
            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }
            navMeshAgent.speed = patrolSpeed;
            move.StartMoveAction(nextPosition);   
        }

        bool AtWaypoint()
        {
            return transform.position == GetCurrentWaypoint();
        }

        void CycleWaypoint()
        {
            currentIndexWaypoint = patrolPath.GetNextIndex(currentIndexWaypoint);    
        }

        Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentIndexWaypoint);
        }

        bool InAttackRangeToPlayer()
        {
            return Vector3.Distance(transform.position, player.transform.position) < chaseDistance;
        }

        //Called by Unity
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
