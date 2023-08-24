using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] private GameObject mainBackground;
    [SerializeField] private GameObject[] secondaryBackgrounds;
    [SerializeField] private float speed;
    [SerializeField] private float offset;
    [SerializeField] private float blurDelta;
    
    private GameObject _main1;
    private GameObject _main2;
    private float _blurTimer;

    //Start is called before the first frame update
    //_main1 and _main2 are the two main backgrounds that are constantly moving down
    //The blurTimer is used to spawn a new background every blurDelta seconds
    void Start()
    {
        _main1 = Instantiate(mainBackground, transform.position, Quaternion.identity);
        _main2 = Instantiate(mainBackground, transform.position + Vector3.up * offset, Quaternion.identity);
        Movement first = _main1.AddComponent<Movement>();
        first.SetMovement(speed, Vector2.down);
        
        Movement second = _main2.AddComponent<Movement>();
        second.SetMovement(speed, Vector2.down);
        
        _blurTimer = blurDelta;
        
    }

    // Update is called once per frame
    // If the main backgrounds are below the offset, they are moved back up
    // If the blurTimer is below 0, a new blur background is spawned
    // blur is selected randomly from the secondaryBackgrounds array
    // movement of blur is same as main backgrounds, but it is destroyed after a certain amount of time
    // when they leave the player's view
    void Update()
    {
        if ( _main1.transform.position.y <=  -offset)
        {
            _main1.transform.position = transform.position + Vector3.up * offset;
        }
        else if (_main2.transform.position.y <=  -offset)
        {
            _main2.transform.position = transform.position + Vector3.up * offset;
        }
        
        _blurTimer -= Time.deltaTime;
        if (_blurTimer < 0)
        {
            _blurTimer = blurDelta;
            int index = Random.Range(0, secondaryBackgrounds.Length);
            GameObject blur = Instantiate(secondaryBackgrounds[index], transform.position + ((Vector3) Vector2.up * offset), Quaternion.identity);
            Movement blurMovement = blur.AddComponent<Movement>();
            blurMovement.SetMovement(speed, Vector2.down);
            Lifetime lifetime = blur.AddComponent<Lifetime>();
            lifetime.SetLifetime((offset / speed) * 2);
            
        }
    }
}
