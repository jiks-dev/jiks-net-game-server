using System.Collections.Concurrent;

namespace JiksGame.Core.Managers
{
    public class CommandExceptionManager : ICommandExceptionManager
    {
        ConcurrentBag<string> _expceptions = new ConcurrentBag<string>();

        public void Add(Exception e) 
        {
            _expceptions.Add(e.ToString());
        }

        public string[] GetAll()
        {
            return _expceptions.ToArray();
        }

        public void Reset() 
        {
            _expceptions.Clear();
        }
    }
}
