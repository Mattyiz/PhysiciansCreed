using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Patient : MonoBehaviour
{

    [SerializeField] private PatientManager manager;

    [Header("Grid Stuff")]
    public int sizeX;
    public int sizeY;
    public List<Vector2> nullSquares; //nullSquares are the spots of the rectangle that aren't in the shape (Think of the tetris shapes)
    public bool clicked;
    public List<GridSpace> holder;
    public bool scheduled;

    [Header("Patient Records")]
    public string fullName;
    public int age;
    public string gender;
    public string familyStatus;
    public string condition;
    public int survivalPercent;
    public int funds;


    // Start is called before the first frame update
    void Start()
    {

        holder = new List<GridSpace>();

        clicked = false;
        scheduled = false;

        if (manager == null)
        {
            manager = GameObject.Find("PatientManager").GetComponent<PatientManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if(clicked)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            mousePos.z = 0;
            mousePos.y = mousePos.y - ((sizeY-1));
            mousePos.x = mousePos.x - ((sizeX-1));
            transform.position = mousePos;
        }

    }

    public void Clicked(PointerEventData eventData)
    {
        //Return if already holding a patient
        if (manager.clickedPatient != null)
        {
            return;
        }

        //Registers this as being held and clicked, and disables the collider
        manager.RegisterPatient(this);
        ToggleHitboxes(false);
        clicked = true;

        //If the patient is scheduled, remove them from the schedule
        if(scheduled)
        {
            scheduled = false;
            ClearHolder();
        }
        
    }

    /// <summary>
    /// Goes through all grid spaces holding this and removes this from them
    /// </summary>
    public void ClearHolder()
    {
        //Updates the UI
        holder[0].gridManager.UpdateUI(-1 * funds);

        //Goes through each grid space holding this and removes this from it
        while (holder.Count > 0)
        {
            holder[0].gameObject.GetComponent<CircleCollider2D>().enabled = true;
            holder[0].heldPatient = null;
            holder.RemoveAt(0);
        }
    }

    public void ToggleHitboxes(bool able)
    {
        foreach (Transform child in transform)
        {
            if(child.gameObject.GetComponent<BoxCollider2D>() != null)
            {
                child.gameObject.GetComponent<BoxCollider2D>().enabled = able;
            }
        }
    }

    public void Rotate()
    {

        for(int i = 0; i < nullSquares.Count; i++)
        {

            {
                Vector2 newNull = new Vector2(nullSquares[i].y, sizeX - nullSquares[i].x - 1);
                nullSquares[i] = newNull;
            }
        }


        int temp = sizeX;
        sizeX = sizeY;
        sizeY = temp;
        transform.Rotate(0, 0, -90);
    }

}
