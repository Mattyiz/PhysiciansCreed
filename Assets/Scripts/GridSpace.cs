using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridSpace : MonoBehaviour, IPointerClickHandler
{
    public GameObject heldPatient;
    [SerializeField] private PatientManager pManager;
    public GridManager gridManager;
    public int x;
    public int y;

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

    public void OnPointerClick(PointerEventData eventData)
    {
        //If manager has a clicked patient and this is not holding a patient
        if (pManager.clickedPatient != null && heldPatient == null)
        {
            Patient patient = pManager.clickedPatient.GetComponent<Patient>();

            //Returns if the patient size isn't good
            if (!CheckPatientSize(patient))
            {
                return;
            }

            //Loops through patient size and adds it each grid space needed
            for (int i = 0; i < patient.sizeY; i++)
            {
                for (int j = 0; j < patient.sizeX; j++)
                {

                    gridManager.grid[y - i, x - j].GetComponent<GridSpace>().heldPatient = pManager.clickedPatient;
                    patient.holder.Add(gridManager.grid[y - i, x - j].GetComponent<GridSpace>());
                    gridManager.grid[y - i, x - j].GetComponent<CircleCollider2D>().enabled = false;
                }
            }

            //Unclicks the patient and schedules them
            pManager.clickedPatient = null;
            patient.clicked = false;
            patient.scheduled = true;

            //Places the patient where needed
            Vector3 newPosition = new Vector3(0, 0, 0);
            heldPatient.GetComponent<BoxCollider2D>().enabled = true;
            newPosition.x = (this.transform.position.x + gridManager.grid[y - (patient.sizeY - 1), x - (patient.sizeX - 1)].transform.position.x) / 2;
            newPosition.y = (this.transform.position.y + gridManager.grid[y - (patient.sizeY - 1), x - (patient.sizeX - 1)].transform.position.y) / 2;
            heldPatient.transform.position = newPosition;

            //Updates the money UI
            gridManager.UpdateUI(patient.money);
        }
    }

    /// <summary>
    /// Makes sure the patient size works
    /// </summary>
    /// <param name="patient">Patient to check</param>
    /// <returns>True if patient would fit in the grid</returns>
    private bool CheckPatientSize(Patient patient)
    {
        //True if it's 1x1
        if(patient.sizeX == 1 && patient.sizeY == 1)
        {
            return true;
        }

        //False if the patient would go off the grid
        if(y - (patient.sizeY-1) < 0)
        {
            return false;
        }
        if (x - (patient.sizeX - 1) < 0)
        {
            return false;
        }

        //Checks each grid space needed to see if they're empty. If any aren't empty returns false
        for (int i = 0; i < patient.sizeY; i++)
        {
            for (int j = 0; j < patient.sizeX; j++)
            {
                if (gridManager.grid[y-i, x - j].GetComponent<GridSpace>().heldPatient != null)
                {
                    return false;
                }
            }
        }

        return true;
    }
}
