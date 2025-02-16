using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.SceneManagement;

public class CVSReader : MonoBehaviour
{
    public TextAsset textAssetData;
    string filename = "";
    [System.Serializable]
    public class Patient
    {
        public string name;
        public string sex;
        public string age;
        public string familialStatus;
        public string condition;
        public string status;

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
        filename = Application.dataPath + "/PaitentResults.csv";
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.loadedSceneCount == 2)
        {
            WriteCSV();
        }
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

    public void WriteCSV()
    {
        if (thePatientList.patient.Length > 0)
        {
            TextWriter tw = new StreamWriter(filename, false);
            tw.WriteLine("Name, Sex, Age, Family Satus, Condition, Results");
            tw.Close();

            tw = new StreamWriter(filename, true);
            for (int index = 0; index < thePatientList.patient.Length; index++)
            {
                if (thePatientList.patient[index].status == "dead")
                {
                    tw.WriteLine(thePatientList.patient[index].name + "," + thePatientList.patient[index].sex + "," +
                        thePatientList.patient[index].age + "," + thePatientList.patient[index].familialStatus + "," +
                        thePatientList.patient[index].condition + "," + thePatientList.patient[index].status);
                }
            }
            tw.Close();
        }
    }
}