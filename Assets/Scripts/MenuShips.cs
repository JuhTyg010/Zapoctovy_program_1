using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MenuShips : MonoBehaviour
{
    [SerializeField] private List<GameObject> ships;
    [SerializeField] private GameObject chooseShipPanel;
    [SerializeField] private GameObject menuPanel;

    public Image Preview;
    public Image Preview2;
    public TextMeshProUGUI Health;
    public TextMeshProUGUI Speed;
    public TextMeshProUGUI Damage;
    
    private int selectedId;
    void Start()
    {
        //load data to show last chossen
        selectedId = 1;
        Load(selectedId);
    }

    // Update is called once per frame
    public void OnSelectShip()
    {
        chooseShipPanel.SetActive(true);
        menuPanel.SetActive(false);
        Load(selectedId);
    }

    public void OnRight()
    {
        selectedId += 1;
        if (selectedId > ships.Count)
        {
            selectedId = 1;
        }
        Load(selectedId);
    }

    public void OnLeft()
    {
        selectedId -= 1;
        if (selectedId < 1)
        {
            selectedId = ships.Count;
        }
        Load(selectedId);
    }

    public void OnChoose()
    {
        menuPanel.SetActive(true);
        chooseShipPanel.SetActive(false);
    }

    void Load(int id)
    {
        GameObject choosen = ships[0];
        foreach (var ship in ships)
        {
            if (ship.GetComponent<PlayerShip>().shipID == id)
            {
                choosen = ship;
                break;
            }
        }

        PlayerShip choosenShip = choosen.GetComponent<PlayerShip>();
        Preview.sprite = choosen.GetComponentInChildren<SpriteRenderer>().sprite;
        Preview2.sprite = choosen.GetComponentInChildren<SpriteRenderer>().sprite;
        Health.text = choosenShip.health.ToString();
        Speed.text = choosenShip.speed.ToString();
        Damage.text = Mathf.RoundToInt(choosenShip.bulletDamage / choosenShip.reloadTime).ToString();

    }
}