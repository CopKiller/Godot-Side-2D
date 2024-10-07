using Side2D.Models;
using Side2D.Models.Enum;
using Side2D.Models.Vectors;

namespace Side2D.Server.TempData.Temp.Interface;

public interface ITempPlayer : ITempData
{
    int Index { get; }
    int AccountId { get; }
    int SlotNumber { get; }
    ClientState ClientState { get; }
    ITempMove? Move { get; }
    ITempAttack? Attack { get; }
    ITempUpdatePlayerVar? UpdatePlayerVar { get; set; }
    void ChangeState(ClientState state, int slotNumber = 0);
    void CreateTempData(int slotNumber);
    void UpdateAccountData(AccountModel accountModel);
    void UpdatePlayerData(PlayerModel playerModel);
    int CountCharacters();
    bool ExistsCharacter(int slotNumber);
    PlayerModel? GetCharacter(int slotNumber);
    List<PlayerModel?> GetCharacters();
}