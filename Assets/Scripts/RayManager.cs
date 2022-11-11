﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayManager : MonoBehaviour
{
    public RaycastHit hitInfo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray,out hitInfo))
            {
                Debug.Log(hitInfo.transform.position);
            }
        }
    }
}
