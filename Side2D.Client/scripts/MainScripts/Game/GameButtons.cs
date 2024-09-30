using Godot;
using System;
using Side2D.scripts.MainScripts.Game;

public partial class GameButtons : Control
{
	private TextureButton _btnConfig;
	public override void _Ready()
	{
		_btnConfig = GetNode<TextureButton>("%btnConfig");
		_btnConfig.Connect(BaseButton.SignalName.Pressed, Callable.From(() => GetParent().GetParent<Game>().ShowGameOptions()));
	}
}
