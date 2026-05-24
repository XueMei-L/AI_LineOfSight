using UnityEngine;

public class PlayerBullet : MonoBehaviour
{   
    public GameObject explosion;
    public float damage = 10f;

    void OnCollisionEnter(Collision col)
    {
        GameObject e = Instantiate(explosion, this.transform.position, Quaternion.identity);
        Destroy(e, 1f);

        RobotGuardAI npcAI = col.gameObject.GetComponent<RobotGuardAI>();
        
        if (npcAI != null) 
        {
            // npcAI.health -= damage;
            // Debug.Log($"NPC health: {npcAI.health}");

            npcAI.health = 10f; 
            Debug.Log($"Testing, NPC con vida: {npcAI.health}");
        }

        Destroy(this.gameObject);
    }
}