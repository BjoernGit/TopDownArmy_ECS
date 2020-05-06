using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public float damageMultiplier = 1f;
    public float powerMultiplier = 1000f;

    //public List<GameObject> collidingGOList = new List<GameObject>();

    public bool m_started;

    private Collider thisPlayerColi;
    private Collider fightBoxColi;

    private void Start()
    {
        thisPlayerColi = transform.parent.gameObject.GetComponent<Collider>();
        fightBoxColi = gameObject.GetComponent<Collider>();
    }
    public void SendAttack(float powerMultiplier)
    {
        //check for colliders in the fighting box
        Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale / 2, transform.rotation);

        //add all found colliders gameobjects to a list
        //foreach (Collider coli in colliders)
        //{
        //    if (colliders != null && coli != thisPlayerColi && coli != fightBoxColi)
        //    {
        //        collidingGOList.Add(coli.gameObject);

        //    }
        //    else
        //    {
        //        Debug.Log("fuck is null");
        //    }
        //}
        //Debug.Log(collidingGOList);

        //attack all containing gameobjects
        foreach (Collider coli in colliders)
        {
            if (coli.gameObject.CompareTag("Player") && coli != thisPlayerColi && coli != fightBoxColi)
            {
                coli.gameObject.GetComponent<PlayerStats>().TakeDamage(damageMultiplier * powerMultiplier);
                coli.gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * powerMultiplier * this.powerMultiplier);
            }
            else if (coli.gameObject.GetComponent<Rigidbody>() != null && coli != thisPlayerColi && coli != fightBoxColi)
            {
                coli.gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * powerMultiplier * this.powerMultiplier);
            }

            if (coli.gameObject.GetComponent<ZombieController>() != null)
            {
                //zombie check!
                coli.gameObject.GetComponent<ZombieController>().TakeDamage(damageMultiplier * powerMultiplier);
            }
        }
    }

}
