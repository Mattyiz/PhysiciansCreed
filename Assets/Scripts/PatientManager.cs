using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> patientPrefabs;
    [SerializeField] private List<GameObject> patients;
    public GameObject clickedPatient;

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
        for(int i = 0; i < patientPrefabs.Count; i++)
        {
            //WARNING EEPY CODE AHEAD
            switch (i)
            {
                case 0:
                    patients.Add(Instantiate(patientPrefabs[0], new Vector3(-7.5f, 3, 0), transform.rotation));
                    patients.Add(Instantiate(patientPrefabs[0], new Vector3(-5.5f, 3, 0), transform.rotation));
                    break;
                case 1:
                    patients.Add(Instantiate(patientPrefabs[2], new Vector3(-6.5f, 1.5f, 0), transform.rotation));
                    break;
                case 2:
                    patients.Add(Instantiate(patientPrefabs[4], new Vector3(-6.5f, -2, 0), transform.rotation));
                    break;
                case 3:
                    patients.Add(Instantiate(patientPrefabs[1], new Vector3(6, 2, 0), transform.rotation));
                    patients.Add(Instantiate(patientPrefabs[0], new Vector3(7.5f, 2, 0), transform.rotation));
                    break;
                case 4:
                    patients.Add(Instantiate(patientPrefabs[3], new Vector3(6.5f, -2, 0), transform.rotation));
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
            if(patients[i].GetComponent<Patient>().scheduled)
            {
                Destroy(patients[i]);
                patients.RemoveAt(i);
                i--;

            }
        }
    }


}
