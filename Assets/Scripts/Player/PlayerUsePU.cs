using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerUsePU : MonoBehaviour
{

    public RectTransform pickedPUText;
    public string pickedPU="";
    //WeaponManager manager;
    //WeaponManager cloneManager;
    public Transform clonePrefab;
    Rigidbody rigidBody;

    void Start ()
    {
        //manager = GetComponent<WeaponManager>();
        rigidBody = GetComponent<Rigidbody>();
    }
	

	void Update ()
    {
        //CONTROLLO PU UTILIZZABILE
        pickedPUText.GetComponent<Text>().text = pickedPU;
        if (pickedPU!="")
        {
            if (Input.GetMouseButtonDown(2))
            {
                switch (pickedPU)
                {
                    case "CloneTrap":
                        rigidBody.AddForce(-transform.forward * 5, ForceMode.Impulse);
                        Instantiate(clonePrefab, transform.position, transform.rotation);
                        //cloneManager = GameObject.FindGameObjectWithTag("PlayerClone").transform.GetComponent<WeaponManager>();
                        //cloneManager.ChangeWeapon(manager.currentWeapon);
                        pickedPU = "";
                        break;
                }
            }
        }
    }
}
