using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class MouseController : MonoBehaviour
{
    public float jetpackForce;
    public ParticleSystem jetpack;
    
    private Rigidbody2D rb;
    
    public float forwardMovementSpeed;
   
    public Transform groundCheckTransform;
    public LayerMask groundCheckLayerMask;
    private bool grounded;
    private Animator animator;

    private bool dead = false;

    private uint coins = 0;
    public TMP_Text textCoins;

    public Button buttonRestart;
    public Button buttonMenu;

    public AudioClip coinCollidectSound;
    public AudioSource jetpackAudio;
    public AudioSource footstepsAudio;
    public AudioSource bgMusic;

    public ParallaxSctroll parallaxScroll;

    private uint level = 0;
    public TMP_Text textLevel;

    public bool Fever = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        textCoins.text = coins.ToString();
        StartCoroutine(LevelCount());
        textLevel.text = "Lv " + level.ToString();
        StartCoroutine(FeverTime());

        LoadVoluem();
    }

    private void FixedUpdate()
    {
        bool jetpckActive = Input.GetButton("Fire1");      

        if (!dead)
        {
            if (jetpckActive)
            {
                rb.AddForce(jetpackForce * Vector2.up);
            }
            Vector2 newVelocity = rb.velocity;
            newVelocity.x = forwardMovementSpeed;
            rb.velocity = newVelocity;
        }

        UPdateGroundedStatus();
        AdjustJetpack(jetpckActive);
        DisplayRestartButton();
        AdjustFootstepAndJetpackSound(jetpckActive);

        parallaxScroll.offset = transform.position.x;
    }

    private void AdjustJetpack(bool jetpackActive)
    {
        var emission = jetpack.emission;
        emission.enabled = !grounded;
        emission.rateOverTime = jetpackActive ? 300f : 75f;
    }

    private void UPdateGroundedStatus()
    {
        grounded = Physics2D.OverlapCircle(groundCheckTransform.position, 0.1f, groundCheckLayerMask);
        animator.SetBool("grounded", grounded);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Coins")
        {
            ColletCoin(collision);
        }
        else
            HitByLaser(collision);
    }

    private void ColletCoin(Collider2D coinCollider)
    {
        ++coins;
        textCoins.text = coins.ToString();
        Destroy(coinCollider.gameObject);

        AudioSource.PlayClipAtPoint(coinCollidectSound, transform.position);
    }

    private void HitByLaser(Collider2D laserCollider)
    {
        if (!dead)
        {
            AudioSource laser = laserCollider.GetComponent<AudioSource>();
            laser.Play();
        }
        dead = true;
        animator.SetBool("dead", true);
    }
    private void DisplayRestartButton()
    {
        bool active = buttonRestart.gameObject.activeSelf; // 가져오기(activeSelf)
        bool ac = buttonMenu.gameObject.activeSelf;
        if (grounded && dead && !active && !ac)
        {
            buttonRestart.gameObject.SetActive(true); // 활성화(SetActive)
            buttonMenu.gameObject.SetActive(true);
        }
    }

    public void OnClickedRestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void GoMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    private void AdjustFootstepAndJetpackSound(bool jetpackActive)
    {
        footstepsAudio.enabled = !dead && grounded;
        jetpackAudio.enabled = !dead && !grounded;
        jetpackAudio.volume = jetpackActive ? 1f : 0.5f;
    }

    IEnumerator LevelCount()
    {
        while(true)
        {
            yield return new WaitForSeconds(20f);

            level++;
            textLevel.text = "Lv " + level.ToString();
            forwardMovementSpeed += 0.5f;
            
        }
       
    }
    public IEnumerator FeverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(60f);
            forwardMovementSpeed *= 2f;
            Fever = true;
            yield return new WaitForSeconds(10f);
            forwardMovementSpeed /= 2f;
            Fever = false;
        }
    }

    //볼륨값 조절 Main값
    private void LoadVoluem()
    {
        float volume = PlayerPrefs.GetFloat("bgVolume");
        bgMusic.volume = volume;
    }
}
