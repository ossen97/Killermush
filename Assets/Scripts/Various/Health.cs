using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Health : MonoBehaviour
{
    public float maxHealth;
    public float coolDownTimer; //Tempo tra una riduzione della vita e un'altra  
    public float currentHealth;
    public float resistance;
    public bool deactiveOnDeath=false;
    public bool hideOnDeath = false;
    private bool onCD; //Determina se si sta attendendo il passare del tempo di attesa tra una riduzione della vita e un'altra  
    public bool damageable=true; 
    private DropPowerUp drop;
    private bool dropped=false;
    public float healthBarShowTime; //Indica quanti secondi rimarrà visibile la healthbar posizionata sulla testa dell'oggetto interessato dopo una riduzione della vita
    public Transform healthBarCanvas;
    public Image healthBarImage;

    void Awake()
    {
        drop = GetComponent<DropPowerUp>();
        if(GetComponentInChildren<Canvas>())
            healthBarCanvas = GetComponentInChildren<Canvas>().transform;
        resistance = 1;
    }

    void Start()
    {
        currentHealth = maxHealth;
        if(healthBarShowTime>0)
            healthBarCanvas.gameObject.SetActive(false);
    }

    void Update()
    {
        currentHealth=Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBarImage.fillAmount = currentHealth / maxHealth;
    }
    
    IEnumerator CoolDownDmg()
    {
        onCD = true;
        yield return new WaitForSeconds(coolDownTimer);
        onCD = false;
    }
    
    IEnumerator ShowHealthBar()
    {
        healthBarCanvas.gameObject.SetActive(true);
        yield return new WaitForSeconds(healthBarShowTime);
        healthBarCanvas.gameObject.SetActive(false);
    }

    /// <summary>
    /// Applica una riduzione della vita dell'oggetto colpito, se esso ne ha una
    /// </summary>
    /// <param name="damage">Vita da sottrarre</param>
    public void Damage(float damage)
    {
        if (!onCD && currentHealth > 0 && damageable)
        {
            StartCoroutine(CoolDownDmg());
            currentHealth -= damage/resistance;
        }
        if (currentHealth <= 0)
        {
            if (drop!=null && dropped==false)
            {
                drop.Drop();
                dropped = true;
            }

            if (!deactiveOnDeath && !hideOnDeath)
                Destroy(gameObject);
            else if (deactiveOnDeath)
                gameObject.SetActive(false);
            else if (hideOnDeath)
            {
                transform.GetComponent<MeshRenderer>().enabled = false;
                transform.GetComponent<Collider>().enabled = false;
                for(int i=0;i<transform.childCount; i++)
                {
                    if(transform.GetChild(i).GetComponent<MeshRenderer>())
                        transform.GetChild(i).GetComponent<MeshRenderer>().enabled = false;
                    if(transform.GetChild(i).GetComponent<Collider>())
                        transform.GetChild(i).GetComponent<Collider>().enabled = false;
                    if (transform.GetChild(i).transform)
                        transform.GetChild(i).transform.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            if (healthBarShowTime > 0)
                StartCoroutine(ShowHealthBar());
        }
    }
}
