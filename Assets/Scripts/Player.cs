using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Authentication;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Player : MonoBehaviour
{
    private bool isLeft, isRight, isTop;
    private float horizontalMove;
    
    public Button top;
    public AudioSource loseLife;
    public AudioSource death;
    public AudioSource nextLevel;
    public AudioSource finish;
    public AudioSource audioCollected;
    public AudioSource audioJump;
    public GameObject[] items;
    
    public static bool isWin;
    public Text txtHighScore;
    public Text txtYourScore;
    public int highScore = 0; 
    public GameObject panelWinGame;
    public GameObject panelGameOver;

    private bool isTouch = false;
    public static int numberOfHearts = 3;
    public Image[] hearts;
    public Sprite fullHeart;
    
    private int jumpCount = 0;
    
    private Rigidbody2D rb;
    public float jumpHeight = 8;
    private bool facingRight;
    private bool grounded;
    
    private Animator anim;
    public float speed = 6;
    public Text txtScore;
    public Text PlayerName;
    public static int score = 0;
    
    private void Awake()
    {
        isWin = false;
        if (PlayerPrefs.GetString("scene", null) != "" && MainMenu.isContinue)
        {
            Items();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        facingRight = true;
        PlayerName.text = PlayerPrefs.GetString("playerName","");
        
        if (PlayerPrefs.GetString("scene", null) != "" && MainMenu.isContinue)
        {
            MainMenu.isContinue = false;
            float playerX = PlayerPrefs.GetFloat("indexPlayerX");
            float playerY = PlayerPrefs.GetFloat("indexPlayerY");
            transform.position =
                new Vector2(playerX, playerY);

            score = PlayerPrefs.GetInt("scoreContinue");
            txtScore.text = "Score: " + score;
            numberOfHearts = PlayerPrefs.GetInt("numberOfHeart");

            // numberOfItem = PlayerPrefs.GetInt("numberOfItem");
            
        }
        else
        {
            score = PlayerPrefs.GetInt("score", 0);
            txtScore.text = "Score: " + score;
            numberOfHearts = 3;
        }
        transform.position = new Vector3(transform.position.x, transform.position.y+0.1f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //tạo biến di chuyển lấy dữ liệu từ bàn phím theo chiều ngang
        float move = Input.GetAxis("Horizontal");   
        if (move > 0 && !facingRight)
        {
            flip();
        }else if (move < 0 && facingRight)
        {
            flip();
        }
        //control player
        moving();
        runLeft();
        runRight();
        jump();
        playerName();
        Heart();
        MovePlayer();
    }

    void Heart()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < numberOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }
    
    void Items()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (PlayerPrefs.GetString(items[i].name) == items[i].name)
            {
                items[i].SetActive(false);
            }
        }
    }



    void jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            audioJump.Play();
            anim.SetFloat("idle", 0);
            anim.SetTrigger("jump");
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            jumpCount++;
        }else if (jumpCount < 2 && Input.GetKeyDown(KeyCode.Space))
        {
            audioJump.Play();
            anim.SetTrigger("doublejump");
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            jumpCount++;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            anim.SetTrigger("jump");
        }
    }
    

    void runLeft()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            anim.SetFloat("run", 1);
        }
        if(Input.GetKeyUp(KeyCode.A))
        {
            anim.SetFloat("run", 0);
        }
    }
    
    void runRight()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            anim.SetFloat("run", 1);
        }
        if(Input.GetKeyUp(KeyCode.D))
        {
            anim.SetFloat("run", 0);
        }
    }
    
    void flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    
    void moving()
    {
        // Lấy giá trị của các phím mũi tên
        float horizontalInput = Input.GetAxis("Horizontal");
        // Di chuyển nhân vật theo hướng đã tính toán
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            jumpCount = 0;
            grounded = true;
            anim.SetFloat("idle", 1);
        }

        if (other.gameObject.tag == "Falling")
        {
            jumpCount = 0;
            grounded = true;
            anim.SetFloat("idle", 1);
        }
        
        if (other.gameObject.tag == "Checkpoint")
        {
            nextLevel.Play();
            // Lưu điểm vào PlayerPrefs
            PlayerPrefs.SetInt("score", score);
            PlayerPrefs.Save();
             
            SceneManager.LoadSceneAsync(2); 
            Time.timeScale = 1f;
        }
        
        if (other.gameObject.tag == "Checkpoint2")
        {
            nextLevel.Play();
            // Lưu điểm vào PlayerPrefs
            PlayerPrefs.SetInt("score", score);
            PlayerPrefs.Save();
            SceneManager.LoadSceneAsync(3); 
            Time.timeScale = 1f;
        }
        
        if (other.gameObject.tag == "Win")
        {
            finish.Play();
            
            // Lưu điểm vào PlayerPrefs
            highScore = PlayerPrefs.GetInt("highscore", 0);
            if (highScore < score)
            {
                PlayerPrefs.SetInt("highscore", score);
            }
            
            PlayerPrefs.DeleteKey("score");
            PlayerPrefs.DeleteKey("indexPlayerX");
            PlayerPrefs.DeleteKey("indexPlayerY");
            PlayerPrefs.DeleteKey("scene");
            PlayerPrefs.DeleteKey("scoreContinue");
            PlayerPrefs.DeleteKey("numberOfHeart");
            
            PlayerPrefs.Save();
            txtYourScore.text = "Your score: " + score;
            txtHighScore.text = "Your score: " + highScore;
            isWin = true;
            
            panelWinGame.SetActive(true);
            Time.timeScale = 0f;
        }

        if (other.gameObject.CompareTag("Fire"))
        {
            
            if (!isTouch)
            {
                isTouch = true;
                StartCoroutine(DamageOverTime());
                
            }
            jumpCount = 0;
            grounded = true;
            anim.SetFloat("idle", 1);
        }

        if (other.gameObject.tag == "Spikes")
        {
            jumpCount = 0;
            grounded = true;
            anim.SetFloat("idle", 1);
            lifeCutOff();
        }
    }
    
    IEnumerator DamageOverTime()
    {
        // Chờ 0.4 giây
        yield return new WaitForSeconds(0.4f);
        
        // Nếu nhân vật vẫn đang đứng trên bẫy thì trừ điểm máu
        if (isTouch)
        {
            isTouch = false;
            // Trừ điểm máu của nhân vật
            lifeCutOff();
            
        }
    }


    public void lifeCutOff()
    {
        if (numberOfHearts <= 3)
        {
            loseLife.Play();
            numberOfHearts--;
            isTouch = false;
            anim.SetFloat("idle", 0);
            anim.SetTrigger("hit");
            rb.AddForce(new Vector2(8f, 8f), ForceMode2D.Impulse);
        }
        if (numberOfHearts == 0)
        {
            death.Play();
            MainMenu.isContinue = false;
            // Destroy(gameObject);
            Time.timeScale = 0f;
            panelGameOver.SetActive(true);
        }
    }
    
    void playerName()
    {
        Vector3 vector3 = transform.position;
        vector3.y = vector3.y + 0.8f;
        PlayerName.transform.position = vector3;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            grounded = false;
        }
        if (other.gameObject.tag == "Falling")
        {
            grounded = false;
        }
        if (other.gameObject.CompareTag("Fire"))
        {
            isTouch = false;
            grounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Fruit")
        {
            audioCollected.Play();
            other.gameObject.tag = "Finish";
            score += 10; 
            txtScore.text = "Score: " + score;
        }
    }
    
    public void Top()
    {
        if (grounded)
        {
            audioJump.Play();
            anim.SetFloat("idle", 0);
            anim.SetTrigger("jump");
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            jumpCount++;
        }else if (jumpCount < 2)
        {
            audioJump.Play();
            anim.SetTrigger("doublejump");
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            jumpCount++;
        }
    }
    
    public void PoiterDownLeft()
    {
        if (facingRight)
        {
            flip();
        }
        anim.SetFloat("run", 1);
        isLeft = true;
    }
    
    public void PoiterUpLeft()
    {
        anim.SetFloat("run", 0);
        isLeft = false;
    }
    
    public void PoiterDownRight()
    {
        if(!facingRight)
        {
            flip();
        }
        anim.SetFloat("run", 1);
        isRight = true;
    }
    
    public void PoiterUpRight()
    {
        anim.SetFloat("run", 0);
        isRight = false;
    }

    private void MovePlayer()
    {
        if (isLeft)
        {
            horizontalMove = -speed;
        }
        else if(isRight)
        {
            horizontalMove = speed;
        }
        else
        {
            horizontalMove = 0;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontalMove, rb.velocity.y);
    }
}
