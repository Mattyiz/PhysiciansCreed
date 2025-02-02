using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patient : MonoBehaviour
{

    [SerializeField] private PatientManager manager;
    public int sizeX;
    public int sizeY;
    public bool clicked;
    public List<GridSpace> holder;

    // Start is called before the first frame update
    void Start()
    {

        holder = new List<GridSpace>();

        clicked = false;

        if(manager == null)
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

    void OnMouseDown()
    {
        if(manager.clickedPatient != null)
        {
            return;
        }

        manager.clickedPatient = this.gameObject;
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        clicked = true;

        while(holder.Count > 0)
        {
            holder[0].GetComponent<CircleCollider2D>().enabled = true;
            holder[0].heldPatient = null;
            holder.RemoveAt(0);
        }
    }
}
