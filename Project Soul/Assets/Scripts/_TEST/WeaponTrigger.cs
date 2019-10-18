using UnityEngine;

public class WeaponTrigger : MonoBehaviour
{
    [SerializeField]
    private AttackController playerAttackController;

    void Awake()
    {
        playerAttackController = GameObject.FindGameObjectWithTag("Player").GetComponent<AttackController>();
    }

    private void OnTriggerEnter(Collider hit)
    {
        if (hit.tag == "Enemy" && hit.GetComponent<HPController>().hp > 0)
        {
            playerAttackController.hitGameObjects.Add(hit.gameObject);
        }
    }

    private void OnTriggerExit(Collider hit)
    {
        if (hit.tag == "Enemy")
        {
            playerAttackController.hitGameObjects.Remove(hit.gameObject);
        }
    }
}