using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : MonoBehaviour
{
    [Header("AttachedObjects")]
    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject playerWhoShot;
    public MeleeAttack meleeAttack;

    [SerializeField] private float shootCoolDown = 0f;
    [SerializeField] private bool shooting = false;
    public bool gravityGun = false;

    //weapon
    public Weapon currentWeapon;

    public float powerMultiplier = 1;
    public float shootSpeedMultiplier = 1f;
    public float thrust = 1000f;

    private PlayerStats playerStats;
    private AudioSource audioSource;
    public AudioClip pistolSoundClip;
    public AudioClip rocketLauncherSoundClip;
    public AudioClip fistSoundClip;
    public AudioClip machineGunSoundClip;

    [SerializeField] private List<Weapon> completeWeaponsList = new List<Weapon>();
    [SerializeField] private List<Weapon> inventoryWeaponsList = new List<Weapon>();
    public Weapon fists;
    public Weapon machineGun;
    public Weapon pistol;
    public Weapon rocketLauncher;
    public Weapon grenade;

    private int weaponSlot = 0;

    //raycast layermask
    public LayerMask mask;

    //weaponmodels
    public GameObject[] WeaponModels;

    private void Start()
    {

        audioSource = GetComponent<AudioSource>();
        playerStats = playerWhoShot.GetComponent<PlayerStats>();

        CreateWeapons();

        currentWeapon = fists;
        inventoryWeaponsList.Add(currentWeapon);

        Debug.Log(inventoryWeaponsList);
        AdjustParametersForNewWeapon();
    }

    //Update each frame
    private void Update()
    {
        if (shootCoolDown >= 0f)
        {
            shootCoolDown -= Time.deltaTime * shootSpeedMultiplier;
        }

        if (shooting && shootCoolDown <= 0f)
        {
            Shoot();
        }

    }


    public void Shoot()
    {
        if (currentWeapon.ammoLeft > 0f)
        {
            if (currentWeapon.weaponType == "balistic")
            {
                GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                Rigidbody rb = bulletGO.GetComponent<Rigidbody>();
                rb.AddForce(transform.forward * thrust);

                ApplyDamageEffects(bulletGO);

                currentWeapon.Shot(1f);
            }
            else if (currentWeapon.weaponType == "raycast")
            {
                RaycastHit hit;

                if (Physics.Raycast(transform.position, transform.forward, out hit, currentWeapon.shootDistance, mask))
                {
                    //Do Ray Visuals


                }
                GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, hit.point, firePoint.rotation);
                ApplyDamageEffects(bulletGO);
                currentWeapon.Shot(1f);
            }
            else if (currentWeapon.weaponType == "melee")
            {
                meleeAttack.SendAttack(powerMultiplier);
            }
            else
            {
                Debug.LogError("Warning Weapon Type not defined");
            }

            shootCoolDown = currentWeapon.shootCooldown;
            audioSource.clip = currentWeapon.shootSound;
            audioSource.Play();

            AdjustParametersForNewWeapon();
        }

    }

    private void ApplyDamageEffects(GameObject projectile)
    {
        projectile.GetComponent<Explosion>().radius *= currentWeapon.explosionRadius * powerMultiplier;
        projectile.GetComponent<Explosion>().power *= currentWeapon.explosionPower * powerMultiplier;
        projectile.GetComponent<Explosion>().damageMultiplier *= currentWeapon.damage * powerMultiplier;

        projectile.GetComponent<Explosion>().deactivateGravity = gravityGun;
        projectile.GetComponent<Explosion>().playerWhoShot = playerWhoShot;
    }

    private void AdjustParametersForNewWeapon()
    {
        //Sounds für neue Waffe anpassen
        //audioSource.clip = currentWeapon.shootSound;

        //Update Variablen für die Anzeige
        playerStats.ammoLeft = currentWeapon.ammoLeft;
        playerStats.weaponName = currentWeapon.weaponName;
        playerStats.UpdateStats();
    }

    public void Shooting()
    {
        shooting = true;
    }

    public void ShootingStopped()
    {
        shooting = false;
    }

    public void SwitchWeapon()
    {
        weaponSlot += 1;
        if (weaponSlot >= inventoryWeaponsList.Count)
        {
            weaponSlot = 0;
        }

        currentWeapon = inventoryWeaponsList[weaponSlot];
        AdjustParametersForNewWeapon();


        foreach (GameObject WeaponModel in WeaponModels)
        {
            if (currentWeapon.weaponName == WeaponModel.name)
            {
                WeaponModel.SetActive(true);
            }
            else
            {
                WeaponModel.SetActive(false);
            }
        }

    }

    public void OnItemPickup(string itemName)
    {
        //wenn wenigstens ein item gefunden wird mit diesem namen, dann handelt es sich um eine waffe
        bool itsAWeapon = false;

        //neue waffe definieren
        Weapon newWeapon = currentWeapon;
        //überprüfen ob der übergebene string einer Waffe entspricht die in der Gesammtwaffenliste steht
        for (int i = 0; i < completeWeaponsList.Count; i++)
        {

            if (completeWeaponsList[i].weaponName == itemName)
            {
                //man hat die richtige waffe gefunden...
                newWeapon = completeWeaponsList[i];
                //...und gleichzeitig bewiesen das es wenigstens eine waffe gibt, die dem übergebenen string entspricht
                itsAWeapon = true;

            }
        }
        //wenn immernoch atLeastOnce == false dann handelt es sich ncht um eine waffe
        if (itsAWeapon == false)
        {
            //Do: was auch immer passieren soll, wenn es sich bei dem string nicht um einen waffennahmen halndelt

        }
        else  //its a weapon!!!!
        {

            foreach (Weapon item in inventoryWeaponsList)
            {
                if (item.weaponName == itemName)
                {
                    item.pickUpAmmo();
                    AdjustParametersForNewWeapon();
                    return;
                }
            }

            inventoryWeaponsList.Add(newWeapon);

        }

    }

    private void CreateWeapons()
    {
        completeWeaponsList.Add(fists = new Weapon("Fists", "melee", 9999, 1f, 1f, fistSoundClip, 0.1f, 5f, 5f));
        completeWeaponsList.Add(machineGun = new Weapon("MachineGun", "balistic", 1500, 0.15f, 100f, machineGunSoundClip, 0.1f, 1f, 1f));
        completeWeaponsList.Add(pistol = new Weapon("Pistol", "balistic", 200, 1.5f, 100f, pistolSoundClip, 1f, 1f, 4f));
        completeWeaponsList.Add(rocketLauncher = new Weapon("RocketLauncher", "balistic", 16, 3f, 100f, rocketLauncherSoundClip, 4.5f, 4.5f, 20f));
        completeWeaponsList.Add(grenade = new Weapon("Grenade", "balistic", 16, 50f, 100f, rocketLauncherSoundClip, 5f, 5f, 5f));
    }

}
