using UnityEngine;
using System.Collections;

public class PowerUpState : MonoBehaviour 
{
    /*
    Power up di stato. 
    State=Health,Speed;
    */
    
    public string type;
    public float minValue;
    public float maxValue;
    private float value;
    public float timer;
    private bool timerStarted = false;
    private float oldValue;
    GameObject player;
    Health health;
    PlayerController controller;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        health = player.transform.GetComponent<Health>();
        controller = player.transform.GetComponent<PlayerController>();
    }

    void Update()
    {
        if (timerStarted)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                switch (type)
                {
                    case "Health":
                        health.currentHealth = oldValue;
                        break;
                    case "Speed":
                        controller.speed = oldValue;
                        break;
                    default:
                        health.currentHealth = oldValue;
                        break;
                }
                GameObject.Destroy(gameObject);
            }
        }

    }

    void OnTriggerEnter(Collider collider)
    {
        value = Random.Range(minValue, maxValue);
        if (collider.gameObject.name == "Player")
        {
            if (timer != 0)
                timerStarted = true;
            else
                Destroy(gameObject);

            switch (type)
            {
                case "Health":
                    oldValue = health.currentHealth;
                    health.currentHealth += value;
                    break;
                case "Speed":
                    oldValue = controller.speed;
                    controller.speed += value;
                    break;
                default:
                    oldValue = health.currentHealth;
                    health.currentHealth += value;
                    break;
            }
            transform.GetChild(0).GetComponent<Renderer>().enabled = false;
            //transform.GetChild(1).GetComponent<Renderer>().enabled = false;
            gameObject.GetComponent<Collider>().enabled = false;
            GetComponent<DestroyAfterTime>().enabled = false;
        }
    }
}
