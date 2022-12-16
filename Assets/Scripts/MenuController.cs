using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public  GameObject escMenu;
    // Start is called before the first frame update
    void Start()
    {
        // escMenu=GameObject.Find("Menu");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(escMenu.activeInHierarchy)
                escMenu.SetActive(false);
            else
                escMenu.SetActive(true);

        }
    }
    public void OnEscButtonClick()
    {
        Application.Quit();
    }
    public void OnReturnButtonClick()
    {
        escMenu.SetActive(false);
    }
    
}
