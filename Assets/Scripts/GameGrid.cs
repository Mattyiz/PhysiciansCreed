using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{

    [SerializeField] int gridX = 3;
    [SerializeField] int gridY = 3;
    public GameObject[,] grid; //grid is [y,x] sorry but we've come to far to change it

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
