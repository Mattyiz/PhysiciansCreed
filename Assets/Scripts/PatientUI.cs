using UnityEngine;
using UnityEngine.UI;

public class PatientUIButton : MonoBehaviour
{
    private GameObject patientPrefab;
    private PatientManager manager;

    public void SetPatientPrefab(GameObject prefab, PatientManager patientManager)
    {
        patientPrefab = prefab;
    }

    public void OnClick()
    {
        // Spawn the real patient in the world space
        GameObject newPatient = Instantiate(patientPrefab, Vector3.zero, Quaternion.identity);
        manager.patients.Add(newPatient);

        // Remove this UI element from ScrollView
        Destroy(gameObject);
    }

    
}