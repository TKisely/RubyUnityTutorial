using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    public int maxHealth = 5;
    public int startHealth = 3;
    public float timeInvincible = 1.0f;
    public GameObject projectilePrefab;
    public ParticleSystem damagedEffect;
    public AudioClip damagedSound;
    public AudioClip throwACog;

    private int currentHealth;
    public int health { get { return currentHealth; } }
    private float hvBaseSpeed=3.0f;
    private Vector2 lookDirection = new Vector2(1, 0);
    
    private bool isInvincible;
    float invincibleTimer;

    private new Rigidbody2D rigidbody2D;
    private Animator animator;

    private static AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        currentHealth = startHealth;
        animator = GetComponent<Animator>();
        damagedEffect.Stop();
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
        audioSource = GetComponent<AudioSource>();

        Debug.Log(audioSource);

        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 28;


    }

    // Update is called once per frame
    void Update()
    {
        RigidMovement();
        InvincibleTester();
        ShootingTester();
        InteractionRayCasting();
        
    }

    static public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
        Debug.Log("Sound Played");
    }

    private void InteractionRayCasting()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2D.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if(hit.collider != null)
            {
                //Debug.Log("Raycast has hit an object: " + hit.collider.gameObject);
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();

                if (character != null)
                {
                    character.DisplayDialog();
                }
            }
        }
    }

    private void ShootingTester()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }
    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2D.position + Vector2.up * 0.5f, Quaternion.identity);

        CogBullet projectile = projectileObject.GetComponent<CogBullet>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");
        PlaySound(throwACog);
    }

    void RigidMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector2 moveDirectionVector = new Vector2(horizontalInput, verticalInput);

        if (!Mathf.Approximately(moveDirectionVector.x, 0.0f) || !Mathf.Approximately(moveDirectionVector.y, 0.0f))
        {
            lookDirection.Set(moveDirectionVector.x, moveDirectionVector.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", moveDirectionVector.magnitude);

        Vector2 position = rigidbody2D.position;

        position = position + moveDirectionVector * hvBaseSpeed * Time.deltaTime;

        rigidbody2D.MovePosition(position);
    }

    void Movement()
    {
        VerticalMovement();
        HorizontalMovement();
        
    }

    void InvincibleTester()
    {
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            animator.SetTrigger("Hit");
            damagedEffect.Play();
            
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;
            PlaySound(damagedSound);
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
        Debug.Log(currentHealth + " health from " + maxHealth);
    }

    void HorizontalMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        //Debug.Log(horizontalInput);

        Vector2 position = transform.position;
        position.x = position.x + hvBaseSpeed * horizontalInput * Time.deltaTime;
        transform.position = position;
    }

    void VerticalMovement()
    {
        float verticalInput = Input.GetAxis("Vertical");
        //Debug.Log(verticalInput);

        Vector2 position = transform.position;
        position.y = position.y + hvBaseSpeed * verticalInput * Time.deltaTime;
        transform.position = position;

    }
}
