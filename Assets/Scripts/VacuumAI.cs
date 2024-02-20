using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VacuumAI : MonoBehaviour
{

    private NavMeshAgent agent;
    private Animator anim;

    private List<Transform> trackedPlayers = new List<Transform>();
    private bool isNearPlayer = false;

    [SerializeField] private List<Transform> patrolPoints;
    private int currentPoint = 0;
    private bool isTravellingToPoint = false;

    private void Awake()
    {

        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

    }

    private void Update()
    {

        if(isNearPlayer && PlayerIsVisible(trackedPlayers[0]))
        {
            StopPatrolling();
            ChasePlayer(trackedPlayers[0]);
        }
        else
        {
            Patrol();
        }

    }

    private bool PlayerIsVisible(Transform trackedPlayer)
    {

        RaycastHit hit;
        Physics.Raycast(transform.position, trackedPlayer.position - transform.position, out hit);

        if(hit.collider.CompareTag("Player")) return true;

        return false;

    }

    private void StopPatrolling()
    {

        StopAllCoroutines();
        anim.SetTrigger("Idle");
        currentPoint = -1;
        isTravellingToPoint = false;

    }

    private void ChasePlayer(Transform trackedPlayer)
    {

        agent.SetDestination(trackedPlayer.position);

    }

    private void Patrol()
    {


        if(!isTravellingToPoint)
        {
            isTravellingToPoint = true;

            if(currentPoint < 0) currentPoint = GetClosestPatrolPoint();
            StartCoroutine("TravelToNextPatrolPoint");
        }

    }

    private int GetClosestPatrolPoint()
    {

        float closestDistance = Mathf.Infinity;
        int closestPoint = -1;

        for(int i = 0; i < patrolPoints.Count; i++)
        {

            float distanceToPoint = (transform.position - patrolPoints[i].position).magnitude;
            if(distanceToPoint < closestDistance)
            {
                closestDistance = distanceToPoint;
                closestPoint = i;
            }

        }

        if(closestPoint >= 0 && closestPoint < patrolPoints.Count) return closestPoint;
        return 0;

    }

    private IEnumerator TravelToNextPatrolPoint()
    {

        Vector3 patrolPoint = patrolPoints[currentPoint].position;

        agent.SetDestination(patrolPoint);

        while(Vector3.Distance(patrolPoint, transform.position) >= 0.5f)
            yield return null;

        anim.SetTrigger("Clean");
        yield return new WaitForSeconds(2.13f);

        isTravellingToPoint = false;

        IncrementPatrolPoint();

    }

    private void IncrementPatrolPoint()
    {

        currentPoint++;
        if(currentPoint >= patrolPoints.Count) currentPoint = 0;

    }

    private void OnTriggerEnter(Collider other)
    {

        if(other.CompareTag("Player"))
        {
            trackedPlayers.Add(other.transform);
            isNearPlayer = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {

        if(other.CompareTag("Player"))
        {
            trackedPlayers.Remove(other.transform);
            if(trackedPlayers.Count < 1) isNearPlayer = false;
        }

    }

}
