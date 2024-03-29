﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.Sockets;
using System.Net;
using Newtonsoft.Json;

public class Client : MonoBehaviour
{
    public static int dataBufferSize = 4096;

    public int id;
    public Player player;
    public TCP tcp;
    public UDP udp;

    public Client(int clientId)
    {
        id = clientId;
        tcp = new TCP(id);
        udp = new UDP(id);
    }

    public class TCP
    {
        public TcpClient socket;

        private readonly int id;
        private NetworkStream stream;
        private Packet receivedData;
        private byte[] receiveBuffer;

        public TCP(int id)
        {
            this.id = id;
        }

        public void Connect(TcpClient clientSocket)
        {
            socket = clientSocket;
            socket.ReceiveBufferSize = dataBufferSize;
            socket.SendBufferSize = dataBufferSize;

            stream = clientSocket.GetStream();

            receivedData = new Packet();
            receiveBuffer = new byte[dataBufferSize];

            stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);

            ServerSend.Welcome(id, "Welcome to the Server");
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
            catch (Exception e)
            {
                Debug.Log($"Error sending data to player {id} via TCP: {e}");
            }
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                int bytelength = stream.EndRead(result);
                if (bytelength <= 0)
                {
                    Server.clients[id].Disconnect();
                    return;
                }

                byte[] data = new byte[bytelength];
                Array.Copy(receiveBuffer, data, bytelength);

                receivedData.Reset(HandleData(data));
                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
            }
            catch (Exception ex)
            {
                Debug.Log($"Error receiving TCP data: {ex}");
                Server.clients[id].Disconnect();
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
                        Server.packetHandlers[packetId](id, packet);
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

        public void Disconnect()
        {
            socket.Close();
            stream = null;
            receivedData = null;
            receiveBuffer = null;
            socket = null;
        }
    }

    public class UDP
    {
        public IPEndPoint endPoint;

        private int id;

        public UDP(int id)
        {
            this.id = id;
        }

        public void Connect(IPEndPoint endPoint)
        {
            this.endPoint = endPoint;
        }

        public void SendData(Packet packet)
        {
            Server.SendUDPData(endPoint, packet);
        }

        public void HandleData(Packet packetData)
        {
            int packetLength = packetData.ReadInt();
            byte[] packetBytes = packetData.ReadBytes(packetLength);

            ThreadManager.ExecuteOnMainThread(() =>
            {
                using (Packet packet = new Packet(packetBytes))
                {
                    int packetId = packet.ReadInt();
                    Server.packetHandlers[packetId](id, packet);
                }
            });
        }

        public void Disconnect()
        {
            endPoint = null;
        }
    }

    public void SendIntoGame(string playerName)
    {
        player = NetworkingManager.instance.InstantiatePlayer();
        player.Initialize(id, playerName);

        foreach (Client client in Server.clients.Values)
        {
            if (client.player != null)
            {
                if (client.id != id)
                {
                    ServerSend.SpawnPlayer(id, client.player);
                }
            }
        }

        foreach (Client client in Server.clients.Values)
        {
            if (client.player != null)
            {
                ServerSend.SpawnPlayer(client.id, player);
            }
        }
    }

    private void Disconnect()
    {
        Debug.Log($"{tcp.socket.Client.RemoteEndPoint} has disconnected.");
        
        ThreadManager.ExecuteOnMainThread((() =>
        {
            player.TellEnemiesNearbyImGone();
            Destroy(player.gameObject);
            player = null;
        }));      
                  

        tcp.Disconnect();
        udp.Disconnect();

        ServerSend.PlayerDisconnected(id);
    }

    public void LoadInventory()
    {
        player.inventory.items = JsonConvert.DeserializeObject<Item[]>(CryptoMethods.EncodeFromBase64String(DBInventories.GetInventoryFromUserChar(new InventoryObj(player.username, 1))));
    }
}
