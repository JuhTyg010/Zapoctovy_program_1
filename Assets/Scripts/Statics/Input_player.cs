using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyInput
{ 
    
    //GetDirection() returns a normalized Vector2 with the direction of the input
    //where the input is the arrow keys or WASD
    public static Vector2 GetDirection(float verticalMultiplier, float horizontalMultiplier)
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
    
    //IsShooting() returns true if the player is holding space or input key selected for "Fire1"
    public static bool IsShooting()
    {
        return Input.GetButton("Fire1");
    }

    public static bool IsPause()
    {
            return Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P);
    }
    
}

public static class Helper
{
    
    //ShipBaseParams is a struct that holds the most basic parameters for a ship
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
    
    //FindComponentsInChildrenWithTag() returns an array of components of type T
    //that are children of the parent and have the tag specified
    //forceActive is used to include inactive objects in the search in children
    //than you just run through the array and remove a different tagged components
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
    
    //ChildrenWithTag() returns an array of GameObjects
    //that are children of the parent and have the tag specified
    //it uses FindComponentsInChildrenWithTag() to find the components and then
    //moves up them to GameObjects
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
