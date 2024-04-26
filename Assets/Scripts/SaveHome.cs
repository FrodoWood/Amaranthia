using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveHome : MonoBehaviour, IInteractable
{
    public Canvas saveGamePrompt;
    public GameObject saveHomeModel;
    public void OnInteract()
    {
        SaveLoadManager.instance.SaveGame();
    }

    private void Start()
    {
        saveHomeModel.transform.DOMove(transform.position + Vector3.up, 4f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        saveHomeModel.transform.DORotate(new Vector3(0, 360, 0), 5f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }

    private void OnTriggerEnter(Collider other)
    {
        saveGamePrompt.gameObject.SetActive(true);
    }
    private void OnTriggerExit(Collider other)
    {
        saveGamePrompt.gameObject.SetActive(false);
    }

}
