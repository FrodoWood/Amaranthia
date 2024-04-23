using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveHome : MonoBehaviour, IInteractable
{
    public void OnInteract()
    {
        SaveLoadManager.instance.SaveGame();
    }
}
