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
        // Fyll dropdown-menyn med alternativ fr�n WinCriteria-enum
        dropdown.ClearOptions();
        dropdown.AddOptions(new List<string>(Enum.GetNames(typeof(WinCriteria))));
    }

    public void OnDropdownValueChanged(int index)
    {
        // F�nga upp det valda alternativet
        WinCriteria selectedCriteria = (WinCriteria)index;

        // Utf�r specifik logik baserat p� det valda alternativet
        switch (selectedCriteria)
        {
            case WinCriteria.Time:
                // Logik f�r att anv�nda tid som vinstkriterium
                break;
            case WinCriteria.Score:
                // Logik f�r att anv�nda po�ng som vinstkriterium
                break;
            case WinCriteria.BloopType:
                // Logik f�r att anv�nda blooptyp som vinstkriterium
                break;
            default:
                break;
        }
    }
}
