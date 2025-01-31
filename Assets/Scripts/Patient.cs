using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patient : MonoBehaviour
{

    [SerializeField] private PatientManager manager;

    // Start is called before the first frame update
    void Start()
    {
        if(manager == null)
        {
            manager = GameObject.Find("PatientManager").GetComponent<PatientManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        manager.clickedPatient = this.gameObject;
    }
}
