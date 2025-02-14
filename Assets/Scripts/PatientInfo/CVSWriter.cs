using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;

public class CVSWriter : MonoBehaviour
{
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
    public PatientList myPatientList = new PatientList();

    // Start is called before the first frame update
    void Start()
    {
        filename = Application.dataPath + "/paitentResults.csv";
    }

    // Update is called once per frame
    void Update()
    {
        /*if (SceneManager.loadedSceneCount == 1)
        {
            WriteCSV();
        }*/
        if (Input.GetKeyUp(KeyCode.Space))
        {
            WriteCSV();
        }
    }

    public void WriteCSV()
    {
        if (myPatientList.patient.Length > 0)
        {
            TextWriter tw = new StreamWriter(filename, false);
            tw.WriteLine("Name, Sex, Age, Family Satus, Condition, Results");
            tw.Close();

            tw = new StreamWriter(filename, true);
            for (int index = 0; index < myPatientList.patient.Length; index++)
            {
                if (myPatientList.patient[index].status == "dead")
                {
                    tw.WriteLine(myPatientList.patient[index].name + "," + myPatientList.patient[index].sex + "," +
                        myPatientList.patient[index].age + "," + myPatientList.patient[index].familialStatus + "," +
                        myPatientList.patient[index].condition + "," + myPatientList.patient[index].status);
                }
            }
            tw.Close();
        }
    }
}
