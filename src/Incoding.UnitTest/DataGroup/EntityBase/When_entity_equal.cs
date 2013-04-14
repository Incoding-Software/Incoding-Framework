namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using Incoding.Data;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(IncEntityBase))]
    public class When_entity_equal : Context_entity_base
    {
        It should_be_not_equal_by_id = () =>
                                           {
                                               var entityBase = new FakeEntityBase(Guid.NewGuid());
                                               var entityBase2 = new FakeEntityBase(Guid.NewGuid());
                                               entityBase.Equals(entityBase2).ShouldBeFalse();
                                               (entityBase != entityBase2).ShouldBeTrue();
                                           };

        It should_be_equal_if_same_id = () =>
                                            {
                                                var sameId = Guid.NewGuid();
                                                var entityBase = new FakeEntityBase(sameId);
                                                var entityBase2 = new FakeEntityBase(sameId);
                                                entityBase.Equals(entityBase2).ShouldBeTrue();
                                                (entityBase == entityBase2).ShouldBeTrue();
                                            };

        It should_be_equal_if_both_entity_not_has_id = () =>
                                                           {
                                                               var entityBase = new FakeEntityBase();
                                                               var entityBase2 = new FakeEntityBase();
                                                               entityBase.Equals(entityBase2).ShouldBeTrue();
                                                           };
    }
}