using Side2D.Models;

namespace Side2D.Server.Database;

public class UserData
{
    public int Index { get; set; }
    public int AccountId { get; set; }
    public List<PlayerModel> PlayerModels { get; private set; } = [];
    
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
}