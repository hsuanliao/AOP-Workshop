namespace MyWalletLib
{
    public interface INotification
    {
        void Push(Role role, string message);
    }
}