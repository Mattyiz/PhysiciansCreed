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

        startButton.SetActive(false);
        endButton.SetActive(true);
    }

    public void EndDay()
    {

        currentMoney += grid.GetComponent<GameGrid>().ChargePatients();

        if(quota <= currentMoney)
        {
            currentMoney -= quota;
        }
        else
        {
            GameOver();
        }

        grid.GetComponent<GameGrid>().carryOverMoney = currentMoney;
        grid.GetComponent<GameGrid>().UpdateUI(-1000);

        patientManager.GetComponent<PatientManager>().TreatPatients();
        grid.SetActive(false);
        startButton.SetActive(true);
        endButton.SetActive(false);
    }

    private void GameOver()
    {
        SceneManager.LoadScene("Game Over");
    }
}
