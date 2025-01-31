using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patient : MonoBehaviour
{

    [SerializeField] private PatientManager manager;
    public int sizeX;
    public int sizeY;
    public bool clicked;

    // Start is called before the first frame update
    void Start()
    {

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
            transform.position = mousePos;
        }

        
    }

    void OnMouseDown()
    {
        manager.clickedPatient = this.gameObject;
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        clicked = true;
    }
}
