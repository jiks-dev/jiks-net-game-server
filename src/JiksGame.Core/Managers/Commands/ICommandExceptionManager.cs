namespace JiksGame.Core.Managers
{
    public interface ICommandExceptionManager
    {
        void Add(Exception e);
        string[] GetAll();
        void Reset();
    }
}
