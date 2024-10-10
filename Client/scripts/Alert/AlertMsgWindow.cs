using System;
using Godot;

namespace Side2D.scripts.Alert
{
    public partial class AlertMsgWindow : AcceptDialog
    {
        public delegate void OkCallBack();

        public OkCallBack OnOk;

        private Button okButton;

        private Button closeButton;

        public EventHandler OnCloseEvent;

        private string textOnly;

        public override void _Ready()
        {
            this.CloseRequested += () =>
            {
                this.Hide();
                OnOk?.Invoke();
                OnCloseEvent?.Invoke(this, null);
            };

            okButton = GetOkButton();

            okButton.Pressed += () =>
            {
                this.Hide();
                OnOk?.Invoke();
                OnCloseEvent?.Invoke(this, null);
            };

            GetLabel().HorizontalAlignment = HorizontalAlignment.Center;
            this.Size = new Vector2I(300, 100);
            this.TransparentBg = true;
            this.Title = "Alert";
            this.DialogAutowrap = true;
            this.AlwaysOnTop = true;
            this.Exclusive = false;

            GetLabel().Text = textOnly;
            this.Show();
            this.PopupCentered();
        }

        public void SetText(string text)
        {
            textOnly = text;
        }
    }
}
