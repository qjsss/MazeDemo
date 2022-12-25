using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RayManager : MonoBehaviour
{
    public RaycastHit hitInfo;
    private bool startSelectStartPoint;
    private bool startSelectFinishPoint;
    public InputField StartPointInput;
    public InputField FinishPointInput;
    private MazeGenerator mazeGenerator;
    public GameObject Panel;
    public int wallWidth;
    // Start is called before the first frame update
    void Start()
    {
        mazeGenerator=GameObject.Find("Main Camera").GetComponent<MazeGenerator>();
        wallWidth=mazeGenerator.wallWidth;
        Panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(startSelectStartPoint&&Input.GetMouseButtonDown(0))
        {
            Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray,out hitInfo))
            {
                Panel.SetActive(false);
                ChangeInputField(StartPointInput);
            }
            startSelectStartPoint=false;
        }
        if(startSelectFinishPoint&&Input.GetMouseButtonDown(0))
        {
            Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray,out hitInfo))
            {
                Panel.SetActive(false);
                ChangeInputField(FinishPointInput);
            }
            startSelectFinishPoint=false;
        }
        // if(Input.GetMouseButtonDown(0))
        // {
        //     Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);
        //     if(Physics.Raycast(ray,out hitInfo))
        //     {
        //         Debug.Log(hitInfo.transform.position);
        //     }
        // }
    }
    private void ChangeInputField(InputField inputField)
    {
        
        int x=(int)(hitInfo.transform.position.x)/(wallWidth+1)+1;
        int y=(int)(hitInfo.transform.position.z)/(wallWidth+1)+1;
        inputField.text="["+x.ToString()+","+y.ToString()+"]";
        if(inputField.name=="StartPointInput")
            mazeGenerator.ChangeStartPoint(x,y);
        else
            mazeGenerator.ChangeFinishPoint(x,y);
    }
    public void OnSelectStartPointClick()
    {
        Panel.SetActive(true);
        startSelectStartPoint=true;
    }
    public void OnSelectFinishPointClick()
    {
        Panel.SetActive(true);
        startSelectFinishPoint=true;
    }
}
