using System;
using System.ComponentModel;
using System.IO;
using Godot;
using Infrastructure.Network.Packet.Server;
using Side2D.scripts.Controls.CustomLabels;
using Side2D.scripts.Network;

namespace Side2D.scripts.MainScripts.Game;

[Tool]
public partial class Map : Node2D
{
	[ExportCategory("Export")]
	
	[Export (PropertyHint.Dir , "res://Exports/Map/")]
	public string ExportDirectory { get; set; }
	
	[Export]
	public TileMapLayer[] TileMapLayer { get; set; }
	
	[Export]
	public bool ExportNow
	{
		get => false;
		set
		{
			if (value)
			{
				ExportMap();
			}
		}
	}
	
	private Players _players { get; set; }
		
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_players = GetNode<Players>(nameof(Players));
	}

	private void ExportMap()
	{
		if (ExportDirectory == string.Empty)
		{
			GD.Print("Export directory is null.");
			return;
		}
		
		if (!DirAccess.DirExistsAbsolute(ExportDirectory))
		{
			GD.Print("Export directory does not exist.");
			return;
		}
		
		GD.Print("Exporting map...");
	}
}