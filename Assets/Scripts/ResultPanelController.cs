using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class ResultPanelController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI resultsText;
    [SerializeField] GameObject resultPanel;

    public void DisplayResults()
    {
        string results = "Wyniki gry:\n";
        results += "Czas dzia�ania: " + Mathf.FloorToInt(PlayerController.instance.totalTime / 60) + " minut " + Mathf.FloorToInt(PlayerController.instance.totalTime % 60) + " sekund\n";
        results += "Poprawnie zneutralizowane drzwi: " + PlayerController.instance.doorsNeutralized + "\n";
        results += "Poprawnie zneutralizowane pomieszczenia: " + PlayerController.instance.roomsNeutralized + "\n";
        results += "Pope�nione b��dy:\n";
        results += "Brak wykonania pomiaru: " + PlayerController.instance.errorsNoMeasurement + "\n";
        results += "Za du�e ustawienie pier�cienia podczas neutralizacji drzwi: " + PlayerController.instance.errorsHighRingSetting + "\n";
        results += "Za ma�e ustawienie pier�cienia podczas neutralizacji drzwi: " + PlayerController.instance.errorsLowRingSetting + "\n";
        results += "Z�e ustawienie pier�cienia podczas strzelania w pomieszczeniu za drzwiami: " + PlayerController.instance.errorsWrongRingSettingRoom + "\n";
        results += "Pomini�te �ciany w pomieszczeniu za drzwiami: " + PlayerController.instance.errorsSkippedWalls + "\n";
        results += "Liczba �cian w pomieszczeniu za drzwiami trafionych wi�cej ni� raz: " + PlayerController.instance.errorsMultipleHits;

        resultsText.text = results;
        resultPanel.SetActive(true); 
    }
}
