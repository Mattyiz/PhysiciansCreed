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
    public TextMeshProUGUI fundsText;

    private void Start() {
        gameObject.SetActive(false);
    }

    public void DisplayPatientInfo(Patient patient) 
    {
        // Display or hide each field based on whether the data is available
        SetTextIfValid(nameText, patient.fullName);
        SetTextIfValid(ageText, patient.age.ToString());
        SetTextIfValid(genderText, patient.gender);
        SetTextIfValid(familyStatusText, patient.familyStatus);
        SetTextIfValid(conditionText, FormatString("Condition: ", patient.condition, ""));
        SetTextIfValid(survivalPercentText, FormatString("Survival Chance: ", patient.survivalPercent.ToString(), "%"));
        SetTextIfValid(fundsText, FormatString("Treatment Funds: $", patient.funds.ToString(), ""));

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
