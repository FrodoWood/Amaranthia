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
    public AudioClip gemPickup;
    [Range(0,1f)]
    public float gemPickupVolume;

    private void Start()
    {

    }
    public void OnCollect(LevelsManager levelsManager)
    {
        levelsManager.AddExp(value);

        // Audio
        GameObject audio = new GameObject("GemPickUpSound");
        AudioSource source = audio.AddComponent<AudioSource>();
        source.clip = gemPickup;
        source.spatialBlend = 0f;
        source.playOnAwake = false;
        source.volume = gemPickupVolume;
        source.Play();
        Destroy(audio,gemPickup.length);

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
