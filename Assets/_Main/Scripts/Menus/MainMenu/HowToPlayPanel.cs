using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlayPanel : MonoBehaviour
{
    public GameObject[] manualPages;
    int currentManualPageIndex = 0;

    public void NextManualPage()
    {
        manualPages[currentManualPageIndex].SetActive(false);
        currentManualPageIndex = (currentManualPageIndex + 1) % manualPages.Length;
        manualPages[currentManualPageIndex].SetActive(true);
    }

    public void PreviousManualPage()
    {
        manualPages[currentManualPageIndex].SetActive(false);
        currentManualPageIndex--;
        if (currentManualPageIndex < 0)
        {
            currentManualPageIndex += manualPages.Length;
        }
        manualPages[currentManualPageIndex].SetActive(true);

    }
}
