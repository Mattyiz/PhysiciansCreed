using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientManager : MonoBehaviour
{
    public GameObject patientHolder; // Holds Patients so they can be hidden in certain cases
    public PatientInfo patientInfo;
    public GridManager gManager;
    private bool first = true;

    [SerializeField] private List<GameObject> patientPrefabs;
    [SerializeField] public List<GameObject> patients;
    public GameObject clickedPatient;

    [SerializeField] private Texture2D open;
    [SerializeField] private Texture2D close;
    [SerializeField] private Texture2D bloodyOpen;
    [SerializeField] private Texture2D bloodyClose;
    [SerializeField] private GameObject WaitingRoom;

    public CVSReader cvsReader;



    // Start is called before the first frame update
    void Start()
    {
        clickedPatient = null;

        foreach(Transform child in transform)
        {
            patients.Add(child.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Spawns the patients
    /// </summary>
    public void SpawnPatients()
    {
        int numberOfPatients;

        //inital spawn
        if (first)
        {
            numberOfPatients = Random.Range(6, 10);
            first = false;
        }
        //otherwise
        else
        {
            //randomly determine the number of patients to spawn (based off available grid spaces)
            int availableSpaces = gManager.CountAvailableGridSpaces();

            //atleast 4 patients spawn
            numberOfPatients = Mathf.Clamp(Random.Range(4, 10), 0, availableSpaces / 2);
        }

        for (int i = 0; i < numberOfPatients; i++)
        {
            //randomly select a patient prefab
            int randomPrefabIndex = Random.Range(0, patientPrefabs.Count);
            GameObject randomPatientPrefab = GetWeightedRandomPrefab(randomPrefabIndex);

            //tandomly assign a position within a defined range
            Vector3 randomPosition = new Vector3(Random.Range(2f, 8f), Random.Range(-2f, 3f), 0);

            //instantiate the patient
            GameObject newPatient = Instantiate(randomPatientPrefab, randomPosition, transform.rotation);
            newPatient.transform.SetParent(patientHolder.transform);
            patients.Add(newPatient);

            AssignRandomPatientData(newPatient);
        }
    }

    private GameObject GetWeightedRandomPrefab(int randomI)
    {
        //assign weights to each prefab
        List<float> weights = new List<float>();
        for (int i = 0; i < patientPrefabs.Count; i++)
        {
            //give the last prefab a lower weight 1 : 5
            weights.Add(i == patientPrefabs.Count - 1 ? 1f : 5f);
        }

        //calculate the total weight
        float totalWeight = 0;
        foreach (float weight in weights)
        {
            totalWeight += weight;
        }

        //generate a random number between 0 and the total weight
        float randomValue = Random.Range(0, totalWeight);

        //determine which prefab to select based on the random value
        for (int i = 0; i < weights.Count; i++)
        {
            if (randomValue < weights[i])
            {
                return patientPrefabs[i];
            }
            randomValue -= weights[i];
        }

        //return random if something goes amiss
        return patientPrefabs[randomI];
    }

     private void AssignRandomPatientData(GameObject patient)
    {
        // Get the PatientData component from the patient prefab
        Patient patientData = patient.GetComponent<Patient>();

        if (patientData != null && cvsReader != null && cvsReader.thePatientList.patient.Length > 0)
        {
            // Randomly select a patient from the CVSReader's patient list
            int randomIndex = Random.Range(0, cvsReader.thePatientList.patient.Length);
            CVSReader.Patient randomPatient = cvsReader.thePatientList.patient[randomIndex];

            // Assign the data to the patient
            patientData.SetData(
                randomPatient.name,
                randomPatient.sex,
                int.Parse(randomPatient.age),
                randomPatient.familialStatus,
                randomPatient.condition
            );
        }
        else
        {
            Debug.LogWarning("PatientData component or CVSReader data is missing!");
        }
    }

    /// <summary>
    /// Goes through the patients, if they're scheduled, destroy them
    /// </summary>
    public void TreatPatients()
    {
        for(int i = 0; i < patients.Count; i++)
        {
            Patient currentPatient = patients[i].GetComponent<Patient>();

            if(currentPatient.scheduled && currentPatient.patientData.treatmentLength <= 1 && !currentPatient.locked)
            {
                Destroy(patients[i]);
                patients.RemoveAt(i);
                i--;

            }
            else if (!currentPatient.scheduled)
            {
                float survivalChance = currentPatient.patientData.survivalPercent;
                float survivalThreshold = Random.Range(0f, 100f);

                if (survivalThreshold > survivalChance)
                {
                    //patient dies
                    DayManager.Instance.patientsLost++;
                    DayManager.Instance.allLostPatients.Add(currentPatient.patientData);
                    Destroy(patients[i]);
                    patients.RemoveAt(i);
                    i--;
                }
                else
                {
                    //patient survives but survivalPercent decreases
                    currentPatient.patientData.survivalPercent = (int)(currentPatient.patientData.survivalPercent * 0.9f); //decrease survival rate
                    if (currentPatient.patientData.survivalPercent < 10)
                        currentPatient.patientData.survivalPercent = 10; //Prevent it from reaching 0%
                }
            }
            else if(currentPatient.scheduled && currentPatient.locked)
            {
                currentPatient.treatedThisWeek = false;
            }
        }

        ChangeCursor(true);


    }

    void OnRotate()
    {
        if(clickedPatient == null)
        {
            return;
        }

        //Debug.Log(clickedPatient);
        clickedPatient.GetComponent<Patient>().Rotate();
    }

    public void ChangeCursor(bool isOpen)
    {

        if (isOpen && DayManager.Instance.allLostPatients.Count <= 0) // Non Bloody Open Cursor
        {
            Cursor.SetCursor(open, new Vector2(0, 0), CursorMode.Auto);
            WaitingRoom.GetComponent<BoxCollider2D>().enabled = false;
            return;
        }else if(!isOpen && DayManager.Instance.allLostPatients.Count <= 0) // Non Bloody Closed Cursor
        {
            Cursor.SetCursor(close, new Vector2(0, 0), CursorMode.Auto);
            WaitingRoom.GetComponent<BoxCollider2D>().enabled = true;
        }else if(isOpen && DayManager.Instance.allLostPatients.Count > 0) // Bloody Open Cursor
        {
            Cursor.SetCursor(bloodyOpen, new Vector2(0, 0), CursorMode.Auto);
            WaitingRoom.GetComponent<BoxCollider2D>().enabled = false;
        }else // Non Bloody Closed
        {
            Cursor.SetCursor(bloodyClose, new Vector2(0, 0), CursorMode.Auto);
            WaitingRoom.GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    public void RegisterPatient(Patient patient)
    {
        clickedPatient = patient.gameObject;
        ChangeCursor(false);
        patientInfo.DisplayPatientInfo(patient);
    }

}
