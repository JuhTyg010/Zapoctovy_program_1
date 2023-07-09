using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
public class Animate
{
    float Rotate(float speed, float angle)
    {
        return speed * angle;
    }
    public Quaternion Rotate(float speed, float angle, int3 axis)
    {
        return new Quaternion(
            Rotate(speed, angle) *  axis.x ,
            Rotate(speed, angle) * axis.y,
            Rotate(speed, angle) * axis.z, 0f);
        
    }
}
public static class MyInput
{ 
    
    public static Vector2 GetDirection(float verticalMultiplier = 1, float horizontalMultiplier = 1)
    {
        Vector2 direction = Vector2.zero;
        direction.x = Input.GetAxisRaw("Horizontal") * horizontalMultiplier; 
        direction.y = Input.GetAxisRaw("Vertical") * verticalMultiplier;
        direction.Normalize();
        return direction;
    }
    
    public static Vector2 GetDirection(Vector2 multiplier)
    {
        return GetDirection(multiplier.y, multiplier.x);
    }
    public static Vector2 GetDirection()
    {
        return GetDirection(1, 1);
    }
    
    public static bool IsShooting()
    {
        return Input.GetButton("Fire1");
    }
    
}

public static class Helper
{
    public struct ShipBaseParams
    {
        public  float difficulty;
        public float spawnTime;
        public int shipID;
        
        public ShipBaseParams(float difficulty, float spawnTime, int shipID)
        {
            this.difficulty = difficulty;
            this.spawnTime = spawnTime;
            this.shipID = shipID;
        }
    }
    
    public static T[] FindComponentsInChildrenWithTag<T>(this GameObject parent, string tag, bool forceActive = false) where T : Component
    {
        if(parent == null) { throw new System.ArgumentNullException(); }
        if(string.IsNullOrEmpty(tag) == true) { throw new System.ArgumentNullException(); }
        List<T> list = new List<T>(parent.GetComponentsInChildren<T>(forceActive));
        if(list.Count == 0) { return null; }
 
        for(int i = list.Count - 1; i >= 0; i--) 
        {
            if (list[i].CompareTag(tag) == false)
            {
                list.RemoveAt(i);
            }
        }
        return list.ToArray();
    }
    
    public static GameObject[] ChildrenWithTag(this GameObject parent, string tag, bool forceActive = false)
    {

        Transform[] list = FindComponentsInChildrenWithTag<Transform>(parent, tag);
        
        if (list.Length == 0)
        {
            return null;
        }
        GameObject[] list2 = new GameObject[list.Length];
        for (int i = 0; i < list.Length; i++)
        {
            list2[i] = list[i].gameObject;
        }
        return list2;
    }
}
