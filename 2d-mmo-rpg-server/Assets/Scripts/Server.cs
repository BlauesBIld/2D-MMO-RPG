using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;

public class Server
{
    public static int MaxPlayers { get; private set; }
    public static int Port { get; private set; }
    public static Dictionary<int, Client> clients = new Dictionary<int, Client>();

    public delegate void PacketHandler(int fromClient, Packet packet);

    public static Dictionary<int, PacketHandler> packetHandlers;

    private static TcpListener tcpListener;
    private static UdpClient udpListener;

    public static void Start(int maxPlayers, int port)
    {
        MaxPlayers = maxPlayers;
        Port = port;

        Debug.Log("Starting server...");
        InitializeServerData();
        IPAddress addr = IPAddress.Any;
        tcpListener = new TcpListener(addr, Port);
        Debug.Log($"Server using IPAddress: {addr}");
        tcpListener.Start();
        tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);

        udpListener = new UdpClient(Port);
        udpListener.BeginReceive(UDPReceiveCallback, null);

        Debug.Log($"Server started on port {Port}.");
        Debug.Log($"Server using Persistent Data Path: {Application.persistentDataPath}");
    }

    private static void TCPConnectCallback(IAsyncResult result)
    {
        TcpClient client = tcpListener.EndAcceptTcpClient(result);
        tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);
        Debug.Log($"Incoming connection from {client.Client.RemoteEndPoint}...");

        for (int i = 1; i <= MaxPlayers; i++)
        {
            if (clients[i].tcp.socket == null)
            {
                clients[i].tcp.Connect(client);
                return;
            }
        }

        Debug.Log($"{client.Client.RemoteEndPoint} failed to connect: Server full!");
    }

    private static void UDPReceiveCallback(IAsyncResult result)
    {
        try
        {
            IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] data = udpListener.EndReceive(result, ref clientEndPoint);
            udpListener.BeginReceive(UDPReceiveCallback, null);

            if (data.Length < 4)
            {
                return;
            }

            using (Packet packet = new Packet(data))
            {
                int clientId = packet.ReadInt();

                if (clientId == 0)
                {
                    return;
                }

                if (clients[clientId].udp.endPoint == null)
                {
                    clients[clientId].udp.Connect(clientEndPoint);
                    return;
                }

                if (clients[clientId].udp.endPoint.ToString() == clientEndPoint.ToString())
                {
                    clients[clientId].udp.HandleData(packet);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log($"Error receiving UDP data: {ex}");
        }
    }

    public static void SendUDPData(IPEndPoint clientEndPoint, Packet packet)
    {
        try
        {
            if (clientEndPoint != null)
            {
                udpListener.BeginSend(packet.ToArray(), packet.Length(), clientEndPoint, null, null);
            }
        }
        catch (Exception ex)
        {
            Debug.Log($"Error sending data to {clientEndPoint} via UDP: {ex}");
        }
    }

    private static void InitializeServerData()
    {
        for (int i = 1; i <= MaxPlayers; i++)
        {
            clients.Add(i, new Client(i));
        }

        packetHandlers = new Dictionary<int, PacketHandler>()
        {
            {(int) ClientPackets.welcomeReceived, ServerHandle.WelcomeReceived},
            {(int) ClientPackets.playerMovement, ServerHandle.PlayerMovement},
            {(int) ClientPackets.playerShoot, ServerHandle.PlayerShoot},
            {(int) ClientPackets.dealDamageToEnemy, ServerHandle.DealDamageToEnemy},
            {(int) ClientPackets.playerHealth, ServerHandle.PlayerHealth},
            {(int) ClientPackets.playerMana, ServerHandle.PlayerMana},
            {(int) ClientPackets.requestHPCrystalEffect, ServerHandle.RequestHPCrystalEffect},
            {(int) ClientPackets.requestMPCrystalEffect, ServerHandle.RequestMPCrystalEffect},
            {(int) ClientPackets.registerPlayer, ServerHandle.RegisterPlayer},
            {(int) ClientPackets.requestClientSaltForPlayer, ServerHandle.RequestClientSalt},
            {(int) ClientPackets.requestPasswordCheck, ServerHandle.RequestPasswordCheck},
            {(int) ClientPackets.requestInventory, ServerHandle.RequestInventory},
            {(int) ClientPackets.moveItemFromSlotToSlot, ServerHandle.MoveItemFromSlotToSlot},
            {(int) ClientPackets.requestLevelAndExperience, ServerHandle.RequestLevelAndExperience},
        };
        Debug.Log("Initialized packets.");
    }

    public static void Stop()
    {
        tcpListener.Stop();
        udpListener.Close();
    }
}