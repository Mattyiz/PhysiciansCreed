using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class CVSReader : MonoBehaviour
{
    public TextAsset textAssetData;

    [System.Serializable]
    public class Patient
    {
        public string name;
        public string sex;
        public string age;
        public string familialStatus;
        public string condition;

    }
    [System.Serializable]
    public class PatientList
    {
        public Patient[] patient;
    }

    public PatientList thePatientList = new PatientList();

    // Start is called before the first frame update
    void Start()
    {
        ReadCSV();
    }

    void ReadCSV()
    {
        string[] data = textAssetData.text.Split(new string[] { ",", "\n" }, StringSplitOptions.None);
        int tabelSize = data.Length / 5 - 1;
        thePatientList.patient = new Patient[tabelSize];

        for (int index  = 0; index < tabelSize; index++)
        {
            thePatientList.patient[index] = new Patient();
            thePatientList.patient[index].name = data[5 * (index + 1)];
            thePatientList.patient[index].sex = data[5 * (index + 1) + 1];
            thePatientList.patient[index].age = data[5 * (index + 1) + 2];
            thePatientList.patient[index].familialStatus = data[5 * (index + 1) + 3];
            thePatientList.patient[index].condition = data[5 * (index + 1) + 4];
        }
    }
}