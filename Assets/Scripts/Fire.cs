using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            Invoke("setTouched",0.2f);
            Invoke("setFire",0.4f);
            Invoke("setIdle", 0.6f);
        }
    }
    
    void setTouched()
    {
        anim.SetBool("touched", true);
    }
    
    void setFire()
    {
        anim.SetBool("makefire", true);
        anim.SetBool("touched", false);
    }

    void setIdle()
    {
        anim.SetBool("makefire", false);
    }
}
