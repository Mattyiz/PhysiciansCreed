using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DayManager : MonoBehaviour
{

    [Header("Quota")]
    [SerializeField] private int quota;
    [SerializeField] private int currentMoney;
    [SerializeField] private int firstDayQuota;

    [Header("Week Data")]
    private int currentWeek = 0;

    [Header("Game State")] // Where Players schedule patients
    [SerializeField] private GameObject gamePhaseUI;
    [SerializeField] private GameObject gamePhaseSprites;
    private bool gameStateActive;

    [Header("Summary State")] // Where Players recieve their results from the game phase
    [SerializeField] private GameObject summaryPhaseUI; // All the UI
    [SerializeField] private TextMeshProUGUI weekNumberText; // Week Number Display
    [SerializeField] private TextMeshProUGUI quotaResultText; // Quota Result Display
    [SerializeField] private TextMeshProUGUI treatmentsscheduledText; // Treatments Scheduled Display
    [SerializeField] private TextMeshProUGUI underObservationText; // Under Observation Display
    [SerializeField] private TextMeshProUGUI dischargedText; // Discharged Display
    [SerializeField] private TextMeshProUGUI awaitingTreatmentText; // Awaiting Treatment Display
    [SerializeField] private TextMeshProUGUI patientsLostText; // Patients Lost that Week Display
    [SerializeField] private TextMeshProUGUI nextQuotaText; // Patients Lost that Week Display
    [SerializeField] private TextMeshProUGUI adminstratorMessageText; // Patients Lost that Week Display
    [SerializeField] private GameObject summaryInfo; // Holds display text for the data that does not appear no the first day
    [SerializeField] private GameObject introText; // Intro for the player

    [Header("Game Objects")]
    [SerializeField] private GameObject grid;
    [SerializeField] private GameObject patientManager;
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject waitingRoom;
    [SerializeField] private GameObject endButton;
    [SerializeField] private GameObject quotaText;
    [SerializeField] private GameObject gameInfo;    


    // Start is called before the first frame update
    void Start()
    {
        // Set Data
        currentMoney = 0;

        // Display Intro
        introText.SetActive(true);
        summaryInfo.SetActive(false);

        // Hide Game State
        ChangeGameState(false);
    }

    /// <summary>
    /// Starts the day by getting quota, spawning the patients, and enabling the grid
    /// </summary>
    public void StartDay()
    {
        // Switch UI
        ChangeGameState(true);

        currentWeek++;

        if(currentWeek == 1)
        {
            quota = firstDayQuota;
            introText.SetActive(false);
        }
        else
        {
            quota = quota + (quota / 10);
        }

        quotaText.GetComponent<TMPro.TextMeshProUGUI>().text = "Quota: " + quota;

        patientManager.GetComponent<PatientManager>().SpawnPatients();
        grid.SetActive(true);
    }

    /// <summary>
    /// Ends the day by getting the money made and comparing it against the quota
    /// </summary>
    public void EndDay()
    {
        if(currentWeek == 1)
        {
            // Hide Intro and Display Summary
            introText.SetActive(false);
            summaryInfo.SetActive(true);
        }

        // Switch UI
        ChangeGameState(false);

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
    }

    /// <summary>
    /// Goes to game over scene
    /// </summary>
    private void GameOver()
    {
        SceneManager.LoadScene("Game Over");
    }

    private void ChangeGameState(bool activateGameState)
    {
        gameStateActive = activateGameState;

        // Update Parent UI States
        gamePhaseUI.SetActive(gameStateActive);
        gamePhaseSprites.SetActive(gameStateActive);
        summaryPhaseUI.SetActive(!gameStateActive);
    }
}
