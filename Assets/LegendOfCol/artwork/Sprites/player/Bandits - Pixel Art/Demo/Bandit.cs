using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class Bandit : MonoBehaviour {

    [SerializeField] float      m_speed = 4.0f;
    [SerializeField] float      m_jumpForce = 7.5f;

    public static Bandit Instance;

    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    private Sensor_Bandit       m_groundSensor;
    private bool                m_grounded = false;
    private bool                m_combatIdle = false;
    private bool                m_isDead = false;
    private bool                isAttacking = false;
    private bool                isBlocking = false;
    private bool                isCountering = false;

    public string currentScene;

    private BoxCollider2D charCollider;
    public LayerMask Ground;

    public int maxHealth;
    public int currentHealth;

    public int attackDamage = 45;
    public Transform attackPoint;
    public BoxCollider2D groundBox;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    //effect
    public GameObject landSmoke;
    public Transform landSmokePos;

    //Freezer
    Freezer _freezer;

    //Hurting
    private bool isHurting = false;
    private float hurtingTime = 0f;

    //
    private bool isJumping = false;
    private float jumpingTime = 0f;

    //Checkpoint
    public Vector3 respawmPoint;

    //joystick
    public FixedJoystick joystick;

    private void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
        PlayerPrefs.DeleteAll();
    }

    // Use this for initialization
    void Start () {
        if (PlayerPrefs.HasKey("PlayerMaxHealth"))
        {
            maxHealth = PlayerPrefs.GetInt("PlayerMaxHealth");
        }
        if (PlayerPrefs.HasKey("PlayerCurrentHealth"))
        {
            currentHealth = PlayerPrefs.GetInt("PlayerCurrentHealth");
        }
        if (PlayerPrefs.HasKey("PlayerDamage"))
        {
            attackDamage = PlayerPrefs.GetInt("PlayerDamage");
        }
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Bandit>();
        charCollider = GetComponent<BoxCollider2D>();
        //if(Instance != null)
        //{
        //    Destroy(this.gameObject);
        //    return;
        //}
        //Instance = this;
        //GameObject.DontDestroyOnLoad(this.gameObject);

        GameObject mgr = GameObject.FindWithTag("Manager");
        if (mgr)
        {
            _freezer = mgr.GetComponent<Freezer>();
        }

        respawmPoint = this.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        PlayerPrefs.SetInt("PlayerMaxHealth", maxHealth);
        PlayerPrefs.SetInt("PlayerCurrentHealth", currentHealth);
        PlayerPrefs.SetInt("PlayerDamage", attackDamage);
        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State()) {
            //SoundEffectManager.playSound("PlayerJumpLand");
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Check if character just started falling
        if(m_grounded && !m_groundSensor.State()) {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // -- Handle input and movement --
        //float inputX = Input.GetAxis("Horizontal");
        float inputX = joystick.Horizontal;

        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else if (inputX < 0)
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        // Move
        if (!isAttacking && !isBlocking && !m_animator.GetCurrentAnimatorStateInfo(0).IsName("Hurt"))
            m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeed", m_body2d.velocity.y);

        // -- Handle Animations --
        //Death
        if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("Hurt"))
        {

        }
            
        //Attack
        else if(CrossPlatformInputManager.GetButton("Attack") && !isAttacking && !isBlocking && !m_animator.GetCurrentAnimatorStateInfo(0).IsName("Hurt")) {
            if (m_grounded)
                m_body2d.velocity = Vector2.zero;
            SoundEffectManager.playSound("PlayerAttack");
            m_animator.SetTrigger("Attack");
            isAttacking = true;
        }

        //Block
        else if (CrossPlatformInputManager.GetButton("Block") && !isBlocking && !isAttacking && !m_animator.GetCurrentAnimatorStateInfo(0).IsName("Hurt"))
        {
            if(m_grounded)
                m_body2d.velocity = new Vector2(0, m_body2d.velocity.y);
            //SoundEffectManager.playSound("PlayerAttack");
            m_animator.SetTrigger("Block");
            isBlocking = true;
        }

        //Jump
        else if (CrossPlatformInputManager.GetButton("Jump") && !isJumping && (groundBox.IsTouchingLayers(Ground) || groundBox.IsTouchingLayers(enemyLayers)) && !isCountering) {
            Instantiate(landSmoke, landSmokePos.position, Quaternion.identity);
            isJumping = true;
            isBlocking = false;
            isAttacking = false;
            SoundEffectManager.playSound("PlayerJump");
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
        }

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
            m_animator.SetInteger("AnimState", 2);

        //Combat Idle
        else if (m_combatIdle)
            m_animator.SetInteger("AnimState", 1);

        //Idle
        else
            m_animator.SetInteger("AnimState", 0);

        if (isHurting)
        {
            hurtingTime += Time.deltaTime;
            if (hurtingTime >= 0.8f)
            {
                isHurting = false;
                hurtingTime = 0f;
            }
        }
        if (isJumping)
        {
            jumpingTime += Time.deltaTime;
            if (jumpingTime >= 0.1f)
            {
                isJumping = false;
                jumpingTime = 0f;
            }
        }
    }

    public void Heal(int healAmount)
    {
        if(currentHealth < maxHealth)
        {
            if (healAmount + currentHealth > maxHealth)
            {
                HeartsHealthVisual.heartsHealthSystemStatic.Heal(maxHealth - currentHealth);
                this.currentHealth = maxHealth;
            }
            else
            {
                this.currentHealth += healAmount;
                HeartsHealthVisual.heartsHealthSystemStatic.Heal(healAmount);
            }
        }
        
    }

    void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.GetComponent<EnemyBossWithBlock>() != null)
            {
                enemy.GetComponent<EnemyBossWithBlock>().TakeDamage(attackDamage, transform.localScale.x > 0 ? -1 : 1, false);
            }
            else if (enemy.GetComponent<Enemy>() != null)
            {
                enemy.GetComponent<Enemy>().TakeDamage(attackDamage, transform.localScale.x > 0 ? -1 : 1, false);
            }
            else if (enemy.GetComponent<EnemyWithBlock>() != null)
            {
                enemy.GetComponent<EnemyWithBlock>().TakeDamage(attackDamage, transform.localScale.x > 0 ? -1 : 1, false);
            }
            
        }
    }
    void AttackDone()
    {
        isAttacking = false;
        isCountering = false;
    }

    void BlockDone()
    {
        isBlocking = false;
    }

    void CounterAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.GetComponent<EnemyBossWithBlock>() != null)
            {
                enemy.GetComponent<EnemyBossWithBlock>().TakeDamage(attackDamage, transform.localScale.x > 0 ? -1 : 1, true);
            }
            else if (enemy.GetComponent<Enemy>() != null)
            {
                enemy.GetComponent<Enemy>().TakeDamage(attackDamage, transform.localScale.x > 0 ? -1 : 1, true);
            }
            else if (enemy.GetComponent<EnemyWithBlock>() != null)
            {
                enemy.GetComponent<EnemyWithBlock>().TakeDamage(attackDamage, transform.localScale.x > 0 ? -1 : 1, true);
            }
        }
    }

    public void TakeDamage(int damage, int direction, int type)
    {
        //SoundEffectManager.playSound("PlayerDeflect");
        //_freezer.Freeze();
        //if (!m_grounded)
        //{
        //    m_animator.SetTrigger("BackToNormal");
        //}
        //else
        //{
        //    m_animator.SetTrigger("CounterAttack");
        //    isAttacking = true;
        //    isCountering = true;
        //}
        //isBlocking = false;
        if (type == 5)
        {
            HeartsHealthVisual.heartsHealthSystemStatic.Damage(damage);
            currentHealth -= damage;
            isAttacking = false;
            if (currentHealth <= 0)
            {
                Debug.Log("Hero died!");
                //GetComponent<Collider2D>().enabled = false;
                SoundEffectManager.playSound("PlayerDie");
                m_animator.SetTrigger("Death");
                this.enabled = false;
                m_isDead = true;
                //if (PlayerPrefs.HasKey("PlayerDamage"))
                //{
                //    attackDamage = PlayerPrefs.GetInt("PlayerDamage");
                //}
                //PlayerPrefs.DeleteAll();
                //PlayerPrefs.SetInt("PlayerDamage", attackDamage);
                PlayerPrefs.DeleteKey("PlayerCurrentHealth");
                StartCoroutine(LoadScene());
            }
            else
            {
                this.transform.position = respawmPoint;
                SoundEffectManager.playSound("PlayerGetHit");
            }
        }
        else if (isBlocking)
        {
            if (m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < .2f)
            {
                SoundEffectManager.playSound("PlayerDeflect");
                _freezer.Freeze();
                //m_animator.SetTrigger("BackToNormal");
                //m_body2d.AddForce(new Vector2(100 * direction, 0));
                if (!m_grounded)
                {
                    m_animator.SetTrigger("BackToNormal");
                }
                else
                {
                    m_animator.SetTrigger("CounterAttack");
                    isAttacking = true;
                    isCountering = true;
                }
                isBlocking = false;
            }
            else
            {
                SoundEffectManager.playSound("PlayerBlock");
                int randomBlock = Random.Range(1, 101);
                if (randomBlock < 25)
                {
                    HeartsHealthVisual.heartsHealthSystemStatic.Damage(1);
                    currentHealth -= 1;
                }
                m_body2d.AddForce(new Vector2(150 * direction, 0));
            }
        }
        else if (!isHurting && !m_isDead && !isCountering)
        {
            HeartsHealthVisual.heartsHealthSystemStatic.Damage(damage);
            currentHealth -= damage;
            m_animator.SetTrigger("Hurt");
            isAttacking = false;
            if (currentHealth <= 0)
            {
                Debug.Log("Hero died!");
                //GetComponent<Collider2D>().enabled = false;
                SoundEffectManager.playSound("PlayerDie");
                m_animator.SetTrigger("Death");
                this.enabled = false;
                m_isDead = true;
                if (PlayerPrefs.HasKey("PlayerDamage"))
                {
                    attackDamage = PlayerPrefs.GetInt("PlayerDamage");
                }
                PlayerPrefs.DeleteAll();
                PlayerPrefs.SetInt("PlayerDamage", attackDamage);
                StartCoroutine(LoadScene());
            }
            else
            {
                isHurting = true;
                SoundEffectManager.playSound("PlayerGetHit");
                m_body2d.AddForce(new Vector2(100 * direction, 0));
            }
        }
    }

    public void TakeHitDamage(int damage, int forceNumber)
    {
        if(!isHurting && !m_isDead && !isCountering)
        {
            HeartsHealthVisual.heartsHealthSystemStatic.Damage(damage);
            currentHealth -= damage;
            m_animator.SetTrigger("Hurt");
            isAttacking = false;
            isBlocking = false;
            if (currentHealth <= 0)
            {
                Debug.Log("Hero died!");
                //GetComponent<Collider2D>().enabled = false;
                SoundEffectManager.playSound("PlayerDie");
                m_animator.SetTrigger("Death");
                this.enabled = false;
                m_isDead = true;
                if (PlayerPrefs.HasKey("PlayerDamage"))
                {
                    attackDamage = PlayerPrefs.GetInt("PlayerDamage");
                }
                PlayerPrefs.DeleteAll();
                PlayerPrefs.SetInt("PlayerDamage", attackDamage);
                StartCoroutine(LoadScene());
            }
            else
            {
                isHurting = true;
                SoundEffectManager.playSound("PlayerGetHit");
                m_body2d.AddForce(new Vector2(100 * this.transform.localScale.x * forceNumber, 0));
            }
        }
    }

    public void StartHurt()
    {
        isHurting = true;
    }

    public void DoneHurt()
    {
        isAttacking = false;
        isBlocking = false;
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(currentScene);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
