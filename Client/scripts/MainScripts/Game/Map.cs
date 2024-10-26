using Godot;

namespace Side2D.scripts.MainScripts.Game;

public partial class Map : Node2D
{
	private Players _players { get; set; }
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_players = GetNode<Players>(nameof(Players));
	}
}