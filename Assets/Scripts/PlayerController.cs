using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Vector2 i_movement;
    Vector2 r_movement;

    [Header("General Settings")]
    public bool levelSelectControls = false;
    public float moveSpeed = 50f;
    public float rotateSpeed = 10f;
    public float jumpSpeed = 5f;
    public float thrust = 1000f;
    public float maxVelocity = 10f;
    public int jumpsLeftStart = 2;
    public float walkBackwardsVelocityMultiplier = 0.25f;
    private float reverencedMaxVelocity;

    [Header("Attached Objects")]
    public Transform firePoint;
    public GameObject playerBody;
    public GameObject healthBarAnchor;
    private FireBullet FPfireBullet;

    [Header("Powerups")]
    public bool increasePower;
    public bool increaseJumpsActive;
    public bool increaseMass;
    public bool increaseShootSpeed;

    private float powerMultiplier = 1f;
    private float increaseJumpsMultiplier = 1f;
    private float increaseMassMultiplier = 1f;

    [Header("Sounds")]
    public AudioClip shootSoundClip;
    public AudioClip fallOfCliffClip;
    private AudioSource audioSource;

    //private internal Variables
    private Rigidbody rb;
    private bool onGround;
    private int playerNumber;
    private Color playerColor;
    private int jumpsLeft = 2;

    [Header("SelectionLevelVariables")]
    Vector2 selection_movement;
    Vector2 selectionPosition = new Vector2(0f, 0f);
    float threshold = 0.8f;
    float stepSize = 2f;
    bool selectionMoved = false;
    public LayerMask levelSelectLayerMask;

    //debug variable
    public float debugCounter;
    public GameObject debugPrefab;

    private void Awake()
    {
        SpawnManager.instance.AssignPlayer(this.gameObject);
        playerNumber = SpawnManager.instance.RequestPlayerNumber(gameObject);
        playerColor = SpawnManager.instance.RequestPlayerColor(playerNumber);

        rb = this.GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        FPfireBullet = firePoint.GetComponent<FireBullet>();

        //ECS Code
        ECS_AppManager.instance.AddPlayerEntity();
    }

    // Start is called before the first frame update
    void Start()
    {

        ResetPlayer();
        SetColor();

    }

    //---InputSection-------------------------------------
    private void OnShoot()
    {

        if (levelSelectControls)
        {
            SelectionSelect();
            return;
        }

        FPfireBullet.Shooting();
    }

    private void SelectionSelect()
    {
        //In der Mitte des selectionsramens wird ein Sendmessage gestartet
        Collider[] hitColliders = Physics.OverlapSphere(GameObject.Find("SelectArea").transform.GetChild(0).transform.position, 0.5f, levelSelectLayerMask);
        if (hitColliders.Length > 1)
        {
            Debug.LogError("Too many Colliders detected from selection!");
            foreach (Collider collider in hitColliders)
            {
                Debug.Log("colliderNames: " + collider.name);
            }
        }
        else if (hitColliders.Length != 0)
        {
            hitColliders[0].SendMessage("StartLevel");
        }
        else
        {
            Debug.Log("No Level to start");
        }

    }

    private void OnShootRelease()
    {
        if (levelSelectControls)
            return;

        FPfireBullet.ShootingStopped();
    }

    private void OnMove(InputValue value)
    {
        if (levelSelectControls)
        {
            SelectionMove(value);
            return;
        }

        i_movement = value.Get<Vector2>();
    }

    private void SelectionMove(InputValue value)
    {
        //Do selection Move
        GameObject.Find("ChooseYourLevel").GetComponent<ChooseYourLevel>().MoveSelecter(value);
    }

    private void CheckSelectionPosition()
    {
        //überprüfe ob der rahmen noch im sichtaberen bereich ist. ansonsten teleportiere
        //den Rahmen an die ander Seite des Rahmenbereiches.
        Transform selecterTransform = GameObject.Find("SelectArea").transform.GetChild(0);

        if (selectionPosition.x <= -2f * stepSize)
        {
            selectionPosition.x += 3f * stepSize;
            selecterTransform.transform.Translate(3f * stepSize, 0f, 0f);
        }
        if (selectionPosition.x >= 2f * stepSize)
        {
            selectionPosition.x -= 3f * stepSize;
            selecterTransform.transform.Translate(-3f * stepSize, 0f, 0f);
        }

        if (selectionPosition.y <= -2f * stepSize)
        {
            selectionPosition.y += 3f * stepSize;
            selecterTransform.transform.Translate(0f, 3f * stepSize, 0f);
        }
        if (selectionPosition.y >= 2f * stepSize)
        {
            selectionPosition.y -= 3f * stepSize;
            selecterTransform.transform.Translate(0f, -3f * stepSize, 0f);
        }

    }

    private void OnRotate(InputValue value)
    {
        if (levelSelectControls)
            return;

        r_movement = value.Get<Vector2>();
    }

    private void OnJump()
    {
        if (levelSelectControls)
            return;

        Jump();
    }

    private void OnSwitchWeapon()
    {
        if (levelSelectControls)
            return;

        FPfireBullet.SwitchWeapon();
    }



    // Update is called once per frame ------------------
    void Update()
    {
        Move();

        // Player fällt in den Abgrund und stirbt
        if (transform.position.y <= -10f)
        {
            GetComponent<PlayerStats>().DeathByFalling();
            audioSource.clip = fallOfCliffClip;
            audioSource.Play();
        }

    }

    private void SetColor()
    {
        GetComponent<Renderer>().material.SetColor("_Color", playerColor);
    }

    public void MoveToStartPosition()
    {
        transform.position = SpawnManager.instance.GetNewPosition();
    }

    public void ResetPlayer()
    {
        //Diese Funktion setzt alle einstellungen zurück um ein neues Level mit den bestehenden Spielern zu starten.
        jumpsLeft = jumpsLeftStart;

        MoveToStartPosition();

        ResetCam();

        //Reset Lifes, Powerups, Healthpoints etc.
        GetComponent<PlayerStats>().ResetPlayerStats();

    }

    private void ResetCam()
    {
        healthBarAnchor.GetComponent<RotateToCamera>().FindNewCam();
    }

    private void Move()
    {
        Vector3 movement = new Vector3(i_movement.x, 0, i_movement.y) * moveSpeed * Time.deltaTime;
        Vector3 currentFlatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        ///---Rotation-----------------------------------------------------------

        //rotieren in Grad
        //transform.Rotate(Vector3.up, Time.deltaTime * rotateSpeed * r_movement.x);

        // Determine which direction to rotate towards
        Vector3 targetDirection = new Vector3(r_movement.x, 0, r_movement.y);
        float singleStep = rotateSpeed * Time.deltaTime;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        transform.rotation = Quaternion.LookRotation(newDirection);



        ///---XY-Bewegung--------------------------------------------------------

        //NEU: Geschwindigkeit abhängig von der Blickrichtung anpassen mit Hilfe der Veränderung der "maxVelocity"

        // Umrechnen der maxVelocity auf eine reverenz Velocity die 1x fach wirkt wenn man in die Richtung geht in die man schaut
        // und bis auf 0.25x reduziert wird wenn man in die entgegengesetzte Richtung läuft als zu der man schaut.

        //Errechnen des Winkels zwischen Blick- und Laufrichtung
        float angleMovementToForward = Vector3.Angle(transform.forward, movement);
        //projezieren des Winkels auf eine Range von 0 bis 1
        float projectedAngle = Mathf.InverseLerp(0f, 180f, angleMovementToForward);
        //Nutzen des projezierten Winkels für eine projektion auf die range von 1 bis 0.25 (bzw. wBVMulti)
        float velocityProjection = Mathf.Lerp(1f, walkBackwardsVelocityMultiplier, projectedAngle);
        //zuweisen der Reverenz Velocity
        reverencedMaxVelocity = maxVelocity * velocityProjection;

        //wenn der Bewegungsvektor PLUS der InputValue addiert einen kleineren Vektor ergeben als den Bewegungsvektor 
        //ODER die maxVelocity nicht überschritten is dann soll die Bewegung zugelassen werden
        Vector3 checkMovementVektor = currentFlatVelocity + movement;
        if ((checkMovementVektor.magnitude <= reverencedMaxVelocity) || currentFlatVelocity.magnitude <= reverencedMaxVelocity)
        {
            rb.AddForce(movement * increaseMassMultiplier, ForceMode.Impulse);
        }

        //falls der player schon über dem maxspeed is sollte der vektor nicht verlängert, aber die richtung gemäss dem input value verändert werden
        else if (rb.velocity.magnitude >= reverencedMaxVelocity)
        {
            //berechnung des differenzvektors von momentanem weg und neuem wunsch weg && Anschliessend AddForce
            //den velocity vektor selbst direkt zu verändern könnte physikprobleme verursachen
            Vector3 deltaVector = movement.normalized * reverencedMaxVelocity - currentFlatVelocity;
            rb.AddForce(deltaVector * .1f, ForceMode.Impulse);
        }

        //wenn der InputValue nahe null ist soll der Charakter stark gebremst werden ABER nicht unter ignorrierung der kräfte von aussen. Explosionen sollen trotzdem einen einfluss haben
        if (i_movement.magnitude <= 0.1f && rb.velocity.magnitude >= .2f)
        {
            rb.velocity -= currentFlatVelocity.normalized * Time.deltaTime * 30f;
        }

    }

    private void Jump()
    {
        if (jumpsLeft > 0f)
        {
            rb.AddForce(new Vector3(0, 1 * increaseMassMultiplier, 0) * jumpSpeed, ForceMode.Impulse);
            jumpsLeft--;
            onGround = false;
        }
    }

    public void ResetJumps()
    {

        jumpsLeft = jumpsLeftStart;

        if (increaseJumpsActive)
        {
            jumpsLeft += (int)increaseJumpsMultiplier;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            ResetJumps();
            onGround = true;
        }
    }

    //---PowerUpFunctions------------------------------

    public void PowerUpIncreasePower(float duration, float multiplier)
    {
        StartCoroutine(PowerUpIncreasePowerRoutine(duration, multiplier));
    }

    public void PowerUpIncreaseJumps(float duration, float multiplier)
    {
        StartCoroutine(PowerUpIncreaseJumpsRoutine(duration, multiplier));
    }

    public void PowerUpGravityGun(float duration, float multiplier)
    {
        StartCoroutine(PowerUpGravityGunRoutine(duration, multiplier));
    }

    public void PowerUpIncreaseMass(float duration, float multiplier)
    {
        StartCoroutine(PowerUpIncreaseMassRoutine(duration, multiplier));
    }

    public void PowerUpShootSpeed(float duration, float multiplier)
    {
        StartCoroutine(PowerUpShootSpeedRoutine(duration, multiplier));
    }


    IEnumerator PowerUpIncreasePowerRoutine(float duration, float multiplier)
    {
        //FPfireBullet.increasePower = true;
        FPfireBullet.powerMultiplier = multiplier;
        yield return new WaitForSeconds(duration);
        //FPfireBullet.increasePower = false;
        FPfireBullet.powerMultiplier = 1f;
    }

    IEnumerator PowerUpIncreaseJumpsRoutine(float duration, float multiplier)
    {
        increaseJumpsActive = true;
        increaseJumpsMultiplier = multiplier;

        jumpsLeft += (int)multiplier;

        yield return new WaitForSeconds(duration);
        increaseJumpsActive = false;
        increaseJumpsMultiplier = 1f;
    }

    IEnumerator PowerUpGravityGunRoutine(float duration, float multiplier)
    {
        FPfireBullet.gravityGun = true;
        yield return new WaitForSeconds(duration);
        FPfireBullet.gravityGun = false;
    }

    IEnumerator PowerUpIncreaseMassRoutine(float duration, float multiplier)
    {
        increaseMass = true;
        increaseMassMultiplier = multiplier;

        rb.mass *= increaseMassMultiplier;

        yield return new WaitForSeconds(duration);
        rb.mass = 1f;
        increaseMass = false;
        increaseMassMultiplier = 1f;
    }

    IEnumerator PowerUpShootSpeedRoutine(float duration, float multiplier)
    {
        increaseShootSpeed = true;

        FPfireBullet.shootSpeedMultiplier *= multiplier;

        yield return new WaitForSeconds(duration);

        increaseShootSpeed = false;
        FPfireBullet.shootSpeedMultiplier = 1f;
    }



    public void OnItemPickup(string itemName)
    {
        FPfireBullet.SendMessage("OnItemPickup", itemName);
    }


}
