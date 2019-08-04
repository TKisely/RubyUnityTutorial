using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //[SerializeField]
    private float speed = 1.5f;
    [SerializeField]
    private bool vertical;

    private float changeTime = 2.0f;
    private float timer;
    private int direction = 1;
    private bool broken = true;

    Rigidbody2D rigidbody2D;
    Animator animator;
    

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        timer = changeTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!broken) { return; }

        timer -= Time.deltaTime;
        if (timer<=0)
        {
            direction *= -1;
            timer = changeTime;
        }

        Vector2 position = rigidbody2D.position;

        if (vertical)
        {
            position.y = position.y + Time.deltaTime * speed*direction;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else
        {
            position.x = position.x + Time.deltaTime * speed*direction;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }

        rigidbody2D.MovePosition(position);
    }

    public void Fix()
    {
        animator.SetTrigger("Fixed");
        broken = false;
        rigidbody2D.simulated = false;
       
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController>();

        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }
}
