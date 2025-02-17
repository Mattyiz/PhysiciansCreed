using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
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

    /// <summary>
    /// Updates the money text box
    /// </summary>
    /// <param name="newMoney">Change to the money</param>
    public void UpdateUI(int newMoney)
    {
        //Resets the money
        if(newMoney <= -1000)
        {
            newMoney = 0;
            newRoundMoney = 0;
        }

        newRoundMoney += newMoney;
        moneyText.GetComponent<TMPro.TextMeshProUGUI>().text = "Funds Earned: $" + newRoundMoney;

    }

    /// <summary>
    /// Clears the grid and returns the total money of the patients treated
    /// </summary>
    /// <returns>Total money of all the patients in the grid</returns>
    public int ChargePatients()
    {
        int money = 0;

        for (int i = 0; i < transform.childCount; i++)
        {
            for (int j = 0; j < transform.GetChild(i).childCount; j++)
            {
                if(grid[i, j].GetComponent<GridSpace>().heldPatient != null)
                {

                    Patient gridPatient = grid[i, j].GetComponent<GridSpace>().heldPatient.GetComponent<Patient>();
                    if(gridPatient.treatedThisWeek == false) // So the same patient isn't treated
                    {
                        gridPatient.treatedThisWeek = true;

                        if(gridPatient.patientData.treatmentLength <= 1 && !gridPatient.locked) // Clear Patient and Get Money
                        {
                            DayManager.Instance.patientsDischarged++;
                            DayManager.Instance.allSavedPatients.Add(gridPatient.patientData);
                            money += gridPatient.patientData.funds;
                            gridPatient.ClearHolder();
                        }else if(gridPatient.patientData.treatmentLength > 1 && !gridPatient.locked) // Lock Patient, Get Money, Subtract Treatment Length
                        {
                            DayManager.Instance.patientsLocked++;
                            money += gridPatient.patientData.funds;
                            gridPatient.patientData.treatmentLength--;
                            gridPatient.locked = true;
                            //Debug.Log("Locking Patient");
                        }else if(gridPatient.patientData.treatmentLength > 1 && gridPatient.locked) // Subtract Treatment Length
                        {
                            //Debug.Log("Already Locked Patient Subtraction");
                            DayManager.Instance.patientsLocked++;
                            gridPatient.patientData.treatmentLength--;
                        }
                        else if(gridPatient.patientData.treatmentLength <= 1 && gridPatient.locked) // Clear Patient
                        {
                            gridPatient.locked = false;
                            //Debug.Log("Locked Finally Treated");
                            DayManager.Instance.patientsDischarged++;
                            DayManager.Instance.allSavedPatients.Add(gridPatient.patientData);
                            gridPatient.ClearHolder();
                        }
                    }
                }
            }
        }

        return money;
    }
}
