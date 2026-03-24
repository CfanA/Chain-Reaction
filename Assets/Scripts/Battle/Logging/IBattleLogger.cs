using ChainReaction.Battle.Contexts;

namespace ChainReaction.Battle.Logging
{
    public interface IBattleLogger
    {
        void Log(string message);
        void LogWarning(string message);
        void LogEvent(BattleEventContext eventContext);
    }
}
