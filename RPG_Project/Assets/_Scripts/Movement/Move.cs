using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.Movement
{
    public class Move : MonoBehaviour, IAction
    {
        [SerializeField] Transform target;

        NavMeshAgent navMeshAgent;
        Animator animator;
        [HideInInspector] public bool isAtDestination;

        void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination)
        {

            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination);
            isAtDestination = Vector3.Distance(transform.position, destination) < 0.1f;
        }

        public void MoveTo(Vector3 destination)
        {
            
            navMeshAgent.destination = destination;
            navMeshAgent.isStopped = false;
         }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

        void UpdateAnimator()
        {
            Vector3 velocity = GetComponent<NavMeshAgent>().velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            animator.SetFloat("forwardSpeed", speed);
        }

    }
}
