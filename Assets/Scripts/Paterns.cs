using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paterns : MonoBehaviour
{
    private int[] shipCount = new[] {10, 10};
    public void MoveLeft(float speed, Transform transform)
    {
        transform.position += Vector3.left * (Time.deltaTime * speed);
    }
    
    public  void MoveRight(float speed, Transform transform)
    {
        transform.position += Vector3.right * (Time.deltaTime * speed);
    }
    
    public void MoveUp(float speed)
    {
        transform.position += Vector3.up * (Time.deltaTime * speed);
    }
    
    public void MoveDown(float speed, Transform transform)
    {
        transform.position += Vector3.down * (Time.deltaTime * speed);
    }
}
