using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public bool vertical;
    public float changeTime;
    public float changeTime2;
    public GameObject projectilePrefab;

    public int maxHealth = 5;//

    public int health { get { return currentHealth; } }
    int currentHealth;

    Rigidbody2D rigidbody2D;
    float timer;
    float timer2;
    int direction = 1;
    bool broken = true;

    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        timer = changeTime;
        timer2 = changeTime2;
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        //remember ! inverse the test, so if broken is true !broken will be false and return won’t be executed.
        if (!broken)
        {
            return;
        }

        timer -= Time.deltaTime;
        timer2 -= Time.deltaTime;
        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
        if (timer2 < 0 & direction == 1)
        {
            Launch();
            timer2 = changeTime2;
        }
    }

    void FixedUpdate()
    {
        //remember ! inverse the test, so if broken is true !broken will be false and return won’t be executed.
        if (!broken)
        {
            return;
        }

        Vector2 position = rigidbody2D.position;

        if (vertical)
        {
            position.y = position.y + Time.deltaTime * speed * direction;
            animator.SetFloat("Look X", 0);
            animator.SetFloat("Look Y", direction);
            animator.SetFloat("Speed", 0.2f);
        }
        else
        {
            position.x = position.x + Time.deltaTime * speed * direction;
            animator.SetFloat("Look X", direction);
            animator.SetFloat("Look Y", 0);
            animator.SetFloat("Speed", 0.2f);
        }

        rigidbody2D.MovePosition(position);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }

    //Public because we want to call it from elsewhere like the projectile script
    public void Fix()
    {
        currentHealth = Mathf.Clamp(currentHealth - 1, 0, maxHealth);
        UIHealthBarEnemy.instance.SetValue(currentHealth / (float)maxHealth);
        if (currentHealth == 0) 
        { 
            broken = false;
            rigidbody2D.simulated = false;
            //optional if you added the fixed animation
            animator.SetTrigger("Stop");
            animator.SetFloat("Speed", 0f);
        }
    }

    void Launch()
    {
        animator.SetTrigger("Launch");
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2D.position + Vector2.up * 0.5f, Quaternion.identity);
        ProjectileEnemy projectile = projectileObject.GetComponent<ProjectileEnemy>();
        projectile.Launch(new Vector2(0,-1), 500);        
    }
}