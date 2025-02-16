using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PatientData
{
    public string fullName;
    public int age;
    public string gender;
    public string familyStatus;
    public string condition;
    public int survivalPercent;
    public int treatmentLength;
    public int funds;

    // Constructor to initialize patient data
    public PatientData(string name, int age, string gender, string familyStatus, string condition, int survivalPercent, int treatmentLength, int funds)
    {
        this.fullName = name;
        this.age = age;
        this.gender = gender;
        this.familyStatus = familyStatus;
        this.condition = condition;
        this.survivalPercent = survivalPercent;
        this.treatmentLength = treatmentLength;
        this.funds = funds;
    }
}