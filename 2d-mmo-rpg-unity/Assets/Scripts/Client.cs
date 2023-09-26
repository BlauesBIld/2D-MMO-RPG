using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

public class Client : MonoBehaviour
{
    public static Client instance;
    public static int dataBufferSize = 4096;

    public string dnsName = "detschn.ddns.net";
    private IPAddress ip;
    public int port = 13061;
    public int myId = 0;
    public TCP tcp;
    public UDP udp;

    private bool isConnected = false;

    private delegate void PacketHandler(Packet packet);

    private static Dictionary<int, PacketHandler> packetHandlers;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    private void OnApplicationQuit()
    {
        Disconnect();
    }

    public void ConnectToServer()
    {
        ip = Dns.GetHostAddresses(dnsName)[0];

        tcp = new TCP();
        udp = new UDP();

        InitializeClientData();

        tcp.Connect();
    }

    public class TCP
    {
        public TcpClient socket;

        private NetworkStream stream;
        private Packet receivedData;
        private byte[] receiveBuffer;

        public void Connect()
        {
            socket = new TcpClient
            {
                ReceiveBufferSize = dataBufferSize,
                SendBufferSize = dataBufferSize
            };

            receiveBuffer = new byte[dataBufferSize];
            socket.BeginConnect(instance.ip, instance.port, ConnectCallback, socket);
        }

        private void ConnectCallback(IAsyncResult result)
        {
            if (result == null)
            {
                return;
            }

            socket.EndConnect(result);

            if (!socket.Connected)
            {
                return;
            }

            stream = socket.GetStream();

            receivedData = new Packet();

            stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
        }

        public void SendData(Packet packet)
        {
            try
            {
                if (socket != null)
                {
                    stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);
                }
            }
            catch (Exception ex)
            {
                Debug.Log($"Error sending data to server via TCP: {ex}");
            }
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                int bytelength = stream.EndRead(result);
                if (bytelength <= 0)
                {
                    instance.Disconnect();
                    return;
                }

                byte[] data = new byte[bytelength];
                Array.Copy(receiveBuffer, data, bytelength);

                receivedData.Reset(HandleData(data));

                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error receiving TCP data: {ex}");
                Disconnect();
            }
        }

        private bool HandleData(byte[] data)
        {
            int packetLength = 0;

            receivedData.SetBytes(data);

            if (receivedData.UnreadLength() >= 4)
            {
                packetLength = receivedData.ReadInt();
                if (packetLength <= 0)
                {
                    return true;
                }
            }

            while (packetLength > 0 && packetLength <= receivedData.UnreadLength())
            {
                byte[] packetBytes = receivedData.ReadBytes(packetLength);
                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using (Packet packet = new Packet(packetBytes))
                    {
                        int packetId = packet.ReadInt();
                        packetHandlers[packetId](packet);
                    }
                });

                packetLength = 0;
                if (receivedData.UnreadLength() >= 4)
                {
                    packetLength = receivedData.ReadInt();
                    if (packetLength <= 0)
                    {
                        return true;
                    }
                }
            }

            if (packetLength <= 1)
            {
                return true;
            }

            return false;
        }

        private void Disconnect()
        {
            instance.Disconnect();

            stream = null;
            receivedData = null;
            receiveBuffer = null;
            socket = null;
        }
    }

    public class UDP
    {
        public UdpClient socket;
        public IPEndPoint endPoint;

        public UDP()
        {
            endPoint = new IPEndPoint(instance.ip, instance.port);
        }

        public void Connect(int localPort)
        {
            socket = new UdpClient(localPort);

            socket.Connect(endPoint);
            socket.BeginReceive(ReceiveCallback, null);

            using (Packet packet = new Packet())
            {
                SendData(packet);
            }
        }

        public void SendData(Packet packet)
        {
            try
            {
                packet.InsertInt(instance.myId);
                if (socket != null)
                {
                    socket.BeginSend(packet.ToArray(), packet.Length(), null, null);
                }
            }
            catch (Exception ex)
            {
                Debug.Log($"Error sending data to server via UDP: {ex}");
            }
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                byte[] data = socket.EndReceive(result, ref endPoint);
                socket.BeginReceive(ReceiveCallback, null);

                if (data.Length < 4)
                {
                    instance.Disconnect();
                    return;
                }

                HandleData(data);
            }
            catch
            {
                Disconnect();
            }
        }

        private void HandleData(byte[] data)
        {
            using (Packet packet = new Packet(data))
            {
                int packetLength = packet.ReadInt();
                data = packet.ReadBytes(packetLength);
            }

            ThreadManager.ExecuteOnMainThread(() =>
            {
                using (Packet packet = new Packet(data))
                {
                    int packetId = packet.ReadInt();
                    packetHandlers[packetId](packet);
                }
            });
        }

        private void Disconnect()
        {
            instance.Disconnect();

            endPoint = null;
            socket = null;
        }
    }

    private void InitializeClientData()
    {
        packetHandlers = new Dictionary<int, PacketHandler>()
        {
            {(int) ServerPackets.welcome, ClientHandle.Welcome},
            {(int) ServerPackets.spawnPlayer, ClientHandle.SpawnPlayer},
            {(int) ServerPackets.removePlayer, ClientHandle.RemovePlayer},
            {(int) ServerPackets.playerServerMovement, ClientHandle.PlayerServerMovement},
            {(int) ServerPackets.playerDisconnected, ClientHandle.PlayerDisconnected},
            {(int) ServerPackets.playerShootAnim, ClientHandle.PlayerShootAnim},
            {(int) ServerPackets.spawnEnemy, ClientHandle.SpawnEnemy},
            {(int) ServerPackets.removeEnemy, ClientHandle.RemoveEnemy},
            {(int) ServerPackets.enemyMovement, ClientHandle.EnemyMovement},
            {(int) ServerPackets.enemyHealth, ClientHandle.EnemyHealth},
            {(int) ServerPackets.playerServerHealth, ClientHandle.PlayerServerHealth},
            {(int) ServerPackets.playerServerMana, ClientHandle.PlayerServerMana},
            {(int) ServerPackets.playerServerCurrentExperience, ClientHandle.PlayerServerCurrentExperience},
            {(int) ServerPackets.playerServerMaxExperience, ClientHandle.PlayerServerMaxExperience},
            {(int) ServerPackets.playerServerLevel, ClientHandle.PlayerServerLevel},
            {(int) ServerPackets.playerSetHPRegenMultiplier, ClientHandle.PlayerSetHPRegenMultiplier},
            {(int) ServerPackets.playerSetMPRegenMultiplier, ClientHandle.PlayerSetMPRegenMultiplier},
            {(int) ServerPackets.removeEnvironmentObj, ClientHandle.RemoveEnvironmentObj},
            {(int) ServerPackets.spawnEnvironmentObj, ClientHandle.SpawnEnvironmentObj},
            {(int) ServerPackets.registrationResult, ClientHandle.RegistrationResult},
            {(int) ServerPackets.responseClientSalt, ClientHandle.ResponseClientSalt},
            {(int) ServerPackets.responsePasswordCheck, ClientHandle.ResponsePasswordCheck},
            {(int) ServerPackets.sendInventoryToPlayer, ClientHandle.SendInventoryToPlayer},
            {(int) ServerPackets.sendLevelAndExperienceToPlayer, ClientHandle.SendLevelAndExperienceToPlayer},
            {(int) ServerPackets.spawnEnemyShot, ClientHandle.SpawnEnemyShot},
        };
        Debug.Log("Initialized Packets.");
    }

    public bool IsConnected()
    {
        return isConnected;
    }

    public void SetConnected(bool isConnected)
    {
        this.isConnected = isConnected;
    }

    private void Disconnect()
    {
        if (isConnected)
        {
            isConnected = false;
            tcp.socket.Close();
            udp.socket.Close();

            Debug.Log("Disconnected from Server.");
        }
    }
}