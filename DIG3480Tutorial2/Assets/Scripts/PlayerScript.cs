using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;

    Animator animator;

    public AudioClip musicClip1;
    public AudioClip musicClip2;

    public AudioSource musicSource;

    private bool facingRight = true;

    public float speed;

    public Text score;
    public Text lives;
    public Text win;
    public Text lose;

    private int scoreValue = 0;
    private int livesValue = 3;

    // Start is called before the first frame update
    void Start(){

        rd2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        musicSource.clip = musicClip1;
        musicSource.loop = true;
        musicSource.Play();

        win.text = "";
        lose.text = "";

    }

    // Update is called once per frame
    void Update(){

        score.text = "Score: " + scoreValue.ToString();
        lives.text = "Lives: " + livesValue.ToString();

        animator.SetInteger("State", 0);

        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");

        if (livesValue <= 0 || scoreValue >= 8){

            animator.SetInteger("State", 0);

        }

        else if (vertMovement > 0){

            animator.SetInteger("State", 2);

        }

        else if (hozMovement > 0 || hozMovement < 0){

            animator.SetInteger("State", 1);

        }

        if (facingRight == false && hozMovement > 0){

            Flip();

        }

        else if (facingRight == true && hozMovement < 0){

            Flip();

        }

    }

    void FixedUpdate(){

        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");

        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));

        if (scoreValue == 8){

            win.text = "You Win! Game Created by Ryan Hazelton";
            Destroy(this);

        }

        if (livesValue < 1){

            lose.text = "You Lose!";
            Destroy(this);
            musicSource.Stop();

        }

    }

    void Flip(){

        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;

    }

    private void OnCollisionEnter2D(Collision2D collision){

       if (collision.collider.tag == "Coin"){

            scoreValue += 1;
            Destroy(collision.collider.gameObject);

            if (scoreValue == 4){

                transform.position = new Vector2(43.0f, 0.0f);

                livesValue = 3;

            }

            if (scoreValue == 8){

                musicSource.Stop();
                musicSource.clip = musicClip2;
                musicSource.loop = false;
                musicSource.Play();

                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

                foreach(GameObject enemy in enemies){

                    GameObject.Destroy(enemy);

                }

            }

        }

        if (collision.collider.tag == "Enemy"){

            livesValue -= 1;
            Destroy(collision.collider.gameObject);

        }

    }

    private void OnCollisionStay2D(Collision2D collision){

        if (collision.collider.tag == "Ground"){

            if (Input.GetKey(KeyCode.W)){

                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse); //the 3 in this line of code is the player's "jumpforce," and you change that number to get different jump behaviors.  You can also create a public variable for it and then edit it in the inspector.

            }

        }

    }

}