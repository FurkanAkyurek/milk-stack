namespace Zenta.Core.Runtime.Interfaces
{
    public interface IGameStateListener
    {
        void OnGameStateChanged(GameState from, GameState to);
    }
}