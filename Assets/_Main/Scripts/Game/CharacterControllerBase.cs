using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;



public class CharacterControllerBase : MonoBehaviourPunCallbacks,IPunObservable
{
    public delegate bool ComboCondition(List<int> attackInputSchedule);
    public delegate void ComboAttack();
    Rigidbody2D rb;
    Animator anim;
    public TextMeshProUGUI nameDisplay;
    PhotonView view;
    #region movement
    [Header("Move Stats")]
    [SerializeField]
    float groundCheckOffset = 0.3f;
    [SerializeField]
    float moveSpeed = 15;
    [SerializeField]
    float jumpSpeed = 35;
    [SerializeField] float rememberGroundedFor = 0.5f;
    float lastTimeGrounded;
    bool isGrounded = false;
    LayerMask groundLayer;
    public Vector2 moveDirection;
    CapsuleCollider2D characterCollider;
    float groundCheckHeight;
    public bool jump;
    #endregion

    #region combat
    //use for combos
    public List<int> attackInputTypeSchedule; // attack condition should be a List<int>
    [SerializeField] bool isBlocking;
    public Dictionary<ComboCondition, ComboAttack> comboDictionary;
    public Transform projectileSpawnPoint;
    bool canBlock = true;

    #endregion

    #region CombatStats
    [Header("Combat Stats")]
    [SerializeField] float ultimateMaxMeter = 100.0f;
    [NonSerialized] public float currentUltimateMeter;
    public float attack1Damage;
    public float attack2Damage;
    [SerializeField] float inputBufferTimeBeforeReset = 1.5f;
    float timeSinceLastResetInputBuffer = 0;
    [SerializeField] float blockCoolDown = 5.0f;
    float timeSinceLastBlock;
    #endregion

    #region RPC

    [PunRPC]
    public void Attack1InputRPC()
    {
        Attack1();
    }
    [PunRPC]
    public void Attack2InputRPC()
    {
        Attack2();
    }
    [PunRPC]
    public void BlockInputRPC()
    {
        Block();
    }
    [PunRPC]
    public void UltimateAttackInputRPC()
    {
        UltimateAttack();
    }

    #endregion

    #region Unity CallBacks
    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        timeSinceLastBlock = blockCoolDown;
        SetupPhysics();
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogError("Animator Comp is not found");
        }

        nameDisplay = GetComponentInChildren<TextMeshProUGUI>();
        view = GetComponent<PhotonView>();
        SetupComboDictionary();

    }

    public virtual void Start()
    {
        view.ObservedComponents.Add(this);
    }
    public virtual void Update()
    {
        CheckAndExcuteCombos();
        //Clear Input Buffer
        timeSinceLastResetInputBuffer += Time.deltaTime;
        if(timeSinceLastResetInputBuffer >= inputBufferTimeBeforeReset)
        {
            attackInputTypeSchedule.Clear();
            timeSinceLastResetInputBuffer = 0;
        }

        //Reset BlockTimer
        timeSinceLastBlock += Time.deltaTime;
        if(timeSinceLastBlock >= blockCoolDown)
        {
            canBlock = true;
        }
        isGrounded = Physics2D.Raycast(transform.position, -transform.up, groundCheckHeight, groundLayer);
        anim.SetBool("Grounded", isGrounded);
        
    }

    public virtual void FixedUpdate()
    {
        Move();
    }
    #endregion

    //for displaying ingame ui
    public Sprite characterAvatar;
    void SetupPhysics()
    {
        characterCollider = GetComponent<CapsuleCollider2D>();
        float height = characterCollider.size.y / 2;
        groundCheckHeight = height + groundCheckOffset;
        rb.gravityScale = 8;
        PhysicsMaterial2D physicsmat = Resources.Load("CharacterPhysicsMaterial") as PhysicsMaterial2D;
        if (physicsmat != null)
        {
            rb.sharedMaterial = physicsmat;
        }
        groundLayer = LayerMask.GetMask("Ground");
        if (moveSpeed == 0)
        {
            moveSpeed = 15;
        }

        if (jumpSpeed == 0)
        {
            jumpSpeed = 35;
        }
    }
    public bool GroundCheck()
    {
        return isGrounded || Time.time - lastTimeGrounded <= rememberGroundedFor;
    }
    void Move()
    {
        Vector2 moveVelocity = new Vector2( moveDirection.x * moveSpeed, rb.velocity.y);
        rb.velocity = moveVelocity;
        anim.SetFloat("speed", Mathf.Clamp(1,0,Mathf.Abs(rb.velocity.x)));
        
        //jump
        if(jump)
        {
            rb.velocity = Vector2.up * jumpSpeed;
            anim.SetTrigger("jump");
            jump = false;
        }
        
    }
    public void Flip(bool isFacingRight)
    {
        if(isFacingRight)
        {
            transform.localEulerAngles = new Vector3(0,0,0);
        }
        else 
        {
            transform.localEulerAngles = new Vector3(0, 180, 0);
        }
    }
    public void Block()
    {
        if (canBlock == false) return;
        ResetAllTriggers();
        isBlocking = true;
        canBlock = false;
        timeSinceLastBlock = 0;
        anim.SetTrigger("block");
    }
    
    public float GetTimeSinceLastBlock()
    {
        return timeSinceLastBlock;
    }

    public float GetBlockCooldown()
    {
        return blockCoolDown;
    }

    public bool GetCurrentlyIsBlocking()
    {
        return isBlocking;
    }

    public float GetMaxUltimateMeter()
    {
        return ultimateMaxMeter;
    }

    public float GetCurrentUltimateMeter()
    {
        return currentUltimateMeter;
    }

    public void AddUltimateMeter(float amount)
    {
        currentUltimateMeter += amount;
    }
    public void ReceiveDamage(CharacterControllerBase source, float damage, bool unblockable)
    {
        if (isBlocking && unblockable == false)
        {
            isBlocking = false; // make sure the character is not blocking incase some bug happens
            OnSuccessfullyBlock(source,damage);
        }

        else
        {
            ResetAllTriggers();
            anim.SetTrigger("takeDamage");
            Health health = GetComponent<Health>();
            if(health != null)
            {
                health.TakeDamage(damage);
            }
            print("take Damage");
        }    
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.up * -1 * groundCheckHeight);
    }
    public static void ApplyDamage(CharacterControllerBase dealDamageCharacter, CharacterControllerBase damagedCharacter, float damage, bool unblockable)
    {
        damagedCharacter.ReceiveDamage(dealDamageCharacter, damage, unblockable);
    }

    public void SpawnProjectile(GameObject projectile)
    {
        if (projectile == null) return;
        float playerYRot = transform.eulerAngles.y;
        if(projectileSpawnPoint == null)
        {
            projectileSpawnPoint = this.transform;
        }
        GameObject newProjectile = Instantiate(projectile, projectileSpawnPoint.position, Quaternion.identity);
        ProjectileBase proj = newProjectile.GetComponent<ProjectileBase>();
        //Set projectile direction
        if(proj != null)
        {
            proj.SetupProjectile(playerYRot, this.gameObject);
        }
        
    }
    #region methods to override
    protected virtual void ResetAllTriggers()
    {
        //Trigger Take damage should not be called since it should overide anything else
        isBlocking = false; // unblock if the character is transiting to another animation
        anim.ResetTrigger("jump");
        anim.ResetTrigger("attack1");
        anim.ResetTrigger("attack2");
        anim.ResetTrigger("block");
    }
    public virtual void Attack1()
    {
        ResetAllTriggers();
        anim.SetTrigger("attack1");
        attackInputTypeSchedule.Add(1);
    }

    public virtual void Attack2()
    {
        ResetAllTriggers();
        anim.SetTrigger("attack2");
        attackInputTypeSchedule.Add(2);
    }

    public virtual void UltimateAttack()
    {

    }
    public virtual void SetupComboDictionary()
    {
        comboDictionary = new Dictionary<ComboCondition, ComboAttack>();
    }

    public virtual void OnSuccessfullyBlock(CharacterControllerBase source,float damage)
    {
        print("SuccessFully Block");
    }

    //Animation Event
    public virtual void OnAttack1Hit()
    {
        ResetAllTriggers();
    }

    //Animation Event
    public virtual void OnAttack2Hit()
    {
        ResetAllTriggers();
    }

    //Animation Event
    public virtual void OnBlockEnd()
    {
        print("End Block");
        isBlocking = false;
    }

    #endregion

    public void CheckAndExcuteCombos()
    {   
        if(comboDictionary == null)
        {
            return;
        }
        foreach (var combo in comboDictionary.Keys)
        {
            if(combo(attackInputTypeSchedule))
            {   
                //Disable all other attacks
                ResetAllTriggers();
                comboDictionary[combo]();
                attackInputTypeSchedule.Clear();
                return;
            }

        }
    }

    public bool CheckAComboCondition(List<int> attachScheduleCondition)
    {
        if (attackInputTypeSchedule.Count < attachScheduleCondition.Count)
        {
            return false;
        }

        for (int i = 0; i <= attachScheduleCondition.Count - 1; i++)
        {
            if(attachScheduleCondition[i] != attackInputTypeSchedule[attackInputTypeSchedule.Count - attachScheduleCondition.Count + i])
            {
                return false;
            }
        }
        return true;
    }

    //To sync the block/ultimate meter
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {

        }
        else
        {

        }
    }
}
