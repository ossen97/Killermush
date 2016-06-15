using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public bool canRotate = true;
    public bool canMove = true;
    int floorMask;
    Vector3 lookPos;
    GameObject[] enemies;
    Rigidbody rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        floorMask = LayerMask.GetMask("Floor");
    }

    void Update()
    {
        //Effettuo un raycast verso il pavimento di gioco dalla posizione del mouse
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(camRay, out hit, 100, floorMask))
        {
            lookPos = hit.point;
        }

        //Calcolo la posizione verso la quale deve girarsi il player e azzero la rotazione sull'asse y (rotazione verticale)
        Vector3 lookDir = lookPos - transform.position;
        lookDir.y = 0;
        if (canRotate)
            transform.LookAt(transform.position + lookDir, Vector3.up);
    }

    //Funzione di movimento che "spinge" il player in base al vettore che gli si passa e alla velocità impostata
    public void Move(Vector3 _movement)
    {
        rigidBody.AddForce(_movement * speed / Time.deltaTime);
    }

    //Alla distruzione (morte), imposta il target dei nemici su se stessi in quanto non c'è più un player in campo
    void OnDestroy()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            enemy.SendMessage("ChangeTarget", enemy.transform, SendMessageOptions.DontRequireReceiver);
        }
    }
}