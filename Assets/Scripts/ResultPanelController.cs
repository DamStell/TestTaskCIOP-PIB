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
        results += "Czas dzia³ania: " + Mathf.FloorToInt(PlayerController.instance.totalTime / 60) + " minut " + Mathf.FloorToInt(PlayerController.instance.totalTime % 60) + " sekund\n";
        results += "Poprawnie zneutralizowane drzwi: " + PlayerController.instance.doorsNeutralized + "\n";
        results += "Poprawnie zneutralizowane pomieszczenia: " + PlayerController.instance.roomsNeutralized + "\n";
        results += "Pope³nione b³êdy:\n";
        results += "Brak wykonania pomiaru: " + PlayerController.instance.errorsNoMeasurement + "\n";
        results += "Za du¿e ustawienie pierœcienia podczas neutralizacji drzwi: " + PlayerController.instance.errorsHighRingSetting + "\n";
        results += "Za ma³e ustawienie pierœcienia podczas neutralizacji drzwi: " + PlayerController.instance.errorsLowRingSetting + "\n";
        results += "Z³e ustawienie pierœcienia podczas strzelania w pomieszczeniu za drzwiami: " + PlayerController.instance.errorsWrongRingSettingRoom + "\n";
        results += "Pominiête œciany w pomieszczeniu za drzwiami: " + PlayerController.instance.errorsSkippedWalls + "\n";
        results += "Liczba œcian w pomieszczeniu za drzwiami trafionych wiêcej ni¿ raz: " + PlayerController.instance.errorsMultipleHits;

        resultsText.text = results;
        resultPanel.SetActive(true); 
    }
}
