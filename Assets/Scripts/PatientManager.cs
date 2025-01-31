using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> patients;
    public GameObject clickedPatient;

    // Start is called before the first frame update
    void Start()
    {
        clickedPatient = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
