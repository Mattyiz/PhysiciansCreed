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
        for (int i = 0; i < patientPrefabs.Count; i++)
        {
            GameObject newPatient;

            switch (i)
            {
                case 0:
                    newPatient = Instantiate(patientPrefabs[0], new Vector3(2.5f, 2.7f, 0), transform.rotation);
                    newPatient.transform.SetParent(patientHolder.transform);
                    patients.Add(newPatient);

                    newPatient = Instantiate(patientPrefabs[0], new Vector3(4.2f, 2.7f, 0), transform.rotation);
                    newPatient.transform.SetParent(patientHolder.transform);
                    patients.Add(newPatient);
                    break;

                case 1:
                    newPatient = Instantiate(patientPrefabs[2], new Vector3(6.7f, -1.5f, 0), transform.rotation);
                    newPatient.transform.SetParent(patientHolder.transform);
                    patients.Add(newPatient);
                    break;

                case 2:
                    newPatient = Instantiate(patientPrefabs[3], new Vector3(3.3f, -1.7f, 0), transform.rotation);
                    newPatient.transform.SetParent(patientHolder.transform);
                    patients.Add(newPatient);
                    break;

                case 3:
                    newPatient = Instantiate(patientPrefabs[1], new Vector3(7.4f, 2.7f, 0), transform.rotation);
                    newPatient.transform.SetParent(patientHolder.transform);
                    patients.Add(newPatient);

                    newPatient = Instantiate(patientPrefabs[1], new Vector3(5.8f, 2.7f, 0), transform.rotation);
                    newPatient.transform.SetParent(patientHolder.transform);
                    patients.Add(newPatient);
                    break;
            }
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
