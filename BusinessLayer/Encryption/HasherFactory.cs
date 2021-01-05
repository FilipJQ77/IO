namespace BusinessLayer.Encryption
{
    class HasherFactory
    {
        public IHasher GetHasher()
        {
            return new Hasher();
        }
    }
}
