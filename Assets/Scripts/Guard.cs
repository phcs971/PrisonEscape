using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Guard : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator animator;

    private GameManager manager;

    public Cell currentDestination;

    public Coroutine doCheck;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        manager = GameObject.FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        CanSeePlayer();
    }

    public void CanSeePlayer()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, 5))
        {
            if (hit.transform.CompareTag("Player"))
            {
                manager.GuardDidSeePlayer();
            }
        }
    }

    public void MoveTo(Cell cell)
    {
        currentDestination = cell;
        if (agent == null) { Start(); }
        agent.SetDestination(currentDestination.destination.position);
        animator.SetTrigger("Walk");
        doCheck = StartCoroutine(DoCheck());
    }

    bool ReachedDestination()
    {
        var distance = Vector3.Distance(transform.position, currentDestination.destination.position);
        return distance < 0.9f; 
    }

    void DidReachDestination()
    {
        StopCoroutine(doCheck);
        animator.SetTrigger("Look");
    }

    public void DidFinishLook()
    {
        manager.DeployGuard(this);
    }

    IEnumerator DoCheck()
    {
        for( ; ; )
        {
            if (ReachedDestination())
            {
                DidReachDestination();
            }
            yield return new WaitForSeconds(.1f);
        }
    }
}
