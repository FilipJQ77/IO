namespace BusinessLayer.Encryption
{
    interface IHasher
    {
        string Hash(string password);
        bool Check(string hash, string password);
    }
}
