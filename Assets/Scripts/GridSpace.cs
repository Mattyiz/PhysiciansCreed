using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridSpace : MonoBehaviour, IPointerClickHandler
{
    public GameObject heldPatient;
    [SerializeField] private PatientManager pManager;
    public GameGrid gridManager;
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
        if (pManager.clickedPatient != null && heldPatient == null)
        {
            Patient patient = pManager.clickedPatient.GetComponent<Patient>();
            if (!CheckPatientSize(patient))
            {
                return;
            }

            for (int i = 0; i < patient.sizeY; i++)
            {
                for (int j = 0; j < patient.sizeX; j++)
                {

                    gridManager.grid[y - i, x - j].GetComponent<GridSpace>().heldPatient = pManager.clickedPatient;
                    patient.holder.Add(gridManager.grid[y - i, x - j].GetComponent<GridSpace>());
                    gridManager.grid[y - i, x - j].GetComponent<CircleCollider2D>().enabled = false;
                }
            }

            pManager.clickedPatient = null;
            patient.clicked = false;
            patient.scheduled = true;

            Vector3 newPosition = new Vector3(0, 0, 0);
            heldPatient.GetComponent<BoxCollider2D>().enabled = true;
            newPosition.x = (this.transform.position.x + gridManager.grid[y - (patient.sizeY - 1), x - (patient.sizeX - 1)].transform.position.x) / 2;
            newPosition.y = (this.transform.position.y + gridManager.grid[y - (patient.sizeY - 1), x - (patient.sizeX - 1)].transform.position.y) / 2;
            heldPatient.transform.position = newPosition;

            gridManager.UpdateUI(patient.money);
        }
    }

    private bool CheckPatientSize(Patient patient)
    {
        if(patient.sizeX == 1 && patient.sizeY == 1)
        {
            return true;
        }

        if(y - (patient.sizeY-1) < 0)
        {
            return false;
        }

        if (x - (patient.sizeX - 1) < 0)
        {
            return false;
        }

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
