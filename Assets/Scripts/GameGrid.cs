using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{

    [SerializeField] private int gridX;
    [SerializeField] private int gridY;
    public GameObject[,] grid; //grid is [y,x] sorry but we've come to far to change it
    public GameObject moneyText;
    public int carryOverMoney;
    public int newRoundMoney;

    // Start is called before the first frame update
    void Start()
    {
        
        //Populates grid
        grid = new GameObject[gridX, gridY];

        for (int i = 0; i < transform.childCount; i++)
        {
            for (int j = 0; j < transform.GetChild(i).childCount; j++)
            {
                grid[i, j] = transform.GetChild(i).GetChild(j).gameObject;
                grid[i, j].GetComponent<GridSpace>().gridManager = this;
                grid[i, j].GetComponent<GridSpace>().x = j;
                grid[i, j].GetComponent<GridSpace>().y = i;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void UpdateUI(int newMoney)
    {
        if(newMoney <= -100)
        {
            newMoney = 0;
            newRoundMoney = 0;
        }

        newRoundMoney += newMoney;
        moneyText.GetComponent<TMPro.TextMeshProUGUI>().text = "Money: " + carryOverMoney + " (" + newRoundMoney + ")";


    }

    public int ChargePatients()
    {
        int money = 0;

        for (int i = 0; i < transform.childCount; i++)
        {
            for (int j = 0; j < transform.GetChild(i).childCount; j++)
            {
                if(grid[i, j].GetComponent<GridSpace>().heldPatient != null)
                {
                    money += grid[i, j].GetComponent<GridSpace>().heldPatient.GetComponent<Patient>().money;
                    grid[i, j].GetComponent<GridSpace>().heldPatient.GetComponent<Patient>().ClearHolder();
                }
            }
        }

        return money;
    }
}
