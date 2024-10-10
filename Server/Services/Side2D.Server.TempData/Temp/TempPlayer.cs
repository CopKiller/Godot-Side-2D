using Core.Game.Models;
using Core.Game.Models.Enum;
using Core.Game.Models.Vectors;
using Side2D.Server.TempData.Temp.Interface;
using Side2D.Server.TempData.Temp.Player;

namespace Side2D.Server.TempData.Temp;

public class TempPlayer(int index) : ITempPlayer
{
    public int Index { get; private set; } = index;
    public int AccountId { get; private set; }
    public int SlotNumber { get; private set; } = 0; // SlotNumber = 0 é nenhum personagem selecionado.
    public ClientState ClientState { get; private set; }
    private List<PlayerModel?> PlayerModels { get; set; } = [];
    
    public ITempAttack? Attack { get; private set; }
    public ITempMove? Move { get; private set; }
    public ITempUpdatePlayerVar? UpdatePlayerVar { get; set; }


    public void ChangeState(ClientState state, int slotNumber = 0)
    {
        ClientState = state;
        
        if (slotNumber > 0)
        {
            SlotNumber = slotNumber;
        }

        switch (state)
        {
            case ClientState.None:
                Dispose();
                break;
            case ClientState.Menu:
                Dispose();
                break;
            case ClientState.Game:
                CreateTempData(SlotNumber);
                break;
            case ClientState.Character:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }
    
    private void CreateTempData(int slotNum)
    {
        SlotNumber = slotNum;
        var lastPosition = PlayerModels.FirstOrDefault(x => x.SlotNumber == SlotNumber)?.Position ?? new Vector2C();
        Move = new TempMove(lastPosition);
        Attack = new TempAttack();
        var vitals = PlayerModels.FirstOrDefault(x => x.SlotNumber == SlotNumber)?.Vitals;
        UpdatePlayerVar = new TempUpdatePlayerVar(vitals);
    }
    
    public void Dispose()
    {
        AccountId = 0;
        SlotNumber = 0;
        PlayerModels.Clear();
        Move?.Dispose();
        Attack?.Dispose();
        UpdatePlayerVar?.Dispose();
        Move = null;
        Attack = null;
    }
    
    public void UpdateAccountData(AccountModel accountModel)
    {
        AccountId = accountModel.Id;
        
        PlayerModels.Clear();
        PlayerModels.AddRange(accountModel.Players);
    }
    
    public void UpdatePlayerData(PlayerModel playerModel)
    {
        var player = PlayerModels.FirstOrDefault(x => x.Id == playerModel.Id);
        
        if (player == null)
        {
            PlayerModels.Add(playerModel);
        }
        else
        {
            player = playerModel;
        }
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
        Move?.Update(currentTick);
        Attack?.Update(currentTick);
        UpdatePlayerVar?.Update(currentTick);
    }
}
