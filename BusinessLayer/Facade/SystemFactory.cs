namespace BusinessLayer.Facade
{
    public class SystemFactory
    {
        private ISystem _system;
        public ISystem System => _system ?? (_system = new System());
    }
}