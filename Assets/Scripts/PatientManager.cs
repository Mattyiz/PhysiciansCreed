using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientManager : MonoBehaviour
{

    public int deadPatients;
    public int savedPatients;

    [SerializeField] private List<GameObject> patientPrefabs;
    [SerializeField] public List<GameObject> patients;
    public GameObject clickedPatient;

    [SerializeField] private Texture2D open;
    [SerializeField] private Texture2D close;
    [SerializeField] private GameObject WaitingRoom;

    // Start is called before the first frame update
    void Start()
    {
        deadPatients = 0;
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
                    patients.Add(Instantiate(patientPrefabs[0], new Vector3(2.5f, 2.7f, 0), transform.rotation));
                    patients.Add(Instantiate(patientPrefabs[0], new Vector3(4.2f, 2.7f, 0), transform.rotation));
                    break;
                case 1:
                    patients.Add(Instantiate(patientPrefabs[2], new Vector3(6.7f, -1.5f, 0), transform.rotation));
                    break;
                case 2:
                    patients.Add(Instantiate(patientPrefabs[3], new Vector3(3.3f, -1.7f, 0), transform.rotation));
                    break;
                case 3:
                    patients.Add(Instantiate(patientPrefabs[1], new Vector3(7.4f, 2.7f, 0), transform.rotation));
                    patients.Add(Instantiate(patientPrefabs[1], new Vector3(5.8f, 2.7f, 0), transform.rotation));
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
                savedPatients++;
                Destroy(patients[i]);
                patients.RemoveAt(i);
                i--;

            }
            else
            {
                deadPatients++;
                Destroy(patients[i]);
                patients.RemoveAt(i);
                i--;
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

}
