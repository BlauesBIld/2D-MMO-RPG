using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private float moveSpeed = 20f;
    public Vector3 movingDirection = Vector3.zero;

    private ManaController manaController;
    private HealthController healthController;
    private ExperienceController experienceController;

    private Collider hitCollider;

    private void Awake()
    {
        UIManager.instance.setPlayerScriptComponents(GetComponents<MonoBehaviour>());

        manaController = GetComponent<ManaController>();
        healthController = GetComponent<HealthController>();
        experienceController = GetComponent<ExperienceController>();
    }

    private void Start()
    {
        ClientSend.PlayerHealth(healthController.currentHealth);
        ClientSend.PlayerMana(manaController.currentMana);

        StartCoroutine(UpdateHealthAndManaToServer());
        
        UIManager.instance.GetCornerPlayerInterfaceController.InitializeBars(healthController, manaController, experienceController);
        
        // Item seas = new Weapon("Book", 0f, 20f, "Book");
        // Item seas1 = new Consumable("MP Crystal", "MPCrystal", 7, 1);

        // inventory.PutItemInSlot(seas, 1);
        // inventory.PutItemInSlot(seas1, 2);
    }

    private void Update()
    {
        healthController.Regenerate();
        manaController.Regenerate();

        Move(GetVector3FromBoolArray(GetMovementInputs()));

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenMenu();
        }


        if (Input.GetKeyDown(KeyCode.B))
        {
            if (UIManager.instance.GetInventoryController.IsInventoryOpen())
            {
                UIManager.instance.GetInventoryController.CloseInventory();
            }
            else
            {
                UIManager.instance.GetInventoryController.OpenInventory();
            }
        }
    }
    private void FixedUpdate()
    {
        SendPositionAndMovingDirectionToServer();
        GetComponent<Rigidbody>().velocity = movingDirection.normalized * moveSpeed;
    }

    private IEnumerator UpdateHealthAndManaToServer()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            ClientSend.PlayerMana(manaController.currentMana);
            ClientSend.PlayerHealth(healthController.currentHealth);
        }
    }

    private void OpenMenu()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        UIManager.instance.OpenInGameMenu();
    }

    private void SendPositionAndMovingDirectionToServer()
    {
        ClientSend.PlayerMovement(transform.position, GetVector3FromBoolArray(GetMovementInputs()));
    }

    private Vector3 GetVector3FromBoolArray(bool[] inputs)
    {
        Vector3 inputDirection = Vector3.zero;
        if (inputs[0])
        {
            inputDirection.z += 1;
        }

        if (inputs[1])
        {
            inputDirection.x -= 1;
        }

        if (inputs[2])
        {
            inputDirection.z -= 1;
        }

        if (inputs[3])
        {
            inputDirection.x += 1;
        }

        return inputDirection;
    }

    public void Move(Vector3 inputDirection)
    {
        movingDirection = transform.right * inputDirection.x + transform.up * inputDirection.z;
        //transform.Translate(moveDirection.normalized * moveSpeed * Time.deltaTime, Space.World);
    }

    private bool[] GetMovementInputs()
    {
        bool[] inputs = new bool[]
        {
            Input.GetKey(KeyCode.W),
            Input.GetKey(KeyCode.A),
            Input.GetKey(KeyCode.S),
            Input.GetKey(KeyCode.D)
        };

        return inputs;
    }
}