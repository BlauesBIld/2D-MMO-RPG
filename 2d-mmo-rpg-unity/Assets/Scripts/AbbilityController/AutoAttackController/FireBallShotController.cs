using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class FireBallShotController : MonoBehaviour
{
    public bool isLocal = true;

    public bool isRightShooting;

    public Slider leftPreShot;
    public Slider rightPreShot;

    public GameObject leftSpawnPosition;
    public GameObject rightSpawnPosition;
    private Vector3 fireBallSpawnPositionOffset = new Vector3(0f, 0f, 1.5f);

    public GameObject fireBallPrefab;

    private float magicDamage;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        leftPreShot.value = 0;
        rightPreShot.value = 0;

        StartCoroutine(StartAnim());
    }

    void Update()
    {
        transform.position = player.transform.position + fireBallSpawnPositionOffset;
    }

    private IEnumerator StartAnim()
    {
        for (float i = 0; i <= 1.01f; i += 0.166f)
        {
            if (isRightShooting)
            {
                rightPreShot.value = i;
            }
            else
            {
                leftPreShot.value = i;
            }
            yield return new WaitForSeconds(0.005f);
        }

        InstantiateFireBall();

        yield return new WaitForSeconds(0.1f);
        Destroy(this.gameObject);
    }

    private void InstantiateFireBall()
    {
        AutoAttackController fireBall;
        if (isRightShooting)
        {
            fireBall = Instantiate(fireBallPrefab, rightSpawnPosition.transform.position, transform.rotation).GetComponent<AutoAttackController>();
        }
        else
        {
            fireBall = Instantiate(fireBallPrefab, leftSpawnPosition.transform.position, transform.rotation).GetComponent<AutoAttackController>();
        }

        fireBall.SetParamaters(isLocal, magicDamage);
    }

    public void SetShootParameters(GameObject player, bool isRightShooting, float magicDamage = 0f)
    {
        this.magicDamage = magicDamage;
        this.player = player;
        this.isRightShooting = isRightShooting;
    }
}