using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotGuardAI : MonoBehaviour
{
    Animator anim;
    [HideInInspector] public NavMeshAgent agent;
    public GameObject target;
    public GameObject bullet;
    public GameObject muzzle;

    [Header("--- NPC settings ---")]
    public float health = 100f;
    public float maxHealth = 100f;

    [Header("--- Visibility setting ---")]
    public float visDist = 30.0f;     
    public float visAngle = 60.0f;    

    [Header("--- Wander Setting ---")]
    public float wanderRadius = 2f;
    public float wanderDistance = 8f;
    public float wanderJitter = 0.2f;
    private Vector3 wanderTarget = Vector3.zero;


    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = this.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("distance", Vector3.Distance(transform.position, target.transform.position));

        anim.SetBool("isVisible", CanSeeTarget());

        anim.SetFloat("health", health);
    }

    // getter for player
    public GameObject GetPlayer()
    {
        return target;
    }

    // check if the target is within line of sight
    public bool CanSeeTarget()
    {
        // RaycastHit raycastInfo;
        // Vector3 rayToTarget = target.transform.position - this.transform.position;
        // if (Physics.Raycast(this.transform.position, rayToTarget, out raycastInfo))
        // {
        //     if (raycastInfo.transform.gameObject.tag == "Player")
        //         return true;
        // }
        // return false;
        Vector3 direction = target.transform.position - this.transform.position;
        float angle = Vector3.Angle(direction, this.transform.forward);

        if (direction.magnitude < visDist && angle < visAngle)
        {
            direction.y = 0;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 2.0f);
            return true;
        }
        return false;
    }

    
    // methed seek 
    private void Seek(Vector3 location)
    {
        agent.SetDestination(location);
    }

    // State 1, Patrol - Wander
    public void Wander()
    {
        wanderTarget += new Vector3(Random.Range(-1.0f, 1.0f) * wanderJitter,
                                        0,
                                        Random.Range(-1.0f, 1.0f) * wanderJitter);
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;
        
        Debug.DrawRay(this.transform.position, this.gameObject.transform.TransformVector(wanderTarget),Color.red);
        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);

        Debug.DrawRay(this.transform.position+this.gameObject.transform.TransformVector(wanderTarget), this.gameObject.transform.TransformVector(new Vector3(0, 0, wanderDistance)),Color.green);
        Vector3 targetWorld = this.transform.position + this.gameObject.transform.TransformVector(targetLocal);
        

        Seek(targetWorld);
    }

    // State 2, Chase - Pursue
    public void Pursue()
    {
        Vector3 targetDir = target.transform.position - this.transform.position;

        float lookAhead = targetDir.magnitude * 3 / agent.speed;
        // Debug.DrawRay(target.transform.position, target.transform.forward * lookAhead,Color.red);
        // Debug.DrawRay(this.transform.position, target.transform.position + target.transform.forward * lookAhead - this.transform.position,Color.green);
        Seek(target.transform.position + target.transform.forward * lookAhead);
    }


    // State 4, Hide - Clever Hide
    public void CleverHide()
    {
        float dist = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;
        Vector3 chosenDir = Vector3.zero;
        GameObject chosenGO = World.Instance.GetHidingSpots()[0];

        for (int i = 0; i < World.Instance.GetHidingSpots().Length; i++)
        {
            Vector3 hideDir = World.Instance.GetHidingSpots()[i].transform.position - target.transform.position;
            hideDir.y = 0.0f;
            Vector3 hidePos = World.Instance.GetHidingSpots()[i].transform.position + hideDir.normalized * 100;
            

            if (Vector3.Distance(this.transform.position, hidePos) < dist)
            {
                chosenSpot = hidePos;
                chosenDir = hideDir;
                chosenGO = World.Instance.GetHidingSpots()[i];
                dist = Vector3.Distance(this.transform.position, hidePos);
            }
        }

        Collider hideCol = chosenGO.GetComponent<Collider>();
        Ray backRay = new Ray(chosenSpot, -chosenDir.normalized);
        RaycastHit info;
        float distance = 250.0f;
        hideCol.Raycast(backRay, out info, distance);
        Debug.DrawRay(chosenSpot, -chosenDir.normalized * distance, Color.red);

        Seek(info.point + chosenDir.normalized);
    }

    // State 4, Hide - Regenerate Health
    public void RegenerateHealth()
    {
        if (health < maxHealth)
        {
            health += 10 * Time.deltaTime;
            health = Mathf.Min(health, maxHealth);
        }
    }

    // State 3, Attack - Fire
    void Fire()
    {
        GameObject b = Instantiate(bullet, muzzle.transform.position, muzzle.transform.rotation);
        b.GetComponent<Rigidbody>().AddForce(muzzle.transform.forward * 500f);
        Destroy(b, 5f);
    }
    public void StopFiring()
    {
        CancelInvoke("Fire");
    }
    public void StartFiring()
    {
        InvokeRepeating("Fire", 0.5f, 0.5f);
    }
}