using Godot;
using Side2D.Host;
using Side2D.Models.Enum;
using Side2D.scripts;

public partial class MainMenuWindows : Node
{
	// Windows
	private winMenu _winMenu;
	private winLogin _winLogin;
	private winRegister _winRegister;
	private winCharacter _winCharacter;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_winMenu = GetNode<winMenu>(nameof(winMenu));
		_winLogin = GetNode<winLogin>(nameof(winLogin));
		_winRegister = GetNode<winRegister>(nameof(winRegister));
		_winCharacter = GetNode<winCharacter>(nameof(winCharacter));
	}
	
	public void ShowMenuWindow()
	{
		HideAll();
		_winMenu.ShowWindow();
	}
	
	public void ShowCharacterWindow()
	{
		HideAll();
		
		_winCharacter.ShowWindow(() =>
		{
			ApplicationHost.Instance.GetSingleton<ClientManager>().ChangeClientState(ClientState.Menu);
		});
	}
	
	public void ShowLoginWindow()
	{
		HideAll();
		
		_winLogin.ShowWindow(ShowMenuWindow);
	}
	
	public void ShowRegisterWindow()
	{
		HideAll();
		
		_winRegister.ShowWindow(ShowMenuWindow);
	}
	
	private void HideAll()
	{
		_winMenu.Hide();
		_winLogin.Hide();
		_winRegister.Hide();
		_winCharacter.Hide();
	}
}
