using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public float speed = 5f;
    public float jumpHeight = 12f;
    public float damageMultiplier = 1f;
    public float fireRateMultiplier = 1f;
    public float maxShields = 0f;
    public float shields = 0f;
    public float invulnerabilityDuration = 1.5f;
    public float shieldRegenTime = 45f;

    public int coinScore;

    public bool isDead = false;
    public bool isInvulnerable = false;

    public bool canPistolShoot = true;
    public float pistolCooldownTime;

    public bool canRifleShoot = true;
    public float rifleCooldownTime;

    public bool canShotgunShoot = true;
    public float shotgunCooldownTime;

    public LayerMask groundLayer;

    public GameObject bulletPrefab;
    public GameObject Gun, Gun2, Gun3, activeGun;
    public Coroutine shieldRegenCoroutine;
    public Transform GunTip;
    public Text scoreTxt;
    //public Camera mainCamera;

    bool isGrounded;
    bool facingRight = true;
    bool isColliding;
    bool isRifleShooting = false;

    float moveDirection = 0;

    Vector3 cameraPos;
    Rigidbody2D rb;
    BoxCollider2D mainCollider;
    Animator anim;
    Transform t;

    void GroundCheck()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = 0.5f;

        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);
        if (hit.collider != null)
        {
            anim.SetBool("isJumping", false);
            isGrounded = true;
        }
        else
        {
            anim.SetBool("isJumping", true);
            isGrounded = false;
        }
    }

    void SwitchGuns(GameObject selectedGun)
    {
        activeGun.SetActive(false);
        activeGun = selectedGun;
        activeGun.SetActive(true);
        GunTip = activeGun.gameObject.transform.GetChild(0);
    }

    void Start()
    {
        t = transform;
        rb = GetComponent<Rigidbody2D>();
        mainCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        GunTip = activeGun.gameObject.transform.GetChild(0);
        facingRight = t.localScale.x > 0;
        activeGun = Gun;
        coinScore = PlayerPrefs.GetInt("wallet");
        scoreTxt.text = coinScore.ToString();

        /*if (mainCamera)
        {
            cameraPos = mainCamera.transform.position;
        }*/

        currentHealth = maxHealth;
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            isDead = true;
            activeGun.SetActive(false);
            anim.SetBool("isDead", true);
        }

        if (Input.GetKey(KeyCode.Alpha1) && !Gun.activeInHierarchy)
        {
            isRifleShooting = false;
            SwitchGuns(Gun);
        }
        if (Input.GetKey(KeyCode.Alpha2) && !Gun2.activeInHierarchy)
        {
            SwitchGuns(Gun2);
        }
        if (Input.GetKey(KeyCode.Alpha3) && !Gun3.activeInHierarchy)
        {
            isRifleShooting = false;
            SwitchGuns(Gun3);
        }

        // Movement controls
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            moveDirection = Input.GetKey(KeyCode.A) ? -1 : 1;
        }
        else
        {
            if (isGrounded)
            {
                moveDirection = 0;
            }
        }

        //Shooting controls
        if(Input.GetMouseButtonDown(0) && !isDead)
        {
            if (activeGun == Gun && canPistolShoot)
            {
                Animator GunAnim = activeGun.GetComponent<Animator>();
                GunAnim.SetTrigger("IsShooting");
                Pistol pistolScript = activeGun.GetComponent<Pistol>();
                GameObject bullet = (GameObject)Instantiate(bulletPrefab, GunTip.position, transform.rotation);
                Bullet bulletScript = bullet.GetComponent<Bullet>();
                bulletScript.playerBulletDamage = pistolScript.GunDamage * damageMultiplier;
                if (facingRight)
                {
                    bulletScript.moveToX = 1;
                }
                else
                {
                    bulletScript.moveToX = -1;
                }
                StartCoroutine(pistolCooldown());    
            }
            else if (activeGun == Gun2)
            {
                isRifleShooting = true;

            }
            else if (activeGun == Gun3 && canShotgunShoot)
            {
                Shotgun shotgunScript = activeGun.GetComponent<Shotgun>();
                shotgunScript.Shoot(facingRight, damageMultiplier);
                StartCoroutine(shotgunCooldown());
            }
        }
        if(Input.GetMouseButtonUp(0) && !isDead && activeGun == Gun2)
        {
            isRifleShooting = false;
        }

        // Change facing direction
        if (moveDirection != 0 && !isDead)
        {
            if (moveDirection > 0 && !facingRight)
            {
                facingRight = true;
                t.localScale = new Vector3(Mathf.Abs(t.localScale.x), t.localScale.y, t.localScale.z);
            }
            if (moveDirection < 0 && facingRight)
            {
                facingRight = false;
                t.localScale = new Vector3(-Mathf.Abs(t.localScale.x), t.localScale.y, t.localScale.z);
            }
        }

        // Jumping
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && isGrounded && !isDead)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            anim.SetBool("isJumping", true);
        }

        if (isRifleShooting && canRifleShoot && !isDead)
        {
            Animator GunAnim = activeGun.GetComponent<Animator>();
            GunAnim.SetTrigger("IsShooting");
            Guns gunScript = activeGun.GetComponent<Guns>();
            GameObject bullet = (GameObject)Instantiate(bulletPrefab, GunTip.position, transform.rotation);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.playerBulletDamage = gunScript.GunDamage * damageMultiplier;
            if (facingRight)
            {
                bulletScript.moveToX = 1;
            }
            else
            {
                bulletScript.moveToX = -1;
            }
            StartCoroutine(rifleCooldown());
        }

        if(shields < maxShields && shieldRegenCoroutine == null)
        {
            shieldRegenCoroutine = StartCoroutine(shieldRegen());
        }

    }

    void FixedUpdate()
    {
        if (!isDead)
        {
            Move();
        }
        else
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
        if(rb.velocity.x != 0 && !isDead)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
        GroundCheck();
        anim.SetFloat("yVelocity", rb.velocity.y);
    }
    
    void Move()
    {
        rb.velocity = new Vector2((moveDirection) * speed, rb.velocity.y);
    }

    IEnumerator InvulnerabilityCoroutine()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityDuration);
        isInvulnerable = false;
    }

    IEnumerator pistolCooldown()
    {
        canPistolShoot = false;
        yield return new WaitForSeconds(pistolCooldownTime * fireRateMultiplier / 100);
        canPistolShoot = true;
    }

    IEnumerator shotgunCooldown()
    {
        canShotgunShoot = false;
        yield return new WaitForSeconds(shotgunCooldownTime * fireRateMultiplier / 100);
        canShotgunShoot = true;
    }

    IEnumerator rifleCooldown()
    {
        canRifleShoot = false;
        yield return new WaitForSeconds(rifleCooldownTime * fireRateMultiplier / 100);
        canRifleShoot = true;
    }

    IEnumerator shieldRegen()
    {
        yield return new WaitForSeconds(shieldRegenTime);
        if(shields < maxShields)
        {
            shields++;
        }
        shieldRegenCoroutine = null;
    }

    public void GetCoin(int coinValue)
    {
        coinScore += coinValue;
        PlayerPrefs.SetInt("wallet", PlayerPrefs.GetInt("wallet") +coinValue);
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreTxt.text = coinScore.ToString();
    }

    public void TakeDamage(float damage)
    {
        if (!isInvulnerable && !isDead)
        {
            StartCoroutine(InvulnerabilityCoroutine());
            anim.Play("PlayerHurt");
            if(shields <= 0)
            {
                currentHealth -= damage;
            }
            else
            {
                shields--;
            }
            //Debug.Log("Player took damage: " + damage + ". Current health: " + currentHealth);
        }
        
    }
}