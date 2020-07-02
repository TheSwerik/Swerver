namespace ServerLibrary.Server
{
    public abstract class GameLogic
    {
        protected abstract void Update(int delta);

        internal void InternalUpdate(int delta)
        {
            ThreadManager.UpdateMain();
            Update(delta);
        }
    }
}