using DG.Tweening;
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
    public bool moveTowardsTarget = false;
    private Transform player;
    public float gemPickupSpeed = 40;
    

    private void Start()
    {

    }
    public void OnCollect(LevelsManager levelsManager)
    {
        levelsManager.AddExp(value);
        GetComponent<Collider>().enabled = false;
        player = levelsManager.transform;
        // Animation
        Vector3 dirPlayerToGem = Vector3.Scale((transform.position - player.position), new Vector3(1,0,1)).normalized;
        Tween moveAway = transform.DOMove(transform.position + dirPlayerToGem * 8 + new Vector3(0,12,0), 0.4f).SetEase(Ease.OutSine).OnComplete(delegate() {
                moveTowardsTarget = true;
            });
        moveAway.Play();
        
        

        // Audio
        GameObject audio = new GameObject("GemPickUpSound");
        AudioSource source = audio.AddComponent<AudioSource>();
        source.clip = gemPickup;
        source.spatialBlend = 0f;
        source.playOnAwake = false;
        source.volume = gemPickupVolume;
        source.Play();
        Destroy(audio,gemPickup.length);

        //Destroy(gameObject);
    }

    private void Update()
    {
        transform.localEulerAngles += new Vector3(0, rotationSpeed * Time.deltaTime, 0);

        t += verticalMotionSpeed * Time.deltaTime;
        float yOffset = Mathf.Sin(t) * verticalMotionOffset;
        transform.position += new Vector3(0, yOffset, 0);

        if (moveTowardsTarget)
        {
            gemPickupSpeed += 40 * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, player.position, gemPickupSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, player.position) < 1f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void MoveTowards(Transform player)
    {
        //Tweener moveTowardsPlayer = transform.DOMove(player.position, 0.5f).SetEase(Ease.Linear);
        //moveTowardsPlayer.OnUpdate(delegate ()
        //{
        //    moveTowardsPlayer.ChangeEndValue(player.position, true);
        //    if (Vector3.Distance(transform.position, player.position) < 1f)
        //    {
        //        Destroy(gameObject);
        //    }
        //});
        
    }
}
