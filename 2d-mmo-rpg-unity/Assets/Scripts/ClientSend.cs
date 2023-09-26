using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    private static void SendTCPData(Packet packet)
    {
        packet.WriteLength();
        Client.instance.tcp.SendData(packet);
    }

    private static void SendUDPData(Packet packet)
    {
        packet.WriteLength();
        Client.instance.udp.SendData(packet);
    }

    #region Packets

    public static void WelcomeReceived(string username)
    {
        using (Packet packet = new Packet((int) ClientPackets.welcomeReceived))
        {
            packet.Write(Client.instance.myId);
            packet.Write(username);

            SendTCPData(packet);
        }
    }

    public static void PlayerMovement(Vector3 position, Vector3 movingDirection)
    {
        using (Packet packet = new Packet((int) ClientPackets.playerMovement))
        {
            packet.Write(position);
            packet.Write(movingDirection);

            SendUDPData(packet);
        }
    }

    public static void PlayerShoot(Quaternion rotation, bool isRight)
    {
        using (Packet packet = new Packet((int) ClientPackets.playerShoot))
        {
            packet.Write(rotation);
            packet.Write(isRight);

            SendTCPData(packet);
        }
    }

    public static void DealDamageToEnemy(EnemyBaseController enemy, float magicDamage, float physicalDamage)
    {
        using (Packet packet = new Packet((int) ClientPackets.dealDamageToEnemy))
        {
            packet.Write(enemy.id);
            packet.Write(magicDamage);
            packet.Write(physicalDamage);

            SendTCPData(packet);
        }
    }

    public static void PlayerHealth(float currentHealth)
    {
        using (Packet packet = new Packet((int) ClientPackets.playerHealth))
        {
            packet.Write(currentHealth);

            SendTCPData(packet);
        }
    }

    public static void PlayerMana(float currentMana)
    {
        using (Packet packet = new Packet((int) ClientPackets.playerMana))
        {
            packet.Write(currentMana);

            SendTCPData(packet);
        }
    }


    public static void RegisterUserLogin(string username, string clientHashedAndSaltedPw, string clientSalt)
    {
        using (Packet packet = new Packet((int) ClientPackets.registerPlayer))
        {
            packet.Write(username);
            packet.Write(clientSalt);
            packet.Write(clientHashedAndSaltedPw);

            SendTCPData(packet);
        }
    }
    
    public static void RequestClientSaltForUser(string username)
    {
        using (Packet packet = new Packet((int) ClientPackets.requestClientSaltForPlayer))
        {
            packet.Write(username);

            SendTCPData(packet);
        }
    }

    public static void RequestHPCrystalEffect()
    {
        using (Packet packet = new Packet((int) ClientPackets.requestHPCrystalEffect))
        {
            SendTCPData(packet);
        }
    }

    public static void RequestMPCrystalEffect()
    {
        using (Packet packet = new Packet((int) ClientPackets.requestMPCrystalEffect))
        {
            SendTCPData(packet);
        }
    }
    
    public static void RequestPasswordCheck(string username, string clientHashedAndSaltedPassword)
    {
        using (Packet packet = new Packet((int) ClientPackets.requestPasswordCheck))
        {
            packet.Write(username);
            packet.Write(clientHashedAndSaltedPassword);
            
            SendTCPData(packet);
        }
    }

    public static void RequestInventory(int charslot)
    {
        using (Packet packet = new Packet((int) ClientPackets.requestInventory))
        {
            packet.Write(charslot); //charslot, currently there is only one charslot TODO: add more charslots
            
            SendTCPData(packet);
        }
    }

    public static void MoveItemFromSlotToSlot(int fromSlotNr, int toSlotNr)
    {
        using (Packet packet = new Packet((int) ClientPackets.moveItemFromSlotToSlot))
        {
            packet.Write(fromSlotNr);
            packet.Write(toSlotNr);
            
            SendTCPData(packet);
        }
    }
    
    public static void RequestLevelAndExperience(int charSlot)
    {
        using (Packet packet = new Packet((int) ClientPackets.requestLevelAndExperience))
        {
            packet.Write(charSlot);
            
            SendTCPData(packet);
        }
    }
    
    #endregion
}