using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    private Animator anim;
    public string key;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        PlayerPrefs.DeleteKey(gameObject.name);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            string name = gameObject.name;
            key = name;
            PlayerPrefs.SetString(key, name);
            anim.SetTrigger("collected");
            Destroy(gameObject, 0.5f);
        }
    }
}
