using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paterns : MonoBehaviour
{
    //this simple methods are used to move the enemies in a specific direction via the delegate
    public void Left(float speed, Transform transform)
    {
        transform.position += Vector3.left * (Time.deltaTime * speed);
    }
    
    public  void Right(float speed, Transform transform)
    {
        transform.position += Vector3.right * (Time.deltaTime * speed);
    }
    
    public void Up(float speed, Transform transform)
    {
        transform.position += Vector3.up * (Time.deltaTime * speed);
    }
    
    public void Down(float speed, Transform transform)
    {
        transform.position += Vector3.down * (Time.deltaTime * speed);
    }
    
    public void LeftUp(float speed, Transform transform)
    {
        transform.position += (Vector3.left + Vector3.up) * (Time.deltaTime * speed);
    }
    
    public void LeftDown(float speed, Transform transform)
    {
        transform.position += (Vector3.left + Vector3.down) * (Time.deltaTime * speed);
    }
    
    public void RightUp(float speed, Transform transform)
    {
        transform.position += (Vector3.right + Vector3.up) * (Time.deltaTime * speed);
    }
    
    public void RightDown(float speed, Transform transform)
    {
        transform.position += (Vector3.right + Vector3.down) * (Time.deltaTime * speed);
    }
}
