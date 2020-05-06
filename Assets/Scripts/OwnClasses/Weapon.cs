using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    public string weaponName;
    public string weaponType;
    public int maxAmmo;
    public float shootCooldown;
    public float shootDistance;
    public AudioClip shootSound;
    public float explosionRadius;
    public float explosionPower;
    public float damage;

    public float ammoLeft;

    public Weapon(string _weaponName, string _weaponType, int _maxAmmo, 
        float _shootCooldown, float _shootDistance, AudioClip _shootSound, float _explosionRadius, 
        float _explosionPower, float _damage)
    {
        weaponName = _weaponName;
        weaponType = _weaponType;
        maxAmmo = _maxAmmo;
        shootCooldown = _shootCooldown;
        shootDistance = _shootDistance;
        shootSound = _shootSound;
        explosionRadius = _explosionRadius;
        explosionPower = _explosionPower;
        damage = _damage;

        ammoLeft = _maxAmmo/4;
    }

    public void Shot(float ammo)
    {
        ammoLeft -= ammo;
        Mathf.Clamp(ammoLeft, 0f, maxAmmo);
    }

    public void pickUpAmmo()
    {
        ammoLeft += maxAmmo / 4;
    }
}
