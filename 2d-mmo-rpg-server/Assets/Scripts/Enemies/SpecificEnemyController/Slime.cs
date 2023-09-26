using System.Collections;
using UnityEngine;

public class Slime : Enemy
{
    public float movementSpeed = 3f;

    private float setMajorMovingDirectionCooldownInSeconds = 3f;
    private float setMinorMovingDirectionCooldownInSeconds = 0.5f;

    private float autoAttackCooldown = 3f;
    private float timeStampAutoAttackLastTimeUsed;

    public GameObject autoAttackPrefab;

    private void Start()
    {
        StartCoroutine(SlimeMovement());
    }

    private void FixedUpdate()
    {
        Shoot();

        GetComponent<Rigidbody>().velocity = movingDirection * movementSpeed;
    }

    private void Shoot()
    {
        if (Time.time > timeStampAutoAttackLastTimeUsed)
        {
            timeStampAutoAttackLastTimeUsed = Time.time + autoAttackCooldown;

            InstantiateSlimeBall();
        }
    }

    private void InstantiateSlimeBall()
    {
        autoAttackPrefab.GetComponent<SlimeBall>().rotation = GetAngleFromEnemyToNearestPlayer();
        Instantiate(autoAttackPrefab);
    }

    private IEnumerator SlimeMovement()
    {
        while (true)
        {
            Player nearestPlayer = GetNearestPlayer();

            if (nearestPlayer != null)
            {
                Vector3 nearestPlayerPosition = nearestPlayer.transform.position;
                Vector3 slimePosition = transform.position;

                float distanceBetweenPlayerAndEnemy = Vector3.Distance(nearestPlayer.transform.position, this.transform.position);

                movingDirection = new Vector3(
                    nearestPlayerPosition.x - slimePosition.x,
                    0,
                    nearestPlayerPosition.z - slimePosition.z
                ).normalized;

                SendMovementAndPositionToPlayersNearby();
                yield return new WaitForSeconds(setMajorMovingDirectionCooldownInSeconds);

                if (Random.Range(0, 2) == 1)
                {
                    movingDirection = new Vector3(
                        -(slimePosition.x - nearestPlayerPosition.x),
                        0,
                        slimePosition.z - nearestPlayerPosition.z
                    ).normalized;
                }
                else
                {
                    movingDirection = new Vector3(
                        slimePosition.x - nearestPlayerPosition.x,
                        0,
                        -(slimePosition.z - nearestPlayerPosition.z)
                    ).normalized;
                }

                SendMovementAndPositionToPlayersNearby();
                yield return new WaitForSeconds(setMinorMovingDirectionCooldownInSeconds);
            }
            else
            {
                movingDirection = Vector3.zero;
                yield return new WaitForSeconds(1f);
                SendMovementAndPositionToPlayersNearby();
            }
        }
    }
}