using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using static LevelDataSO;

public class WinCriteriaDropdown : MonoBehaviour
{
    public Dropdown dropdown;

    void Start()
    {
        // Fyll dropdown-menyn med alternativ från WinCriteria-enum
        dropdown.ClearOptions();
        dropdown.AddOptions(new List<string>(Enum.GetNames(typeof(WinCriteria))));
    }

    public void OnDropdownValueChanged(int index)
    {
        // Fånga upp det valda alternativet
        WinCriteria selectedCriteria = (WinCriteria)index;

        // Utför specifik logik baserat på det valda alternativet
        switch (selectedCriteria)
        {
            case WinCriteria.Time:
                // Logik för att använda tid som vinstkriterium
                break;
            case WinCriteria.Score:
                // Logik för att använda poäng som vinstkriterium
                break;
            case WinCriteria.BloopType:
                // Logik för att använda blooptyp som vinstkriterium
                break;
            default:
                break;
        }
    }
}
