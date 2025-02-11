using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WaitingRoom : MonoBehaviour
{

    [SerializeField] private PatientManager pManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //If manager has a clicked patient and this is not holding a patient
        if (pManager.clickedPatient != null)
        {
            Patient patient = pManager.clickedPatient.GetComponent<Patient>();
            
            //Unclicks the patient and schedules them
            pManager.clickedPatient = null;
            pManager.ChangeCursor(true);
            patient.clicked = false;
        }
    }
}
