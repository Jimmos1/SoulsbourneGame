//using UnityEngine;

//public class WeaponHook : MonoBehaviour
//{
//    private enum Weapons { None, Maul, Dagger };

//    [SerializeField]
//    private Weapons weapon;
//    private Weapons currentWeapon;

//    private GameObject maul;
//    private GameObject dagger;

//    void Awake()
//    {
//        maul = GameObject.Find("Maul Weapon");
//        dagger = GameObject.Find("Dagger Weapon");
//        maul.SetActive(false);
//        dagger.SetActive(false);

//        weapon = Weapons.None;
//        currentWeapon = Weapons.None;
//    }

//    // Change Weapon
//    void Update()
//    {
//        if (currentWeapon != weapon)
//        {
//            maul.SetActive(false);
//            dagger.SetActive(false);

//            if (weapon == Weapons.Maul)
//            {
//                maul.SetActive(true);
//                PlaceWeaponOnHand(maul);
//            }
//            else if (weapon == Weapons.Dagger)
//            {
//                dagger.SetActive(true);
//                PlaceWeaponOnHand(dagger);
//            }

//            currentWeapon = weapon;
//        }
//    }

//    public void PlaceWeaponOnHand(GameObject weapon)
//    {
//        weapon.transform.position = this.transform.position;
//        weapon.transform.rotation = this.transform.rotation;

//        weapon.transform.parent = this.transform;
//    }
//}