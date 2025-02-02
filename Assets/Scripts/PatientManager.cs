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

    public void SpawnPatients()
    {
        for(int i = 0; i < patientPrefabs.Count; i++)
        {
            //WARNING EEPY CODE AHEAD
            if(i < 3)
            {
                patients.Add(Instantiate(patientPrefabs[i], new Vector3(-6, (i-1) * 2.5f, 0), transform.rotation));
            }
            else
            {
                if(i%2 == 0)
                {
                    patients.Add(Instantiate(patientPrefabs[i], new Vector3(6, 2.5f, 0), transform.rotation));

                }
                else
                {
                    patients.Add(Instantiate(patientPrefabs[i], new Vector3(6, -2.5f, 0), transform.rotation));

                }
            }
        }
    }

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
