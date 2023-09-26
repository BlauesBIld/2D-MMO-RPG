using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class MageController : MonoBehaviour
{
    public float autoAttackCooldown = 0.166f;
    public float teleportCooldown = 2f;
    public float teleportDistance = 15f;
    public float teleportManaCost = 25f;
    private float magicDamage = 10f;

    private float autoAttackUsedLastTime;
    private float teleportUsedLastTime;

    private bool isCoroutineExecuting = false;

    private bool isRightSideShooting = true;
    private bool isShooting = false;

    private ManaController manaController;

    // Start is called before the first frame update
    void Awake()
    {
        manaController = GetComponent<ManaController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsMouseOverUI() && Input.GetMouseButtonDown(0))
        {
            StartShooting();
        }

        if (!IsMouseOverUI() && Input.GetMouseButtonUp(0))
        {
            StopShooting();
        }

        if (Input.GetKey(KeyCode.E))
        {
            StartCoroutine(TeleportInMouseDirection());
        }

        if (Input.GetKey(KeyCode.F))
        {
            IceSwipe();
        }

        if (isShooting)
        {
            Shoot();
        }
    }

    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject() ||
               UIManager.instance.GetInventoryController.isDraggingItem;
    }

    private void StartShooting()
    {
        isShooting = true;
    }

    private void StopShooting()
    {
        isShooting = false;
    }

    private void Shoot()
    {
        if (Time.time > autoAttackUsedLastTime)
        {
            autoAttackUsedLastTime = Time.time + autoAttackCooldown;

            ClientSend.PlayerShoot(GetAngleFromPlayerToMousePosition(), isRightSideShooting);

            GameObject fireBall = Instantiate(GameManager.instance.autoAttackPrefabs[(int) AutoAttacks.fireball],
                transform.position, GetAngleFromPlayerToMousePosition());

            fireBall.GetComponent<FireBallShotController>()
                .SetShootParameters(this.gameObject, isRightSideShooting, magicDamage);
            isRightSideShooting = !isRightSideShooting;
        }
    }

    private Quaternion GetAngleFromPlayerToMousePosition()
    {
        Vector3 mousePosition2D = Input.mousePosition;
        mousePosition2D.z = 5;
        mousePosition2D.y -= 15;
        Vector3 mouseInWorldPosition =
            transform.GetComponentInChildren<Camera>().ScreenToWorldPoint(mousePosition2D);
        float angleRad = Mathf.Atan2(mouseInWorldPosition.x - transform.position.x,
            mouseInWorldPosition.z - transform.position.z);
        float angleDeg = (180 / Mathf.PI) * angleRad;
        Quaternion directionToShoot = Quaternion.Euler(90, angleDeg, 0);
        return directionToShoot;
    }

    void IceSwipe()
    {
    }

    IEnumerator TeleportInMouseDirection()
    {
        if (isCoroutineExecuting)
        {
            yield break;
        }

        isCoroutineExecuting = true;


        if (Time.time > teleportUsedLastTime)
        {
            if (manaController.UseMana(teleportManaCost))
            {
                yield return new WaitForSeconds(0.1f);

                teleportUsedLastTime = Time.time + teleportCooldown;

                Vector3 mousePosition2D = Input.mousePosition;
                mousePosition2D.z = 5;
                Vector3 mousePosition = transform.GetComponentInChildren<Camera>().ScreenToWorldPoint(mousePosition2D);

                Vector3 direction = (mousePosition - transform.position);
                direction.y = 0;
                direction = direction.normalized;

                Vector3 teleportDirection = new Vector3(direction.x, 0f, direction.z);

                transform.position += teleportDirection * teleportDistance;
            }
            else
            {
                StartCoroutine(UIManager.instance.GetCornerPlayerInterfaceController.ShowMissingMana(teleportManaCost));
            }
        }

        isCoroutineExecuting = false;
    }
}