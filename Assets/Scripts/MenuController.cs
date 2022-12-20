using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public  GameObject escMenu;
    // Start is called before the first frame update
    void Start()
    {
        var menu=GameObject.Find("Menu");
        if(menu!=null)menu.SetActive(false);
        if(escMenu==null&&menu!=null)escMenu=menu;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            OnEscapeKeyDown();
    }
    private void OnEscapeKeyDown()
    {
        if(escMenu.activeInHierarchy)
            escMenu.SetActive(false);
        else
            escMenu.SetActive(true);
    }
    public void OnReloadButtonClick()
    {
        SceneManager.LoadScene(0);
    }
    public void OnEscButtonClick()
    {
        OnEscapeKeyDown();
    }
    public void OnReturnButtonClick()
    {
        escMenu.SetActive(false);
    }
    public void OnQuitButtonClick()
    {
        Application.Quit();      
    }
    
}
