using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bullet;
    public Transform muzzle;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }
    
    void Shoot()
    {
        GameObject b = Instantiate(bullet, muzzle.transform.position, muzzle.rotation);
        
        b.GetComponent<Rigidbody>().AddForce(muzzle.transform.forward * 500f);
        
        Destroy(b, 4f);
    }
}