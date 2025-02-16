using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DayManager : MonoBehaviour
{

    [Header("Quota")]
    [SerializeField] private int quota;
    [SerializeField] private int fundsEarned = 0;
    [SerializeField] private int firstDayQuota;
    [SerializeField] private bool quotaMet = false;
    [SerializeField] private GameObject quotaText;

    [Header("Week Data")]
    [SerializeField] private int currentWeek = 0;
    [SerializeField] private int patientsScheduled = 0;
    [SerializeField] private int patientsLocked = 0;
    [SerializeField] private int patientsDischarged = 0;
    [SerializeField] private int patientsWaiting = 0;
    [SerializeField] private int patientsLost = 0;

    [Header("Game State")] // Where Players schedule patients
    [SerializeField] private GameObject gamePhaseUI;
    [SerializeField] private GameObject gamePhaseSprites;
    private bool gameStateActive;

    [Header("Summary State")] // Where Players recieve their results from the game phase
    [SerializeField] private GameObject summaryPhaseUI; // All the UI
    [SerializeField] private TextMeshProUGUI weekNumberText; // Week Number Display
    [SerializeField] private TextMeshProUGUI quotaResultText; // Quota Result Display
    [SerializeField] private TextMeshProUGUI treatmentsScheduledText; // Treatments Scheduled Display
    [SerializeField] private TextMeshProUGUI underObservationText; // Under Observation Display
    [SerializeField] private TextMeshProUGUI dischargedText; // Discharged Display
    [SerializeField] private TextMeshProUGUI awaitingTreatmentText; // Awaiting Treatment Display
    [SerializeField] private TextMeshProUGUI patientsLostText; // Patients Lost that Week Display
    [SerializeField] private TextMeshProUGUI nextQuotaText; // Patients Lost that Week Display
    [SerializeField] private TextMeshProUGUI adminstratorMessageText; // Patients Lost that Week Display
    [SerializeField] private GameObject summaryInfo; // Holds display text for the data that does not appear no the first day
    [SerializeField] private GameObject introText; // Intro for the player

    [Header("Buttons")]
    [SerializeField] private GameObject startWeek;
    [SerializeField] private GameObject endGame;


    [Header("Game Objects")]
    [SerializeField] private GameObject grid;
    [SerializeField] private GameObject patientManager;


    // Start is called before the first frame update
    void Start()
    {
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

        // Update Week Data
        NewWeekData();
        
        // Update UI
        quotaText.GetComponent<TMPro.TextMeshProUGUI>().text = "Quota: " + quota;

        // Spawn Patients
        patientManager.GetComponent<PatientManager>().SpawnPatients();
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

        fundsEarned += grid.GetComponent<GridManager>().ChargePatients();

        //Game over if didn't meet quota
        if(quota <= fundsEarned)
        {
            quotaMet = true;
        }else
        {
            quotaMet = false;
        }
        startWeek.SetActive(quotaMet);
        endGame.SetActive(!quotaMet);

        //Updates money UI
        grid.GetComponent<GridManager>().carryOverMoney = fundsEarned;
        grid.GetComponent<GridManager>().UpdateUI(-1000);

        //Treats patients, disables grid
        patientManager.GetComponent<PatientManager>().TreatPatients();
        grid.SetActive(false);

        UpdateSummaryUI();
    }

    /// <summary>
    /// Goes to game over scene
    /// </summary>
    public void GameOver()
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

    private void UpdateSummaryUI()
    {
        weekNumberText.text = FormatString("Week ", currentWeek.ToString(), ".");
        quotaResultText.text = quotaMet ? "✓ Quota Achieved." : "× Quota Failed.";
        treatmentsScheduledText.text = FormatString("✓ ", patientsScheduled.ToString(), " Treatment(s) Scheduled.");
        underObservationText.text = FormatString("✓ ", patientsLocked.ToString(), " Patient(s) Under Observation.");
        dischargedText.text = FormatString("✓ ", patientsDischarged.ToString(), " Patient(s) Discharged.");

        string awaitingTreatmenSymbol = patientsWaiting == 0 ? "✓ " : "× ";
        string patientsLostSymbol = patientsLost == 0 ? "✓ " : "× ";
        awaitingTreatmentText.text = FormatString(awaitingTreatmenSymbol, patientsWaiting.ToString(), "  Patient(s) Awaiting Treatment.");
        patientsLostText.text = FormatString(patientsLostSymbol, patientsLost.ToString(), " Patient(s) Lost.");

        nextQuotaText.text = FormatString("Next Quota: ", (quota + (quota / 10)).ToString(), "");

        // Custom Adminstrator Message cause I wanna:
        string customAdminstratorMessage = "";
        if(!quotaMet) // Player Fails
        {
            customAdminstratorMessage = "For failure to meet quota, you are hereby terminated.\n" + "Please clear out your desk.\n";
        }else if(patientsLost > 0)
        {
            customAdminstratorMessage = "Please keep patient casualties at a minimum.\n";
        }else{
            customAdminstratorMessage = "Keep up the good work, Doctor.\n";
        }
        customAdminstratorMessage += "Received from Hospital Administrator.";
        adminstratorMessageText.text = customAdminstratorMessage;
    }

    private void NewWeekData()
    {
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
        quotaMet = false;

        // Reset Data
        fundsEarned = 0;
        patientsScheduled = 0;
        patientsLocked = 0;
        patientsDischarged = 0;
        patientsWaiting = 0;
        patientsLost = 0;
    }

    private string FormatString(string prefix, string value, string suffix)
    {
        return prefix + value + suffix;
    }
}
