using System.Collections;
using UnityEngine;
using TMPro;

public class LevelUpPopup : MonoBehaviour
{
    public TextMeshProUGUI levelUpText;
    public ParticleSystem levelUpParticles;

    private void Start()
    {
        // Dölj popup från början
        gameObject.SetActive(false);
        if (levelUpParticles != null)
        {
            levelUpParticles.gameObject.SetActive(false);
        }
    }

    public void ShowLevelUpPopup(int newLevel)
    {
        Debug.Log("Trying to show LevelUpPopup for level: " + newLevel);

        // Uppdatera texten i popupen
        levelUpText.text = "You have reached level " + newLevel + "!";

        // Visa popup
        gameObject.SetActive(true); // Aktivera objektet innan du startar koroutinen
        StartCoroutine(ShowAndHide());

        if (levelUpParticles != null)
        {
            levelUpParticles.gameObject.SetActive(true);
            levelUpParticles.Play(); // Starta partikeleffekten
        }
    }

    IEnumerator ShowAndHide()
    {
        // Visa popup
        Debug.Log("Showing LevelUpPopup");

        // Vänta i 2 sekunder
        yield return new WaitForSeconds(3f);

        // Dölj popup
        Debug.Log("Hiding LevelUpPopup");
        gameObject.SetActive(false); // Inaktivera objektet efter att korutinen har avslutats

        if (levelUpParticles != null)
        {
            levelUpParticles.Stop();
            levelUpParticles.gameObject.SetActive(false);
        }
    }
}
