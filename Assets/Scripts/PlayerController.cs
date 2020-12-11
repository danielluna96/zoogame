using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 3.0f;

    public int maxHealth = 5;
    public int bananaMount = 1;


    //Audios Utilizados para los sonidos
    public AudioClip walkSound;
    public AudioClip launchSound;
    public AudioClip pickSound;
    public AudioClip hurtSound;

    //Componente de audio
    AudioSource audioSource;

    //Objeto prefab que será lanzado
    public GameObject projectilePrefab; 

    public int health { get { return currentHealth; } }
    int currentHealth;

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;
    float timerCounterWalk = 0.6f;
    float timerWalk = 0f;
    int targetFrameRate = 120;

    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;

        //Se establece un max frame rate de 120 fps
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFrameRate;

        //Componente de audio con el componente del GameObject
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        //Si se esta moviendo
        if (move.magnitude > 0)
        {
            timerWalk -= Time.deltaTime;
            if (timerWalk < 0)
            {
                playSound(walkSound);
                timerWalk = timerCounterWalk;
            }
        }

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
                //animator.SetTrigger("Hit");

        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (bananaMount > 0)
                Launch();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                Debug.Log("Raycast has hit the object " + hit.collider.gameObject);
            }
        }
    }
    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;
            animator.SetTrigger("Hit");
            playSound(hurtSound);
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
        if (currentHealth <= 0)
        {
            rigidbody2d.simulated = false;
            DialogueManager dialogueManager = gameObject.GetComponent<DialogueManager>();
            dialogueManager.changeMap(true, "MainMenu");
        }
    }

    public void BananaPickUp()
    {
        if (bananaMount < 5)
        {
            bananaMount++;
            playSound(pickSound);
        }
        UIProjectileCounter.instance.SetValue(bananaMount);
    }

    void Launch()
    {
        bananaMount--;
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 600);

        animator.SetTrigger("Launch");
        UIProjectileCounter.instance.SetValue(bananaMount);
        playSound(launchSound);
    }

    void playSound(AudioClip audio)
    {
        audioSource.PlayOneShot(audio);
    }
}
