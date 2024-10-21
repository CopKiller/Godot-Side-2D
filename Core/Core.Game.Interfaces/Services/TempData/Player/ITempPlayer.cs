using Core.Game.Interfaces.TempData.Player.Attribute;
using Core.Game.Models;
using Core.Game.Models.Enum;

namespace Core.Game.Interfaces.TempData.Player;

public interface ITempPlayer : ITempData
{
    int Index { get; }
    int AccountId { get; }
    int SlotNumber { get; }
    ClientState ClientState { get; }
    
    void ChangeState(ClientState state, int slotNumber = 0);
    void AddAccountData(AccountModel accountModel);
    void AddPlayerData(PlayerModel playerModel);
    int CountCharacters();
    bool ExistsCharacter(int slotNumber);
    PlayerModel? GetCharacter(int slotNumber);
    List<PlayerModel?> GetCharacters();
}