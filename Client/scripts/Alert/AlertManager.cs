using System.Collections.Generic;
using Godot;
using Infrastructure.Network.Packet.Server;
using Side2D.scripts.Network;

namespace Side2D.scripts.Alert
{
    public partial class AlertManager : Node, IPacketHandler
    {
        private List<AlertMsgWindow> _alerts;

        public override void _Ready()
        {
            _alerts = [];
            this.Name = nameof(AlertManager);
            
            RegisterPacketHandlers();
        }

        public void AddAlert(string text)
        {
            var newAlert = new AlertMsgWindow();

            newAlert.OnCloseEvent += (sender, args) =>
            {
                _alerts.Remove(newAlert);
                newAlert.QueueFree();
            };

            newAlert.SetText(text);
            _alerts.Add(newAlert);
            CallDeferred(nameof(AddChild), newAlert);
        }
        
        public void AddAlert(string text, AlertMsgWindow.OkCallBack callBack)
        {
            var newAlert = new AlertMsgWindow();

            newAlert.OnCloseEvent += (sender, args) =>
            {
                _alerts.Remove(newAlert);
                newAlert.QueueFree();
            };
            
            newAlert.OnOk += callBack;

            newAlert.SetText(text);
            _alerts.Add(newAlert);

            CallDeferred(nameof(AddChild), newAlert);
        }
        
        private void AddChild(Node node)
        {
            base.AddChild(node);
        }

        public void RegisterPacketHandlers()
        {
            ClientPacketProcessor.RegisterPacket<SAlertMsg>(HandleAlertMsg);

            return;
            
            void HandleAlertMsg(SAlertMsg packet)
            {
                AddAlert(packet.Message);
            }
        }
        
        public override void _ExitTree()
        {
            base._ExitTree();
            
            ClientPacketProcessor.UnregisterPacket<SAlertMsg>();
            
            foreach (var alert in _alerts)
            {
                alert.QueueFree();
            }
            
            _alerts.Clear();
            
            _alerts = null;
            
            this.QueueFree();
        }
    }
}
