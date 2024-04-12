using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour, ICollectable
{
    [SerializeField] private float value;
    [SerializeField] private float rotationSpeed;
    private float t=0;
    [SerializeField] private float verticalMotionSpeed = 1;
    [SerializeField] private float verticalMotionOffset = 2;
    public void OnCollect(LevelsManager levelsManager)
    {
        levelsManager.AddExp(value);
        Destroy(gameObject);
    }

    private void Update()
    {
        transform.localEulerAngles += new Vector3(0, rotationSpeed * Time.deltaTime, 0);

        t += verticalMotionSpeed * Time.deltaTime;
        float yOffset = Mathf.Sin(t) * verticalMotionOffset;
        transform.position += new Vector3(0, yOffset, 0);
    }
}