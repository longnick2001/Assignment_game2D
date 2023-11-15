using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fixx : MonoBehaviour
{
    private Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, 0);
        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(Vector3.zero) * 100;
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(mainCamera.rect.width, mainCamera.rect.height)) * 100;
        Vector3 screenSize = topRight - bottomLeft;
        float screenRatio = screenSize.x / screenSize.y;
        float disiredRatio = transform.localScale.x / transform.localScale.y;

        if (screenRatio > disiredRatio)
        {
            float height = screenSize.y;
            transform.localScale = new Vector3(height * disiredRatio, height);
        }else
        {
            float width = screenSize.x;
            transform.localScale = new Vector3(width, width / disiredRatio);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
