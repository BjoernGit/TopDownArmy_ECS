using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CountdownOnCollision : MonoBehaviour
{
    public UnityEvent countdownEvents;
    public float counter = 5f;
    public Text counterText;

    private bool colliding = false;

    private void Awake()
    {
        GameManager.instance.RequestPlayerJoinEnabled();
    }

    private void OnCollisionEnter(Collision collision)
    {
        colliding = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        colliding = false;
    }

    private void Update()
    {
        if (counter >= 0 && colliding)
        {
            counter -= Time.deltaTime;
            counterText.text = (Mathf.Round(counter * 100f)/100f + "s");
        }
        else if(counter < 0)
        {
            countdownEvents.Invoke();
            GameManager.instance.RequestPlayerJoinDisabled();
        }
    }
}
