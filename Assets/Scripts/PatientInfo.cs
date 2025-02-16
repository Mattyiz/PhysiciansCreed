using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PatientInfo : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI ageText;
    public TextMeshProUGUI genderText;
    public TextMeshProUGUI familyStatusText;
    public TextMeshProUGUI conditionText;
    public TextMeshProUGUI survivalPercentText;
    public TextMeshProUGUI treatmentLengthText;
    public TextMeshProUGUI fundsText;
    [SerializeField] private bool lostPatient;

    private void Start() {
        if (!lostPatient) gameObject.SetActive(false);
    }

    public void DisplayPatientInfo(Patient patient) 
    {
        // Display or hide each field based on whether the data is available
        SetTextIfValid(nameText, patient.patientData.fullName);
        SetTextIfValid(ageText, patient.patientData.age.ToString());
        SetTextIfValid(genderText, patient.patientData.gender);
        SetTextIfValid(familyStatusText, patient.patientData.familyStatus);
        SetTextIfValid(conditionText, FormatString("Condition: ", patient.patientData.condition, ""));
        SetTextIfValid(survivalPercentText, FormatString("Survival Chance: ", patient.patientData.survivalPercent.ToString(), "%"));
        SetTextIfValid(treatmentLengthText, FormatString("", patient.patientData.treatmentLength.ToString(), " Week Treatment"));
        SetTextIfValid(fundsText, FormatString("Treatment Funds: $", patient.patientData.funds.ToString(), ""));

        gameObject.SetActive(true);
    }

    public void HidePatientInfo()
    {
        gameObject.SetActive(false);
    }

    private string FormatString(string prefix, string value, string suffix)
    {
        return prefix + value + suffix;
    }

    private void SetTextIfValid(TextMeshProUGUI textField, string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            textField.text = value;
            textField.gameObject.SetActive(true); // Show the text field
        }
        else
        {
            textField.gameObject.SetActive(false); // Hide the text field
        }
    }
}
