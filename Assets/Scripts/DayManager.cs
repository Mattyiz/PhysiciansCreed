using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DayManager : MonoBehaviour
{

    [Header("Quota")]
    [SerializeField] private int quota;
    [SerializeField] private int currentMoney;
    [SerializeField] private int firstDayQuota;

    [Header("Game Objects")]
    [SerializeField] private GameObject grid;
    [SerializeField] private GameObject patientManager;
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject waitingRoom;
    [SerializeField] private GameObject endButton;
    [SerializeField] private GameObject quotaText;

    // Start is called before the first frame update
    void Start()
    {
        currentMoney = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Starts the day by getting quota, spawning the patients, and enabling the grid
    /// </summary>
    public void StartDay()
    {
        if(quota <= 0)
        {
            quota = firstDayQuota;
        }
        else
        {
            quota = quota + (quota / 10);
        }

        quotaText.GetComponent<TMPro.TextMeshProUGUI>().text = "Quota: " + quota;

        patientManager.GetComponent<PatientManager>().SpawnPatients();
        grid.SetActive(true);
        waitingRoom.SetActive(true);

        startButton.SetActive(false);
        endButton.SetActive(true);
    }

    /// <summary>
    /// Ends the day by getting the money made and comparing it against the quota
    /// </summary>
    public void EndDay()
    {

        currentMoney += grid.GetComponent<GridManager>().ChargePatients();

        //Game over if didn't meet quota
        if(quota <= currentMoney)
        {
            currentMoney -= quota;
        }
        else
        {
            GameOver();
        }

        //Updates money UI
        grid.GetComponent<GridManager>().carryOverMoney = currentMoney;
        grid.GetComponent<GridManager>().UpdateUI(-1000);

        //Treats patients, disables grid
        patientManager.GetComponent<PatientManager>().TreatPatients();
        grid.SetActive(false);
        waitingRoom.SetActive(false);
        startButton.SetActive(true);
        endButton.SetActive(false);
    }

    /// <summary>
    /// Goes to game over scene
    /// </summary>
    private void GameOver()
    {
        SceneManager.LoadScene("Game Over");
    }
}
