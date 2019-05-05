namespace MyWalletLib
{
    public interface ICacheProvider
    {
        bool Contains(string key);

        object Get(string key);

        void Put(string key, object value, int duration);
    }
}