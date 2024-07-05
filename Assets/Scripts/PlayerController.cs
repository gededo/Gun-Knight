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
    public float invulnerabilityDuration = 1.5f;
    public float shieldRegenTime = 45f;

    public int maxShields = 0;
    public int shields = 0;

    public int coinScore;
    public Slider healthSlider;

    public bool isDead = false;
    public bool isInvulnerable = false;

    public bool canPistolShoot = true;
    public float pistolCooldownTime;

    public bool boughtRifle = false;
    public bool canRifleShoot = true;
    public float rifleCooldownTime;

    public bool boughtShotgun = false;
    public bool canShotgunShoot = true;
    public float shotgunCooldownTime;

    public LayerMask groundLayer;

    public GameObject bulletPrefab;
    public GameObject Gun, Gun2, Gun3, activeGun;
    public GameObject deathScreen;
    public ShieldHud shieldScript;
    public Coroutine shieldRegenCoroutine;
    public Coroutine walkCoroutine;
    public Transform GunTip;
    public Text scoreTxt;
    //public Camera mainCamera;

    public bool isGrounded;
    bool facingRight = true;
    bool isColliding;
    bool isRifleShooting = false;

    public AudioClip damageSoundClip;
    [SerializeField] private AudioClip jumpSoundClip;
    [SerializeField] private AudioClip pistolSoundClip;
    [SerializeField] private AudioClip rifleSoundClip;
    [SerializeField] private AudioClip shotgunSoundClip;
    [SerializeField] private AudioClip shotgunReloadSoundClip;
    [SerializeField] private AudioClip coinSoundClip;
    [SerializeField] private AudioClip shieldBreakSoundClip;
    [SerializeField] private AudioClip dieSoundClip;
    [SerializeField] private AudioClip walkSoundClip;
    [SerializeField] private AudioClip shieldRegenSoundClip;

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

    public void SwitchGuns(GameObject selectedGun)
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
        shieldScript.UpdateShieldDisplay(shields);

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
            PlayDeathSound();
            isDead = true;
            activeGun.SetActive(false);
            anim.SetBool("isDead", true);
            deathScreen.SetActive(true);
        }

        if (Input.GetKey(KeyCode.Alpha1) && !Gun.activeInHierarchy)
        {
            isRifleShooting = false;
            SwitchGuns(Gun);
        }
        if (Input.GetKey(KeyCode.Alpha2) && !Gun2.activeInHierarchy && boughtRifle)
        {
            SwitchGuns(Gun2);
        }
        if (Input.GetKey(KeyCode.Alpha3) && !Gun3.activeInHierarchy && boughtShotgun)
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
        if(Input.GetMouseButtonDown(0) && !isDead && Time.timeScale == 1f)
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
                SoundFXManager.instance.PlaySoundFXCLip(pistolSoundClip, transform, 0.1f);
                StartCoroutine(pistolCooldown());
            }
            else if (activeGun == Gun2)
            {
                isRifleShooting = true;

            }
            else if (activeGun == Gun3 && canShotgunShoot && Time.timeScale == 1f)
            {
                Shotgun shotgunScript = activeGun.GetComponent<Shotgun>();
                shotgunScript.Shoot(facingRight, damageMultiplier);
                SoundFXManager.instance.PlaySoundFXCLip(shotgunSoundClip, transform, 0.1f);
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
            SoundFXManager.instance.PlaySoundFXCLip(jumpSoundClip, transform, 0.5f);
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            anim.SetBool("isJumping", true);
        }

        if (isRifleShooting && canRifleShoot && !isDead && Time.timeScale == 1f)
        {
            Animator GunAnim = activeGun.GetComponent<Animator>();
            GunAnim.SetTrigger("IsShooting");
            Guns gunScript = activeGun.GetComponent<Guns>();
            GameObject bullet = (GameObject)Instantiate(bulletPrefab, GunTip.position, transform.rotation);
            SoundFXManager.instance.PlaySoundFXCLip(rifleSoundClip, transform, 0.2f);
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
        if(walkCoroutine == null && moveDirection != 0 && isGrounded)
        {
            walkCoroutine = StartCoroutine(walkFXCoroutine());
        }
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
        yield return new WaitForSeconds((shotgunCooldownTime * fireRateMultiplier / 100) - 0.5f);
        SoundFXManager.instance.PlaySoundFXCLip(shotgunReloadSoundClip, transform, 0.5f);
        yield return new WaitForSeconds(0.5f);
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
            SoundFXManager.instance.PlaySoundFXCLip(shieldRegenSoundClip, transform, 0.5f);
            shields++;
            shieldScript.UpdateShieldDisplay(shields);
        }
        shieldRegenCoroutine = null;
    }

    public void GetCoin(int coinValue)
    {
        coinScore += coinValue;
        SoundFXManager.instance.PlaySoundFXCLip(coinSoundClip, transform, 0.1f);
        PlayerPrefs.SetInt("wallet", PlayerPrefs.GetInt("wallet") +coinValue);
        UpdateScoreText();
    }

    public void UpdateScoreText()
    {
        scoreTxt.text = coinScore.ToString();
    }

    public void SetHealthSlider(float health)
    {
        healthSlider.value = health;
    }

    public void TakeDamage(float damage)
    {
        if (!isInvulnerable && !isDead)
        {
            StartCoroutine(InvulnerabilityCoroutine());
            anim.Play("PlayerHurt");
            if(shields <= 0)
            {
                SoundFXManager.instance.PlaySoundFXCLip(damageSoundClip, transform, 0.8f);
                currentHealth -= damage;
                SetHealthSlider(currentHealth);
            }
            else
            {
                SoundFXManager.instance.PlaySoundFXCLip(shieldBreakSoundClip, transform, 0.1f);
                shields--;
                shieldScript.UpdateShieldDisplay(shields);
            }
            //Debug.Log("Player took damage: " + damage + ". Current health: " + currentHealth);
        }
        
    }

    public void PlayDeathSound()
    {
        if (!isDead)
        {
            SoundFXManager.instance.PlaySoundFXCLip(dieSoundClip, transform, 0.5f);
        }
    }

    IEnumerator walkFXCoroutine()
    {
        bool a = PlayerPrefs.GetString("equippedpowerups").Contains("Speed Boost");
        float walkSpeed = a ? 0.2f : 0.3f;
        SoundFXManager.instance.PlaySoundFXCLip(walkSoundClip, transform, 0.03f);
        yield return new WaitForSeconds(walkSpeed);
        walkCoroutine = null;
        StopCoroutine(walkFXCoroutine());
    }
}