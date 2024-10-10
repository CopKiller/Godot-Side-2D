using System;
using Infrastructure.Logger;
using Infrastructure.Network.CustomDataSerializable;
using LiteNetLib;
using Side2D.scripts.MainScripts.Game;

namespace Side2D.scripts.Network
{
    public class ClientPlayer(ClientPacketProcessor clientPacketProcessor)
    {
        private ClientPacketProcessor ClientPacketProcessor { get; } = clientPacketProcessor;
        
        //public Action? LoadGameEntities;
        
        /// <summary>
        /// Get the current latency between server and client
        /// </summary>
        public int Ping { get; set; }

        /// <summary>
        /// The network server peer
        /// </summary>
        public NetPeer RemotePeer { get; set; } = null;

        /// <summary>
        /// The current peer
        /// </summary>
        public NetPeer CurrentPeer { get; set; } = null;
        
        /// <summary>
        ///  Player Index in the server
        /// </summary>
        public int PlayerIndex { get; set; } = -1;
        
        public bool CheckConnection()
        {
            return RemotePeer?.ConnectionState == ConnectionState.Connected;
        }
        
        public void Disconnect()
        {
            // Na duvida eu desconecto os dois
            CurrentPeer?.Disconnect();
            RemotePeer?.Disconnect();
            
            PlayerIndex = -1;
        }

        public void OnLocalPeerConnected(NetPeer peer)
        {
            CurrentPeer = peer;
            
            // Pode adicionar uma alteração de estado no client manager para loading enquanto espera o remote conectar
        }

        public void OnRemotePeerConnected(NetPeer peer)
        {
            RemotePeer = peer;
            
            PlayerIndex = CurrentPeer.RemoteId;
            
            //ChangeClientState?.Invoke(GameState.MainMenu);
        }

        public void OnLocalPeerDisconnected(NetPeer peer)
        {
            Log.PrintInfo($"Desconectado do servidor");
        }

        public void OnRemotePeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            Log.PrintInfo($"Desconectado do servidor: {disconnectInfo.Reason}");
        }
        
        public void SendData<T>(T packet, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableOrdered) where T : class, new()
        {
            if (CurrentPeer == null)
            {
                Log.PrintError("RemotePeer is null");
                return;
            }
            if (CurrentPeer.ConnectionState != ConnectionState.Connected)
            {
                Log.PrintError("RemotePeer is not connected");
                return;
            }
            if (packet == null)
            {
                Log.PrintError("Packet is null");
                return;
            }
            if (CurrentPeer == null)
            {
                Log.PrintError("CurrentPeer is null");
                return;
            }
            
            ClientPacketProcessor.Send(CurrentPeer, packet, deliveryMethod);
        }
    }
}