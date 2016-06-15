using UnityEngine;
using System.Collections;

public class SkifBall : MonoBehaviour {

    public float timer;
    public float radius;
    public float explosionForce;
    public Transform skifDecal;

	void Start ()
    {
        Destroy(gameObject, timer);
	}

    void OnCollisionEnter(Collision col)
    {
        Collider[] colliders = Physics.OverlapSphere(col.contacts[0].point, radius); 
        foreach(Collider c in colliders)
        {
            if (!c.GetComponent<Rigidbody>()) continue;
            c.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, col.contacts[0].point, radius, 1, ForceMode.Impulse);
        }

        if(col.collider.tag=="Floor")
        {
            Quaternion hitRotation = Quaternion.FromToRotation(Vector3.up, col.contacts[0].normal);
            Vector3 point = col.contacts[0].point;
            point.y += 0.1f;
            Instantiate(skifDecal, point, hitRotation);
        }

        Destroy(gameObject);
    }
}
