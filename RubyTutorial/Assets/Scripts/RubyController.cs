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

    private int currentHealth;
    public int health { get { return currentHealth; } }
    private float hvBaseSpeed=3.0f;
    private Vector2 lookDirection = new Vector2(1, 0);
    
    private bool isInvincible;
    float invincibleTimer;

    private Rigidbody2D rigidbody2D;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        currentHealth = startHealth;
        animator = GetComponent<Animator>();

        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 28;


    }

    // Update is called once per frame
    void Update()
    {
        RigidMovement();
        InvincibleTester();
        ShootingTester();
        
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
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
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
