using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSizeOverTime : MonoBehaviour
{

    [SerializeField] private Vector2 sizeChange;
    [SerializeField] private float speed;
    
    void Update()
    {
        transform.localScale += (Vector3) sizeChange * (Time.deltaTime * speed);
    }
}
