using Godot;
using Side2D.Models.Enum;
using Side2D.scripts.Host;

namespace Side2D.scripts;

public partial class Root : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var clientManager = ApplicationHost.Instance.GetSingleton<ClientManager>();
		clientManager.Start();
		
		clientManager.ChangeClientState(ClientState.Menu);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}