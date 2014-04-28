namespace Incoding.UnitTest
{
    using Incoding.Data;

    public class FakeEntityForNew:IEntity
    {
        public virtual object Id { get; private set; }
    }
}