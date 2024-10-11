using Core.Game.Models.Player;
using Godot;
using Infrastructure.Network.Packet.Server;
using Side2D.scripts.Controls;
using Side2D.scripts.Network;

public partial class GameBars : Control, IPacketHandler
{
	private BaseTextureProgressBar? _hpBar;
	private BaseTextureProgressBar? _mpBar;

	private Vitals? _vitals;

	public GameBars()
	{
		Name = nameof(GameBars);
		RegisterPacketHandlers();
	}

	public override void _Ready()
	{
		_hpBar = GetNode<BaseTextureProgressBar>("%HpBar");
		_mpBar = GetNode<BaseTextureProgressBar>("%MpBar");
		
		Update();
	}

	public void UpdateBars(Vitals vitals)
	{
		_vitals = vitals;

		Update();
	}

	private void Update()
	{
		if (_hpBar == null || _mpBar == null) return;
		
		_hpBar.SetOptions(true,true);
		_mpBar.SetOptions(true,true);
		
		if (_vitals == null) return;
		
		_hpBar.MaxValue = _vitals.MaxHealth;
		_hpBar.CurrentValue = _vitals.Health;
		
		_mpBar.MaxValue = _vitals.MaxMana;
		_mpBar.CurrentValue = _vitals.Mana;
	}

public void RegisterPacketHandlers()
	{
		ClientPacketProcessor.RegisterPacket<SPlayerUpdateVitals>(HandleVitals);
	}

	public override void _ExitTree()
	{
		ClientPacketProcessor.UnregisterPacket<SPlayerUpdateVitals>();
		base._ExitTree();
	}

	private void HandleVitals(SPlayerUpdateVitals obj)
	{
		if (obj.Vitals == null) return;
		
		GD.Print("Vitals received");
		UpdateBars(obj.Vitals);
	}
}
