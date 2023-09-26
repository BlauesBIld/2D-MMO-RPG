using System.Net;
using Assets.Scripts;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet packet)
    {
        if (!Client.instance.IsConnected())
        {
            string msg = packet.ReadString();
            int myId = packet.ReadInt();

            Debug.Log($"Message from server: {msg}");
            Client.instance.myId = myId;
            //ClientSend.WelcomeReceived();

            Client.instance.SetConnected(true);

            Client.instance.udp.Connect(((IPEndPoint) Client.instance.tcp.socket.Client.LocalEndPoint).Port);
        }
        else
        {
            Debug.Log("Connection already established!");
        }
    }

    public static void SpawnPlayer(Packet packet)
    {
        int id = packet.ReadInt();
        string username = packet.ReadString();
        Vector3 position = packet.ReadVector3();

        GameManager.instance.SpawnPlayer(id, username, position);
    }

    public static void PlayerServerMovement(Packet packet)
    {
        int id = packet.ReadInt();
        Vector3 position = packet.ReadVector3();
        Vector3 movingDirection = packet.ReadVector3();
        //TODO: Detschn: Solve this crap
        if (GameManager.players.ContainsKey(id))
        {
            GameManager.players[id].Move(position, movingDirection);
        }
    }

    public static void PlayerDisconnected(Packet packet)
    {
        int id = packet.ReadInt();

        if (GameManager.players.ContainsKey(id))
        {
            Destroy(GameManager.players[id].gameObject);
            GameManager.players.Remove(id);
        }
    }

    public static void PlayerShootAnim(Packet packet)
    {
        int id = packet.ReadInt();
        Quaternion rotation = packet.ReadQuaternion();
        bool isRight = packet.ReadBool();

        GameObject fireBall = Instantiate(GameManager.instance.autoAttackPrefabs[(int) AutoAttacks.fireball],
            GameManager.players[id].transform.position, rotation);

        fireBall.GetComponent<FireBallShotController>().SetShootParameters(GameManager.players[id].gameObject, isRight);
        fireBall.GetComponent<FireBallShotController>().isLocal = false;
    }

    public static void SpawnEnemy(Packet packet)
    {
        int id = packet.ReadInt();
        Vector3 position = packet.ReadVector3();
        float maxHealth = packet.ReadFloat();
        float currentHealth = packet.ReadFloat();
        string type = packet.ReadString();

        GameManager.instance.SpawnEnemy(id, position, maxHealth, currentHealth, type);
    }

    public static void EnemyHealth(Packet packet)
    {
        int id = packet.ReadInt();
        float health = packet.ReadFloat();

        GameManager.enemies[id].SetHealth(health);
    }

    public static void EnemyMovement(Packet packet)
    {
        int id = packet.ReadInt();
        Vector3 position = packet.ReadVector3();
        Vector3 movingDirection = packet.ReadVector3();

        GameManager.enemies[id].Move(position, movingDirection);
    }

    public static void RemoveEnemy(Packet packet)
    {
        int enemyId = packet.ReadInt();
        GameManager.instance.RemoveEnemy(enemyId);
    }

    public static void PlayerServerHealth(Packet packet)
    {
        int id = packet.ReadInt();
        float currentHealth = packet.ReadFloat();

        //TODO: Detschn: Solve this crap
        if (GameManager.players.ContainsKey(id))
        {
            GameManager.players[id].GetComponent<HealthController>().SetCurrentHealth(currentHealth);
        }
    }

    public static void PlayerServerMana(Packet packet)
    {
        int id = packet.ReadInt();
        float currentMana = packet.ReadFloat();

        //TODO: Detschn: Solve this crap
        if (GameManager.players.ContainsKey(id))
        {
            GameManager.players[id].GetComponent<ManaController>().SetCurrentMana(currentMana);
        }
    }

    public static void PlayerServerCurrentExperience(Packet packet)
    {
        int id = packet.ReadInt();
        int currentExperience = packet.ReadInt();
        GameManager.players[id].GetComponent<ExperienceController>().SetCurrentExperience(currentExperience);
    }

    public static void PlayerServerMaxExperience(Packet packet)
    {
        int id = packet.ReadInt();
        int maxExperience = packet.ReadInt();
        GameManager.players[id].GetComponent<ExperienceController>().SetMaxExperience(maxExperience);
    }

    public static void PlayerServerLevel(Packet packet)
    {
        int id = packet.ReadInt();
        int level = packet.ReadInt();
        GameManager.players[id].GetComponent<ExperienceController>().SetLevel(level);
    }

    public static void PlayerSetHPRegenMultiplier(Packet packet)
    {
        int id = packet.ReadInt();
        int mutliplier = packet.ReadInt();
        GameManager.players[id].GetComponent<HealthController>().SetHealthRegenMultiplier(mutliplier);
    }

    public static void PlayerSetMPRegenMultiplier(Packet packet)
    {
        int id = packet.ReadInt();
        int mutliplier = packet.ReadInt();
        GameManager.players[id].GetComponent<ManaController>().SetManaRegenMultiplier(mutliplier);
    }

    public static void RemoveEnvironmentObj(Packet packet)
    {
        string assetName = packet.ReadString();
        Vector3 transformPosition = packet.ReadVector3();
        GameManager.instance.RemoveEnvironmentObj(assetName, transformPosition);
    }

    public static void SpawnEnvironmentObj(Packet packet)
    {
        string assetName = packet.ReadString();
        Vector3 transformPosition = packet.ReadVector3();
        GameManager.instance.SpawnEnvironmentObj(assetName, transformPosition);
    }

    public static void RegistrationResult(Packet packet)
    {
        bool successful = packet.ReadBool();
        string message = packet.ReadString();

        UIManager.instance.GetRegisterScreenContorller.ReceiveResponse(successful, message);
    }

    public static void ResponseClientSalt(Packet packet)
    {
        string clientSalt = packet.ReadString();

        UIManager.instance.GetLoginScreenController.ReceiveClientSalt(clientSalt);
    }

    public static void ResponsePasswordCheck(Packet packet)
    {
        bool passwordConfirmed = packet.ReadBool();

        UIManager.instance.GetLoginScreenController.ReceivePasswordCheck(passwordConfirmed);
    }

    public static void SendInventoryToPlayer(Packet packet)
    {
        string base64DecodedInventory = packet.ReadString();
        UIManager.instance.GetLoginScreenController.InventoryArrived();
        
        if (base64DecodedInventory != null)
        {
            UIManager.instance.GetInventoryController.playerInventory.LoadInventory(base64DecodedInventory);
        }
    }

    public static void SendLevelAndExperienceToPlayer(Packet packet)
    {
        int level = packet.ReadInt();
        int maxExp = packet.ReadInt();
        int exp = packet.ReadInt();
        
        UIManager.instance.GetCornerPlayerInterfaceController.experienceController.SetLevel(level);
        UIManager.instance.GetCornerPlayerInterfaceController.UpdateLevelText();
        UIManager.instance.GetCornerPlayerInterfaceController.experienceController.SetMaxExperience(maxExp);
        UIManager.instance.GetCornerPlayerInterfaceController.experienceController.SetCurrentExperience(exp);
        UIManager.instance.GetCornerPlayerInterfaceController.UpdateExperienceValue();
    }

    public static void RemovePlayer(Packet packet)
    {
        throw new System.NotImplementedException();
    }

    public static void SpawnEnemyShot(Packet packet)
    {
        throw new System.NotImplementedException();
    }
}