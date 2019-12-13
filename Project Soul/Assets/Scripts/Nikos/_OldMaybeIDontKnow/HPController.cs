using UnityEngine;
using UnityEngine.UI;

public class HPController : MonoBehaviour
{
    public float hp;

    [SerializeField]
    private AttackController playerAttackController;

    [SerializeField]
    private Slider hpSlider;

    private float deathDestroyTimer;

    void Awake()
    {
        playerAttackController = GameObject.FindGameObjectWithTag("Player").GetComponent<AttackController>();
        hpSlider = GetComponentInChildren<Canvas>().GetComponentInChildren<Slider>();

        hp = 100;
        deathDestroyTimer = 0;
    }

    void Update()
    {
        // Update the HP Slider
        hpSlider.transform.LookAt(playerAttackController.transform);
        hpSlider.value = hp / 100;

        // TEMPORARY Death Destroy
        if (hp <= 0)
        {
            deathDestroyTimer += Time.deltaTime;
        }
        if (deathDestroyTimer > 3.0f)
        {
            Destroy(gameObject);
        }
    }

    public void GotHit(float damage)
    {
        hp -= damage;
    }
}