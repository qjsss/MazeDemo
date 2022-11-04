using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CameraManager : MonoBehaviour
{
    public Transform myCamera;
    public InputField RowInput;
    public InputField ColumnInput;
    public int wallWidth=1;
//     private void Update() {
//         float  mouseCenter = Input.GetAxis("Mouse ScrollWheel");
// 21      if(mouseCenter <0  ) {        
// 　　　　　　　　　　　//滑动限制
// 24                 if (myCamera.fieldOfView <= maxView) {
// 25                     c.fieldOfView += 10 * slideSpeed*Time.deltaTime;
// 26                     if (c.fieldOfView >= maxView) {
// 27                         c.fieldOfView = minView;
// 28                     }
// 29                 }     
// 30          //mouseCenter >0 = 正数 往前滑动,放大镜头
// 31         } else if (mouseCenter >0 ) {
// 　　　　　　　　　　　　//滑动限制
// 32                 if (c.fieldOfView >= minView) {
// 33                         c.fieldOfView -= 10 * slideSpeed*             
// 34                       Time.deltaTime;
// 35                     if (c.fieldOfView <= minView) {
// 36                         c.fieldOfView = maxView;
// 37                     }
// 38                  }      
// 39          }
//     }
    public void OnStartButtonClick()
    {
        int row=int.Parse(RowInput.text);
        int column=int.Parse(ColumnInput.text);
        Vector3 v3=new Vector3(row*(wallWidth+1)/2f,(row+column)*(wallWidth+1)/1.5f,column*(wallWidth+1)/2f);
        myCamera.position=v3;
    }
    
}
