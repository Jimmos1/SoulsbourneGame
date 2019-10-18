using UnityEngine;
using UnityEngine.UI;

public class HPController : MonoBehaviour
{
    public float hp;

    [SerializeField]
    private float deathDestroyTimer;

    [SerializeField]
    private AttackController playerAttackController;

    [SerializeField]
    private Slider hpSlider;

    void Awake()
    {
        playerAttackController = GameObject.FindGameObjectWithTag("Player").GetComponent<AttackController>();
        hpSlider = GetComponentInChildren<Canvas>().GetComponentInChildren<Slider>();

        hp = 100;
        deathDestroyTimer = 0;
    }

    void Update()
    {
        hpSlider.transform.LookAt(playerAttackController.transform);
        hpSlider.value = hp / 100;

        if (hp <= 0)
        {
            if (playerAttackController.hitGameObjects.Contains(gameObject))
                playerAttackController.hitGameObjects.Remove(gameObject);

            deathDestroyTimer += Time.deltaTime;
        }

        if (deathDestroyTimer > 5.0f)
        {
            Destroy(gameObject);
        }
    }
}