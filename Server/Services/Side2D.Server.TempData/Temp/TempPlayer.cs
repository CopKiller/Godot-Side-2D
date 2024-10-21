using Core.Game.Interfaces.Services.Network.NetworkEventServices.TempData;
using Core.Game.Interfaces.TempData.Player;
using Core.Game.Models;
using Core.Game.Models.Enum;

namespace Side2D.Server.TempData.Temp;

public class TempPlayer(int index, Action<PlayerModel> saveOnDatabase, Action<int, ClientState> changeClientState) : ITempPlayer
{
    public int Index { get; private set; } = index;
    public int AccountId { get; private set; }
    public int SlotNumber { get; private set; } = 0; // SlotNumber = 0 é nenhum personagem selecionado.
    public ClientState ClientState { get; private set; }
    private List<PlayerModel?> PlayerModels { get; set; } = [];
    
    public void ChangeState(ClientState state, int slotNumber = 0)
    {
        ClientState = state;
        
        changeClientState(Index, state);

        switch (state)
        {
            case ClientState.None:
                Save();
                Dispose();
                break;
            case ClientState.Menu:
                Save();
                Dispose();
                break;
            case ClientState.Game:
                SlotNumber = slotNumber;
                break;
            case ClientState.Character:
                Save();
                SlotNumber = 0;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }

    private void Save()
    {
        if (ClientState != ClientState.Game) return;
        
        if (SlotNumber == 0) return;
            
        var player = PlayerModels.FirstOrDefault(x => x != null && x.SlotNumber == SlotNumber);

        if (player == null) return;

        saveOnDatabase(player);
    }
    
    public void Dispose()
    {
        AccountId = 0;
        SlotNumber = 0;
        PlayerModels.Clear();
    }
    
    public void AddAccountData(AccountModel accountModel)
    {
        AccountId = accountModel.Id;
        
        PlayerModels.Clear();
        PlayerModels.AddRange(accountModel.Players);
    }
    
    public void AddPlayerData(PlayerModel playerModel)
    {
        var player = PlayerModels.FirstOrDefault(x => x != null && x.Id == playerModel.Id);

        if (player != null) return;
        
        PlayerModels.Remove(player);
        PlayerModels.Add(playerModel);
    }
    
    public int CountCharacters()
    {
        return PlayerModels.Count;
    }
    
    public bool ExistsCharacter(int slotNumber)
    {
        return PlayerModels.Exists(x => x.SlotNumber == slotNumber);
    }
    
    public PlayerModel? GetCharacter(int slotNumber)
    {
        return PlayerModels.FirstOrDefault(x => x != null && x.SlotNumber == slotNumber);
    }
    
    public List<PlayerModel?> GetCharacters()
    {
        return PlayerModels;
    }

    public void Update(long currentTick)
    {
        if (ClientState != ClientState.Game) return;
        
    }
}
