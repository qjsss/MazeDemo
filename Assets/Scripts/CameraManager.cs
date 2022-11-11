using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CameraManager : MonoBehaviour
{
    public Camera myCamera;
    public InputField RowInput;
    public InputField ColumnInput;
    public int wallWidth=1;
    public float mouseCenter;
    public float maxView=90;
    public float minView=30;

    public float slideSpeed=20;
    private void Update() {
        mouseCenter = Input.GetAxis("Mouse ScrollWheel");
        //copy and learn from https://www.cnblogs.com/alanshreck/p/13559303.html
        if(mouseCenter<0){
            if(myCamera.fieldOfView<=maxView){
                myCamera.fieldOfView+=10*slideSpeed*Time.deltaTime;
            }
        }
        else if(mouseCenter>0){
            if(myCamera.fieldOfView>=minView){
                myCamera.fieldOfView-=10*slideSpeed*Time.deltaTime;
            }
        }
    }
    public void OnStartButtonClick()
    {
        int row=int.Parse(RowInput.text);
        int column=int.Parse(ColumnInput.text);
        Vector3 v3=new Vector3(row*(wallWidth+1)/2f,(row+column)*(wallWidth+1)/1.5f,column*(wallWidth+1)/2f);
        myCamera.transform.position=v3;
    }
    
}
