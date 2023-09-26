using UnityEngine;

public class ServerSend
{
    private static void SendTCPData(int toClient, Packet packet)
    {
        packet.WriteLength();
        Server.clients[toClient].tcp.SendData(packet);
    }

    private static void SendUDPData(int toClient, Packet packet)
    {
        packet.WriteLength();
        Server.clients[toClient].udp.SendData(packet);
    }

    private static void SendTCPDataToAll(Packet packet)
    {
        packet.WriteLength();
        for (int i = 1; i < Server.MaxPlayers; i++)
        {
            Server.clients[i].tcp.SendData(packet);
        }
    }

    private static void SendTCPDataToAll(int exceptClient, Packet packet)
    {
        packet.WriteLength();
        for (int i = 1; i < Server.MaxPlayers; i++)
        {
            if (i != exceptClient)
            {
                Server.clients[i].tcp.SendData(packet);
            }
        }
    }

    private static void SendUDPDataToAll(Packet packet)
    {
        packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].udp.SendData(packet);
        }
    }

    private static void SendUDPDataToAll(int exceptClient, Packet packet)
    {
        packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != exceptClient)
            {
                Server.clients[i].udp.SendData(packet);
            }
        }
    }

    #region Packets

    public static void Welcome(int toClient, string msg)
    {
        using (Packet packet = new Packet((int) ServerPackets.welcome))
        {
            packet.Write(msg);
            packet.Write(toClient);

            SendTCPData(toClient, packet);
        }
    }

    public static void SpawnPlayer(int toClient, Player player)
    {
        using (Packet packet = new Packet((int) ServerPackets.spawnPlayer))
        {
            packet.Write(player.id);
            packet.Write(player.username);
            packet.Write(player.transform.position);

            SendTCPData(toClient, packet);
        }
    }
    
    public static void RemovePlayer(int id, Player player)
    {
        using (Packet packet = new Packet((int) ServerPackets.removePlayer))
        {
            packet.Write(player.id);
            
            SendTCPData(id, packet);
        }
    }

    public static void PlayerServerMovement(Player player, Vector3 movingDirection)
    {
        using (Packet packet = new Packet((int) ServerPackets.playerServerMovement))
        {
            packet.Write(player.id);
            packet.Write(player.transform.position);
            packet.Write(movingDirection);

            SendUDPDataToAll(player.id, packet);
        }
    }

    public static void PlayerDisconnected(int playerId)
    {
        using (Packet packet = new Packet((int) ServerPackets.playerDisconnected))
        {
            packet.Write(playerId);

            SendTCPDataToAll(packet);
        }
    }

    public static void PlayerShootAnim(Player player, Quaternion direction, bool isRight)
    {
        using (Packet packet = new Packet((int) ServerPackets.playerShootAnim))
        {
            packet.Write(player.id);
            packet.Write(direction);
            packet.Write(isRight);

            SendTCPDataToAll(player.id, packet);
        }
    }

    public static void RemoveEnemy(int toPlayer, Enemy enemy)
    {
        using (Packet packet = new Packet((int) ServerPackets.removeEnemy))
        {
            packet.Write(enemy.id);
            SendTCPData(toPlayer, packet);
        }
    }

    public static void EnemyServerMovement(int toClient, Enemy enemy, Vector3 movingDirection)
    {
        using (Packet packet = new Packet((int) ServerPackets.enemyMovement))
        {
            packet.Write(enemy.id);
            packet.Write(enemy.transform.position);
            packet.Write(movingDirection);

            SendTCPData(toClient, packet);
        }
    }

    public static void SpawnEnemy(int toPlayer, Enemy enemy)
    {
        using (Packet packet = new Packet((int) ServerPackets.spawnEnemy))
        {
            packet.Write(enemy.id);
            packet.Write(enemy.transform.position);
            packet.Write(enemy.maxHealth);
            packet.Write(enemy.health);
            packet.Write(enemy.GetType().Name);

            SendTCPData(toPlayer, packet);
        }
    }

    public static void EnemyHealth(Enemy enemy)
    {
        using (Packet packet = new Packet((int) ServerPackets.enemyHealth))
        {
            packet.Write(enemy.id);
            packet.Write(enemy.health);

            SendTCPDataToAll(packet);
        }
    }

    public static void PlayerServerHealth(Player player)
    {
        using (Packet packet = new Packet((int) ServerPackets.playerServerHealth))
        {
            packet.Write(player.id);
            packet.Write(player.healthController.currentHealth);

            SendTCPDataToAll(player.id, packet);
        }
    }

    public static void PlayerServerMana(Player player)
    {
        using (Packet packet = new Packet((int) ServerPackets.playerServerMana))
        {
            packet.Write(player.id);
            packet.Write(player.manaController.currentMana);

            SendTCPDataToAll(player.id, packet);
        }
    }

    public static void PlayerServerCurrentExperience(Player player)
    {
        using (Packet packet = new Packet((int) ServerPackets.playerServerCurrentExperience))
        {
            packet.Write(player.id);
            packet.Write(player.experienceController.currentExperience);
            SendTCPData(player.id, packet);
        }
    }

    public static void PlayerServerMaxExperience(Player player)
    {
        using (Packet packet = new Packet((int) ServerPackets.playerServerMaxExperience))
        {
            packet.Write(player.id);
            packet.Write(player.experienceController.maxExperience);
            SendTCPData(player.id, packet);
        }
    }

    public static void PlayerServerLevel(Player player)
    {
        using (Packet packet = new Packet((int) ServerPackets.playerServerLevel))
        {
            packet.Write(player.id);
            packet.Write(player.experienceController.level);
            SendTCPData(player.id, packet);
        }
    }

    public static void PlayerSetHPRegenMultiplier(Player player)
    {
        using (Packet packet = new Packet((int) ServerPackets.playerSetHPRegenMultiplier))
        {
            packet.Write(player.id);
            packet.Write(player.healthController.healthRegenMultiplier);
            SendTCPData(player.id, packet);
        }
    }

    public static void PlayerSetMPRegenMultiplier(Player player)
    {
        using (Packet packet = new Packet((int) ServerPackets.playerSetMPRegenMultiplier))
        {
            packet.Write(player.id);
            packet.Write(player.manaController.manaRegenMultiplier);
            SendTCPData(player.id, packet);
        }
    }

    public static void SpawnEnvironmentObj(Player player, string gameObjectName, Vector3 transformPosition)
    {
        using (Packet packet = new Packet((int) ServerPackets.spawnEnvironmentObj))
        {
            packet.Write(gameObjectName);
            packet.Write(transformPosition);
            SendTCPData(player.id, packet);
        }
    }

    public static void RemoveEnvironmentObj(Player player, string gameObjectName, Vector3 transformPosition)
    {
        using (Packet packet = new Packet((int) ServerPackets.removeEnvironmentObj))
        {
            packet.Write(gameObjectName);
            packet.Write(transformPosition);
            SendTCPData(player.id, packet);
        }
    }

    public static void RegistrationResult(int toClient, bool successful, string message)
    {
        using (Packet packet = new Packet((int) ServerPackets.registrationResult))
        {
            packet.Write(successful);
            packet.Write(message);

            SendTCPData(toClient, packet);
        }
    }

    public static void ResponseClientSalt(int toClient, string clientSalt)
    {
        using (Packet packet = new Packet((int) ServerPackets.responseClientSalt))
        {
            packet.Write(clientSalt);
            SendTCPData(toClient, packet);
        }
    }

    public static void ResponsePasswordCheck(int toClient, bool checkPassword)
    {
        using (Packet packet = new Packet((int) ServerPackets.responsePasswordCheck))
        {
            packet.Write(checkPassword);
            SendTCPData(toClient, packet);
        }
    }

    public static void SendInventoryToPlayer(int toClient, string base64DecodedInventory)
    {
        using (Packet packet = new Packet((int) ServerPackets.sendInventoryToPlayer))
        {
            packet.Write(base64DecodedInventory);

            SendTCPData(toClient, packet);
        }
    }

    public static void SendLevelAndExperienceFromCharToPlayer(int toClient, int level, int maxExp, int exp)
    {
        using (Packet packet = new Packet((int) ServerPackets.sendLevelAndExperienceToPlayer))
        {
            packet.Write(level);
            packet.Write(maxExp);
            packet.Write(exp);

            SendTCPData(toClient, packet);
        }
    }

    public static void SpawnEnemyShot(int toClient, BasicAutoAttack autoAttack)
    {
        using (Packet packet = new Packet((int) ServerPackets.spawnEnemyShot))
        {
            packet.Write(autoAttack.name);
            packet.Write(autoAttack.transform.position);
            
            packet.Write(autoAttack.defaultSpeed);
            packet.Write(autoAttack.defaultRange);

            SendTCPData(toClient, packet);
        }
    }

    #endregion
}