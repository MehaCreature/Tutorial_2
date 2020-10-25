using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rd2d; 
    public float speed;
    public Text score;
    private int scoreValue = 0;
    public Text win;
    public Text livesCount;
    private int lives;
    public float threshold;
    public float maxVelocity = 7F;
    private bool facingRight = true;
    private bool hasTeleported;

    public AudioClip musicClipOne;

    public AudioClip musicClipTwo;

    public AudioClip musicClipThree;

    public AudioSource musicSource;

    Animator anim;


    // Start is called before the first frame update
    void Start()
    {
       rd2d = GetComponent<Rigidbody2D>();
       score.text = scoreValue.ToString("Score: 0");

       Victory();

       musicSource.clip = musicClipOne;
       musicSource.Play();

       win.text = "";
       lives = 3;
       SetLivesCount();

       anim = GetComponent<Animator>();

       anim.SetInteger("State", 1);

       hasTeleported = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float verMovement = Input.GetAxis("Vertical");

        rd2d.AddForce(new Vector2(hozMovement * speed, verMovement * speed));
        
        if(rd2d.velocity.magnitude >maxVelocity)
        {
            rd2d.velocity = rd2d.velocity.normalized * maxVelocity;
        }

        if(scoreValue >=10 )
       {
            win.text = "You win! Game by Felix Padilla";
            Victory();
           // musicSource.Stop();
           // musicSource.clip = musicClipThree;
           // musicSource.Play();
       }
        if(scoreValue == 4 && hasTeleported == false)
        {
            transform.position = new Vector2(73.0F, 0.1F);
            hasTeleported = true;
        }

        if (Input.GetKey("escape"))
       {
            Application.Quit();
        }

        //if (Input.GetKeyDown(KeyCode.D))
       // {
           // anim.SetInteger("State", 3);
       // }
       // else if(Input.GetKeyUp(KeyCode.D))
       // {
            //anim.SetInteger("State", 1);
       // }

        //if(Input.GetKeyDown(KeyCode.A))
          //{
             // anim.SetInteger("State", 3);
          //}
         // else if(Input.GetKeyUp(KeyCode.A))
         // {
             // anim.SetInteger("State", 1);
         // }

        if(transform.position.y <threshold && scoreValue >4)
          {
              lives = lives - 1;
              SetLivesCount();
              transform.position = new Vector2(73.0F, 0.01F);
          }
        else if (transform.position.y <threshold)
          {
             lives = lives - 1;
             SetLivesCount();
             transform.position = new Vector2(0.0F, 0.01F);
          }

          if(facingRight == false && hozMovement >0)
          {
              Flip();
          }
          else if(facingRight == true && hozMovement <0)
          {
              Flip();
          }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Coin")
        {
            scoreValue +=1;
            score.text = "Score: " + scoreValue.ToString();
            Destroy(collision.collider.gameObject);
        }
        else if(collision.collider.tag == "Enemy")
        {
            lives = lives - 1;
            SetLivesCount();
            Destroy(collision.collider.gameObject);
            anim.SetInteger("State", 6);
        }
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground")
        {
             if(Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
                anim.SetInteger("State", 4);
            }
            else if(Input.GetKey(KeyCode.D))
            {
                anim.SetInteger("State", 3);
            }
            else if(Input.GetKey(KeyCode.A))
            {
                anim.SetInteger("State", 3);
            }
            else anim.SetInteger("State", 1);
        }
    }
    void SetLivesCount()
        {
            livesCount.text = "Lives: " + lives.ToString();
            if(lives == 0)
            {   
                win.text = "GAME OVER";
                rd2d.constraints = RigidbodyConstraints2D.FreezeAll;
                rd2d.velocity = Vector3.zero;
                //Destroy();
                anim.SetInteger("State", 5);
                musicSource.Stop();
                musicSource.clip = musicClipTwo;
                musicSource.Play();
            }
        }

        void Victory()
        {
            if(scoreValue >=10)
            {
                musicSource.Stop();
                musicSource.clip = musicClipThree;
                musicSource.Play();
            }
        }
        void Flip()
        {
            facingRight = !facingRight;
            Vector2 Scaler = transform.localScale;
            Scaler.x = Scaler.x * -1;
            transform.localScale = Scaler;
        }
}
