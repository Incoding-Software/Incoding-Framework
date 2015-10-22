namespace Incoding.UnitTest.ExtensionsGroup
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Management.Instrumentation;
    using Incoding.Block;
    using Incoding.Block.Logging;
    using Incoding.CQRS;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Incoding.UnitTest.Block;
    using Machine.Specifications;
    using Machine.Specifications.Annotations;

    #endregion

    [Subject(typeof(ReflectionExtensions))]
    public class When_reflection_extensions
    {
        #region Fake classes

        class FakeQuery : QueryBase<string>
        {
            protected override string ExecuteResult()
            {
                throw new NotImplementedException();
            }
        }

        interface IGenericType { }

        class GenericType1 : IGenericType { }

        class GenericType2 : IGenericType { }

        class GenericType3 : IGenericType { }

        // ReSharper disable UnusedTypeParameter
        interface IGenericHandler<TGenericType> where TGenericType : IGenericType { }

        // ReSharper restore UnusedTypeParameter
        class GenericSubscriber : IGenericHandler<GenericType1>, IGenericHandler<GenericType2> { }

        class FakeSetValueClass
        {
            #region Static Fields

            static string staticValue = "staticValue";

            #endregion

            #region Fields

            readonly string readOnlyPrivateField;

            string privateField;
            int privateIntField;
            int? privateIntNullableField;

            private object classField;

            #endregion

            internal class FakeInnerClass
            {
                public string Val { get; set; }
            }

            #region Properties

            [UsedImplicitly]
            public string PrivateField { get; set; }
            

            // ReSharper disable MemberCanBePrivate.Local
            protected string ProtectedProperty { get; set; }

            #endregion

            // ReSharper restore MemberCanBePrivate.Local
            #region Api Methods

            public string GetProtected()
            {
                return ProtectedProperty;
            }

            public string GetPrivateFiled()
            {
                return this.privateField;
            }

            public string GetReadOnlyPrivateFiled()
            {
                return this.readOnlyPrivateField;
            }

            public FakeSetValueClass SetPrivateField(string value)
            {
                this.privateField = value;
                return this;
            }

            public FakeSetValueClass SetPrivateIntField(int value)
            {
                this.privateIntField = value;
                return this;
            }

            public FakeSetValueClass SetPrivateIntNullableField(int? value)
            {
                this.privateIntNullableField = value;
                return this;
            }

            public FakeSetValueClass SetPrivateClassNullableField(FakeInnerClass value)
            {
                this.classField = value;
                return this;
            }

            #endregion
        }

        #endregion

        It should_be_primitive_int = () => typeof(int).IsPrimitive().ShouldBeTrue();

        It should_be_primitive_nullable_int = () => typeof(int?).IsPrimitive().ShouldBeTrue();

        It should_be_primitive_string = () => typeof(string).IsPrimitive().ShouldBeTrue();

        It should_be_not_primitive = () => typeof(IGenericType).IsPrimitive().ShouldBeFalse();

        It should_be_not_primitive_generic = () => typeof(IGenericHandler<GenericType2>).IsPrimitive().ShouldBeFalse();

        It should_be_implement_class = () => typeof(ArgumentException).IsImplement<Exception>().ShouldBeTrue();

        It should_be_implement_with_generic = () => typeof(List<int>).IsImplement<IList<int>>();

        It should_be_implement_interface = () => typeof(ClipboardLogger).IsImplement<ILogger>().ShouldBeTrue();

        It should_not_be_implement_interface = () => typeof(ClipboardLogger).IsImplement<ICloneable>().ShouldBeFalse();

        It should_be_implement_query = () => typeof(FakeQuery).IsImplement(typeof(QueryBase<string>)).ShouldBeTrue();

        It should_be_implement_deep = () => typeof(ArgumentException).IsImplement<object>().ShouldBeTrue();

        It should_be_implement_generic_1 = () => typeof(GenericSubscriber).IsImplement(typeof(IGenericHandler<GenericType1>)).ShouldBeTrue();

        It should_be_implement_generic_2 = () => typeof(GenericSubscriber).IsImplement(typeof(IGenericHandler<GenericType2>)).ShouldBeTrue();

        It should_not_be_implement_generic_3 = () => typeof(GenericSubscriber).IsImplement(typeof(IGenericHandler<GenericType3>)).ShouldBeFalse();

        It should_be_implement_same_type = () => typeof(ArgumentException).IsImplement<ArgumentException>().ShouldBeTrue();

        It should_be_first_attribute = () => typeof(FakeCacheKey).FirstOrDefaultAttribute<FakeAttribute>().ShouldNotBeNull();

        It should_be_has_attribute = () => typeof(FakeCacheKey).HasAttribute<FakeAttribute>().ShouldBeTrue();

        It should_be_first_attributes_with_default = () => typeof(FakeCacheKey).FirstOrDefaultAttribute<CLSCompliantAttribute>().ShouldBeNull();

        It should_be_get_member_name = () =>
                                       {
                                           Expression<Func<ArgumentException, object>> expression = exception => exception.Message;
                                           expression.GetMemberName().ShouldEqual("Message");
                                       };

        It should_be_get_member_name_from_convert = () =>
                                                    {
                                                        Expression<Func<ArgumentException, object>> expression = exception => exception.Data;
                                                        expression.GetMemberName().ShouldEqual("Data");
                                                    };

        It should_be_get_member_name_complex_object = () =>
                                                      {
                                                          Expression<Func<ArgumentException, object>> expression = exception => exception.InnerException.Data;
                                                          expression.GetMemberName().ShouldEqual("InnerException.Data");
                                                      };

        It should_be_get_member_name_complex_object_one_char = () =>
                                                               {
                                                                   Expression<Func<ArgumentException, object>> expression = r => r.InnerException.Data;
                                                                   expression.GetMemberName().ShouldEqual("InnerException.Data");
                                                               };

        It should_be_get_member_name_with_complex_field = () =>
                                                          {
                                                              Expression<Func<ArgumentException, object>> expression = exception => exception.InnerException.Message;
                                                              expression.GetMemberName().ShouldEqual("InnerException.Message");
                                                          };

        It should_be_set_value_to_field_with_camelcase = () => new FakeSetValueClass()
                .SetValue("privateField", Pleasure.Generator.TheSameString())
                .GetPrivateFiled()
                .ShouldEqual(Pleasure.Generator.TheSameString());

        It should_be_set_value_to_readonly_field = () => new FakeSetValueClass()
                .SetValue("readOnlyPrivateField", Pleasure.Generator.TheSameString())
                .GetReadOnlyPrivateFiled()
                .ShouldEqual(Pleasure.Generator.TheSameString());

        It should_be_set_value_to_property = () => new FakeSetValueClass()
                .SetValue("ProtectedProperty", Pleasure.Generator.TheSameString())
                .GetProtected().ShouldEqual(Pleasure.Generator.TheSameString());

        It should_be_set_field_with_wrong_name = () => Catch.Exception(() => new FakeSetValueClass().SetValue("NotFoundProperty", "Value"))
                .ShouldBeOfType<InstanceNotFoundException>();

        It should_be_try_get_value_with_wrong_name = () => new FakeSetValueClass()
                .TryGetValue("NotFoundField")
                .ShouldBeNull();

        It should_be_try_get_value = () => new FakeSetValueClass()
                .SetPrivateField(Pleasure.Generator.TheSameString())
                .TryGetValue("privateField")
                .ShouldEqual(Pleasure.Generator.TheSameString());

        It should_be_try_get_value_static = () => new FakeSetValueClass()
                .TryGetValue("staticValue")
                .ShouldEqual("staticValue");

        It should_be_try_get_value_int_private = () => new FakeSetValueClass().SetPrivateIntField(5)
                .TryGetValue<int>("privateIntField")
                .ShouldEqual(5);

        It should_be_try_get_value_int_nullable_private = () => new FakeSetValueClass().SetPrivateIntNullableField(null)
                .TryGetValue<int?>("privateIntNullableField")
                .ShouldBeNull();

        It should_be_try_get_value_int_nullable_val_private = () => new FakeSetValueClass().SetPrivateIntNullableField(6)
                .TryGetValue<int?>("privateIntNullableField")
                .ShouldEqual(6);

        It should_be_try_get_value_class_nullable_private = () => new FakeSetValueClass().SetPrivateClassNullableField(null)
                .TryGetValue<FakeSetValueClass.FakeInnerClass>("classField")
                .ShouldBeNull();

        It should_be_try_get_value_class_nullable_val_private = () => new FakeSetValueClass().SetPrivateClassNullableField(new FakeSetValueClass.FakeInnerClass() { Val = "asd"})
                .TryGetValue<FakeSetValueClass>("classField")
                .ShouldEqualWeak(new FakeSetValueClass.FakeInnerClass() { Val = "asd" });

        It should_be_is_typical_type = () =>
                                       {
                                           DateTime.Now.GetType().IsTypicalType().ShouldBeTrue();
                                           5.GetType().IsTypicalType().ShouldBeTrue();
                                           "5".GetType().IsTypicalType().ShouldBeTrue();
                                           typeof(DayOfWeek).IsTypicalType().ShouldBeTrue();
                                           new FakeSetValueClass().GetType().IsTypicalType().ShouldBeFalse();
                                       };
    }
}