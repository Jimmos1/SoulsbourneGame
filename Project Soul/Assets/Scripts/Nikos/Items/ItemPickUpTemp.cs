using UnityEngine;

public class ItemPickUpTemp : MonoBehaviour
{
    [SerializeField]
    private InventoryManager inventoryManager;

    private void Awake()
    {
        inventoryManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<InventoryManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.tag == "Item" && other.tag == "Player")
        {
            Debug.Log("Picked up " + this.name);

            inventoryManager.AddItem(this.gameObject);
            Destroy(gameObject);
        }
    }
}