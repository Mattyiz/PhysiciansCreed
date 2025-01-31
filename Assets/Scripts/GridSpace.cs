using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpace : MonoBehaviour
{
    public GameObject heldPatient;
    [SerializeField] private PatientManager pManager;

    // Start is called before the first frame update
    void Start()
    {
        if (pManager == null)
        {
            pManager = GameObject.Find("PatientManager").GetComponent<PatientManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        if(pManager.clickedPatient != null)
        {
            heldPatient = pManager.clickedPatient;
            pManager.clickedPatient = null;
            heldPatient.GetComponent<Patient>().clicked = false;
            heldPatient.GetComponent<BoxCollider2D>().enabled = true;

            heldPatient.transform.position = this.transform.position;
        }
    }
}
