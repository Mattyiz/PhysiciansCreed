using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{

    [SerializeField] int gridX = 3;
    [SerializeField] int gridY = 3;
    private GameObject[,] grid;

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
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        /* Test that it populates ok
        foreach(GameObject gameObject in grid)
        {
            Debug.Log(gameObject);
        }*/
    }
}
