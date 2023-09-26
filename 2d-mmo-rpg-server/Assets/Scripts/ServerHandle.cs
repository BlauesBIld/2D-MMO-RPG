using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Database;
using Newtonsoft.Json;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class ServerHandle
{
    public static void WelcomeReceived(int fromClient, Packet packet)
    {
        int clientIdCheck = packet.ReadInt();
        string username = packet.ReadString();

        Debug.Log($"{Server.clients[fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully with username: \"{username}\" and has the id: {fromClient}");
        if (fromClient != clientIdCheck)
        {
            Debug.Log($"Player \"{username}\" (ID: {fromClient}) has assumed the wrong client ID ({clientIdCheck})");
        }
        Server.clients[fromClient].SendIntoGame(username);
        Server.clients[fromClient].LoadInventory();
    }

    public static void PlayerMovement(int fromClient, Packet packet)
    {
        Vector3 position = packet.ReadVector3();
        Vector3 movingDirection = packet.ReadVector3();

        Server.clients[fromClient].player.SetPositionAndMovingDirection(position, movingDirection);
    }

    public static void PlayerShoot(int fromClient, Packet packet)
    {
        Quaternion rotation = packet.ReadQuaternion();
        bool isRight = packet.ReadBool();

        Server.clients[fromClient].player.Shoot(rotation, isRight);
    }

    public static void DealDamageToEnemy(int fromclient, Packet packet)
    {
        int id = packet.ReadInt();
        float magicDamage = packet.ReadFloat();
        float physicalDamage = packet.ReadFloat();

        EnemyManager.instance.enemies[id].TakeDamage(magicDamage, physicalDamage);
    }

    public static void PlayerHealth(int fromClient, Packet packet)
    {
        float currentHealth = packet.ReadFloat();

        Server.clients[fromClient].player.SetHealth(currentHealth);
    }

    public static void PlayerMana(int fromClient, Packet packet)
    {
        float currentMana = packet.ReadFloat();

        Server.clients[fromClient].player.SetMana(currentMana);
        ServerSend.PlayerServerMana(Server.clients[fromClient].player);
    }

    public static void RequestHPCrystalEffect(int fromClient, Packet packet)
    {

    }

    public static void RequestMPCrystalEffect(int fromClient, Packet packet)
    {
        Server.clients[fromClient].player.manaController.TemporaryAddOneMultiplierToBaseManaRegen(6f);
    }

    public static void RegisterPlayer(int fromclient, Packet packet)
    {
        string username = packet.ReadString();
        string clientSalt = packet.ReadString();
        string clientHashedAndSaltedPassword = packet.ReadString();
        
        RegistrationObj registrationObj = DBUserLogins.PrepareNewRegistration(username, clientSalt, clientHashedAndSaltedPassword);
        
        DBUserLogins.RegisterNewLogin(Server.clients[fromclient].id, registrationObj);
    }

    public static void RequestClientSalt(int fromclient, Packet packet)
    {
        string username = packet.ReadString();

        string clientSalt = DBUserLogins.GetClientSaltForUser(username) ?? "";
        ServerSend.ResponseClientSalt(Server.clients[fromclient].id, clientSalt);
    }

    public static void RequestPasswordCheck(int fromclient, Packet packet)
    {
        string clientUsername = packet.ReadString();
        string clientHashedAndSaltedPassword = packet.ReadString();

        ServerSend.ResponsePasswordCheck(Server.clients[fromclient].id, DBUserLogins.CheckPassword(clientUsername, clientHashedAndSaltedPassword));
    }
    
    public static void RequestInventory(int fromclient, Packet packet)
    {
        int charslot = packet.ReadInt();
        
        string inventoryFromUserChar = DBInventories.GetInventoryFromUserChar(new InventoryObj(Server.clients[fromclient].player.username, charslot));

        ServerSend.SendInventoryToPlayer(fromclient, inventoryFromUserChar);
    }

    public static void MoveItemFromSlotToSlot(int fromclient, Packet packet)
    {
        int fromSlot = packet.ReadInt();
        int toSlot = packet.ReadInt();
        
        Server.clients[fromclient].player.inventory.MoveItemFromSlotToSlot(fromSlot, toSlot);

        string base64CodedInventory = CryptoMethods.DecodeToBase64String(JsonConvert.SerializeObject(Server.clients[fromclient].player.inventory.items, Formatting.None, new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.All}));
        DBInventories.SaveInventoryToDB(new InventoryObj(Server.clients[fromclient].player.username, 1, base64CodedInventory));
        ServerSend.SendInventoryToPlayer(fromclient, base64CodedInventory);
    }

    public static void RequestLevelAndExperience(int fromclient, Packet packet)
    {
        int charSlot = packet.ReadInt();
        
        ExperienceObj expObj = new ExperienceObj(Server.clients[fromclient].player.username, charSlot);
        
        int level = DBExperience.GetLevelForUserChar(expObj);
        int maxExp = DBExperience.GetMaxExpForUserChar(expObj);
        int exp = DBExperience.GetExperienceForUserChar(expObj);
        
        Server.clients[fromclient].player.experienceController.SetCurrentExperience(exp);
        Server.clients[fromclient].player.experienceController.SetMaxExperience(maxExp);
        Server.clients[fromclient].player.experienceController.SetLevel(level);
        
        ServerSend.SendLevelAndExperienceFromCharToPlayer(fromclient, level, maxExp, exp);
    }
}
