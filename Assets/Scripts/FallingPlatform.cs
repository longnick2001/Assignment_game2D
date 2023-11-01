using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    private Rigidbody2D rb;

    private Collider2D collider2D;

    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        // rb.simulated = false;
        collider2D.isTrigger = false;
        PlayerPrefs.DeleteKey(gameObject.name);
    }
    
    // Coroutine để bật lại thuộc tính RigidBody2D sau 2 giây
    private IEnumerator EnableRigidbody2D(float time)
    {
        yield return new WaitForSeconds(time);
        rb.isKinematic = false;
        // Set thuộc tính isTrigger của Collider2D thành true
        collider2D.isTrigger = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            string name = gameObject.name;
            PlayerPrefs.SetString(name, name);
            // Sử dụng Coroutine để bật lại thuộc tính RigidBody2D sau 0.5 giây
            StartCoroutine(EnableRigidbody2D(0.5f));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            anim.SetTrigger("grounded");
            rb.simulated = false;
            Destroy(gameObject, 0.5f);
        }
    }
}
