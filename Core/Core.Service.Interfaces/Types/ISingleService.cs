
namespace Core.Service.Interfaces.Types;

public interface ISingleService : IService
{
    void Restart();
    void Update(long tick);
}