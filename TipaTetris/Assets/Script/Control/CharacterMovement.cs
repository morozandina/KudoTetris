using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tetris;
using Characters;

namespace Control
{
    public class CharacterMovement : MonoBehaviour
    {
        // Actions
        public static Action<float, float> JoystickMove;
        public static Action Attack;

        public Character _character;
        // Effects || VFX
        [Header("Effects :")]
        [SerializeField] private ParticleSystem dust;
        // Character components and characteristics
        private float moveSpeed;
        
        private float jumpForce;
        private float jumpTimer;
        [Range(0f, 1f)]
        [SerializeField] private float checkRadius;
        [SerializeField] private LayerMask whatIsGround;
        [Range(0f, 1f)]
        [SerializeField] private float stepForJump;
        [Space]
        [Header("Attack :")]
        [Range(0f, 1f)]
        [SerializeField] private float checkRadiusAttack;
        [SerializeField] private Transform attackCheck;
        [SerializeField] private LayerMask whatIsTetrisBlock;
        [Space]
        [Header("Physics :")]
        [SerializeField] private float gravity;
        [SerializeField] private float linearDrag;
        [SerializeField] private float fallMultiplier;
        // Components
        private Rigidbody2D rb;
        private Animator animator;
        // Local variables
        private bool onGround = false;
        private bool facingRight = true;
        private Vector2 direction;

        private static Vector3 _lastPosition;

        private void Awake()
        {
            JoystickMove += JoystickMoveControl;
            Attack += OnAttack;
        }

        private void OnDestroy()
        {
            JoystickMove -= JoystickMoveControl;
            Attack -= OnAttack;
        }

        private void Start()
        {
            StartAddParam();
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }

        // Add character parameters
        private void StartAddParam()
        {
            moveSpeed = _character.moveSpeed;
            jumpForce = _character.jumpForce;
        }

        private void Update()
        {
            var wasOnGround = onGround;
            onGround = Physics2D.Raycast(transform.position, Vector2.down, checkRadius, whatIsGround);

            if (!wasOnGround && onGround)
            {
                StartCoroutine(JumpSqueeze(1.25f, .8f, .05f));
                CreateDust();
            }
        }

        private void FixedUpdate()
        {
            Move(direction.x);
            GetDamage();

            if (jumpTimer > Time.time && onGround)
                Jump();

            ModifyPhysics();
        }

        private void JoystickMoveControl(float horizontal, float vertical)
        {
            direction = new Vector2(horizontal, vertical);

            if (vertical >= stepForJump)
                jumpTimer = Time.time + .25f;
        }

        // Move and rotate player function
        private void Move(float horizontal)
        {
            rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
            animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));

            if ((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight))
                Flip();
        }

        private void ModifyPhysics()
        {
            var changingDirection = (direction.x > 0 && rb.velocity.x < 0) || (direction.x < 0 && rb.velocity.x > 0);

            if (onGround)
            {
                if (Mathf.Abs(direction.x) < .4f || changingDirection)
                    rb.drag = linearDrag;
                else
                    rb.drag = 0f;
                rb.gravityScale = 0;
            }
            else
            {
                rb.gravityScale = gravity;
                rb.drag = linearDrag * 0.15f;
                if (rb.velocity.y < 0)
                    rb.gravityScale = gravity * fallMultiplier;
                else if (rb.velocity.y > 0 && direction.y < stepForJump)
                    rb.gravityScale = gravity * (fallMultiplier / 2);
            }
        }

        private void Flip()
        {
            CreateDust();
            facingRight = !facingRight;
            transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
            checkRadiusAttack *= -1;
        }

        // Jumping
        private void Jump()
        {
            CreateDust();
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpTimer = 0;
            StartCoroutine(JumpSqueeze(.5f, 1.2f, .1f));
        }

        private IEnumerator JumpSqueeze(float xSqueeze, float ySqueeze, float seconds)
        {
            Vector3 originalSize = Vector3.one;
            Vector3 newSize = new Vector3(xSqueeze, ySqueeze, originalSize.z);
            float t = 0f;
            while (t <= 1.0)
            {
                t += Time.deltaTime / seconds;
                transform.localScale = Vector3.Lerp(originalSize, newSize, t);
                yield return null;
            }

            t = 0f;
            while (t <= 1.0)
            {
                t += Time.deltaTime / seconds;
                transform.localScale = Vector3.Lerp(newSize, originalSize, t);
                yield return null;
            }
        }

        // Attack function
        private void OnAttack()
        {
            animator.SetTrigger("Attack");

            var hit = Physics2D.Raycast(attackCheck.position, Vector2.right, checkRadiusAttack, whatIsTetrisBlock);
            if (hit.collider != null && hit.collider.gameObject.CompareTag("Block"))
            {
                hit.transform.parent.GetComponent<TetrisLife>().DeleteChildBox(hit.transform);
            }
        }

        private void GetDamage()
        {
            var damagePos = transform.position + new Vector3(.05f, .75f, 0);
            var checkDamage = Physics2D.Raycast(damagePos, Vector2.up, checkRadius, whatIsTetrisBlock);

            if (!checkDamage)
                return;
            
            gameObject.transform.localScale = new Vector3(1.25f, .25f, 1);
            GameManager.Lose?.Invoke();
            Destroy(gameObject);
        }

        // FX
        private void CreateDust()
        {
            dust.Play();
        }
    }
}