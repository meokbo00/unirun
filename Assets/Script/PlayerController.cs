using UnityEngine;

public class PlayerController : MonoBehaviour {
   public AudioClip deathClip;
   public float jumpForce = 700f;

   private int jumpCount = 0; 
   private bool isGrounded = false; 
   private bool isDead = false;

   private Rigidbody2D playerRigidbody; 
   private Animator animator; 
   private AudioSource playerAudio; 

   private void Start() {
        animator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        playerRigidbody = GetComponent<Rigidbody2D>();
   }

   private void Update() {
      if(isDead) return;
      if(Input.GetMouseButtonDown(0) && jumpCount < 2)
        {
            jumpCount++;
            playerRigidbody.velocity = Vector2.zero;
            playerRigidbody.AddForce(new Vector2(0,jumpForce));
            playerAudio.Play();
        }
      else if(Input.GetMouseButtonUp(0) && playerRigidbody.velocity.y > 0)
        {
            playerRigidbody.velocity = playerRigidbody.velocity * 0.5f;
        }
        animator.SetBool("Grounded", isGrounded);
   }

   private void Die() {
        animator.SetTrigger("Die");
        playerAudio.clip = deathClip;
        playerAudio.Play();
        playerRigidbody.velocity = Vector2.zero;
        isDead = true;
   }

   private void OnTriggerEnter2D(Collider2D other) {
       // 트리거 콜라이더를 가진 장애물과의 충돌을 감지
       if(other.tag == "Dead" && !isDead)
        {
            Die();
        }
   }

   private void OnCollisionEnter2D(Collision2D collision) {
        // 바닥에 닿았음을 감지하는 처리
        if (collision.contacts[0].normal.y > 0.7f)
        {
            isGrounded = true;
            jumpCount = 0;
        }
   }

   private void OnCollisionExit2D(Collision2D collision) {
       // 바닥에서 벗어났음을 감지하는 처리
       isGrounded = false;
   }
}