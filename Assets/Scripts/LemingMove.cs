using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LemingMove : MonoBehaviour
{
    [SerializeField] private int speed = 5;
    [SerializeField] private int randomtimeMin = 2;
    [SerializeField] private int randomtimeMax = 5;
    [SerializeField] private Rigidbody2D rb;
    private float randTime = 2.0f;
    private float timeElapsed = 0.0f;
    private Vector3 currentTarget = Vector3.zero;
    private int targetPriority = 0;
    private bool isScared = true;

    void Start()
    {
        currentTarget = transform.position;
    }
    void Update()
    {

        foreach(Target target in FindObjectsOfType<Target>()/*.Select(t => t.transform.position)*/)
        {
            if (!(target.getPriority() < targetPriority))
            {
                float disToNewTarget = Vector3.Distance(transform.position, target.transform.position);
                //check if leming can see target
                if (disToNewTarget > target.getPriority())
                {
                    continue;
                }

                RaycastHit2D hit = Physics2D.Raycast(transform.position, (target.transform.position - transform.position));
                //check if there is a obstical between leming and target
                if (hit.collider.gameObject != target.gameObject)
                {
                    continue;
                }

                //checks to see if new target has a higher priority or if it is closer
                if(target.getPriority() > targetPriority)
                {
                    currentTarget = target.transform.position;
                    targetPriority = target.getPriority();
                    isScared = false;
                    Debug.Log(target.name);
                }
                else
                {
                    float disToCurTarget = Vector3.Distance(transform.position, currentTarget);
                    if(disToNewTarget < disToCurTarget)
                    {
                        currentTarget = target.transform.position;
                        targetPriority = target.getPriority();
                        isScared = false;
                        Debug.Log(target.name);
                    }
                }
            }
        }
        //end foreach
        //move leming

        if (currentTarget == transform.position)
        {
            currentTarget = new Vector3(transform.position.x + (Random.Range(-1, 2) * 100), transform.position.y + (Random.Range(-1, 2) * 100), 0);
            randTime = Random.Range(randomtimeMin, randomtimeMax);
            timeElapsed = 0.0f;
            isScared = true;
        }

        //transform.position = Vector2.MoveTowards(transform.position, currentTarget, Time.deltaTime * speed);
        Vector2 dir = new Vector2(currentTarget.x - transform.position.x , currentTarget.y - transform.position.y);
        rb.velocity = dir.normalized * speed;

        if (timeElapsed < randTime && isScared)
        {
            timeElapsed += Time.deltaTime;
        }
        else
        {
            currentTarget = transform.position;
            targetPriority = 0;
        }

    }
}
