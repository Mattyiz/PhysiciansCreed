using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientManager : MonoBehaviour
{
    public GameObject patientHolder; // Holds Patients so they can be hidden in certain cases
    public PatientInfo patientInfo;

    [SerializeField] private List<GameObject> patientPrefabs;
    [SerializeField] public List<GameObject> patients;
    public GameObject clickedPatient;

    [SerializeField] private Texture2D open;
    [SerializeField] private Texture2D close;
    [SerializeField] private GameObject WaitingRoom;
    


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
        //randomly determine the number of patients to spawn (between 6 and 9)
        int numberOfPatients = Random.Range(6, 10);

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

            }else if(!currentPatient.scheduled)
            {

                // TODO: Make it so patients can survive or die depending on their survival percentage
                DayManager.Instance.patientsLost++;
                DayManager.Instance.allLostPatients.Add(currentPatient.patientData);
                Destroy(patients[i]);
                patients.RemoveAt(i);
                i--;
            }else if(currentPatient.scheduled && currentPatient.locked)
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

        if (isOpen)
        {
            Cursor.SetCursor(open, new Vector2(0, 0), CursorMode.Auto);
            WaitingRoom.GetComponent<BoxCollider2D>().enabled = false;
            return;
        }

        Cursor.SetCursor(close, new Vector2(0, 0), CursorMode.Auto);
        WaitingRoom.GetComponent<BoxCollider2D>().enabled = true;
    }

    public void RegisterPatient(Patient patient)
    {
        clickedPatient = patient.gameObject;
        ChangeCursor(false);
        patientInfo.DisplayPatientInfo(patient);
    }

}
