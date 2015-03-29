namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(ConditionalData<>))]
    public class When_conditional_data
    {
        #region Fake classes

        class FakeModel
        {
            #region Properties

            public OfType Type { get; set; }

            public string Prop1 { get; set; }

            public bool PropBool { get; set; }

            #endregion

            #region Nested classes

            public class InnerFakeModel
            {
                #region Properties

                public int Prop1 { get; set; }

                #endregion
            }

            #endregion

            #region Enums

            public enum OfType
            {
                Value, 

                Value2
            }

            #endregion
        }

        #endregion

        #region Data

        It should_be_data_equal = () => new ConditionalData<FakeModel>(r => r.Prop1 == "123", true)
                                                .GetData()
                                                .ShouldEqualWeak(new
                                                                 {
                                                                         type = ConditionalOfType.Data.ToString(), 
                                                                         inverse = false, 
                                                                         property = "Prop1", 
                                                                         value = "123", 
                                                                         method = "equal", 
                                                                         and = true
                                                                 }, dsl => dsl.IncludeAllFields());

        It should_be_data_equal_to_enum = () => new ConditionalData<FakeModel>(r => r.Type == FakeModel.OfType.Value2, true)
                                                        .GetData()
                                                        .ShouldEqualWeak(new
                                                                         {
                                                                                 type = ConditionalOfType.Data.ToString(), 
                                                                                 inverse = false, 
                                                                                 property = "Type", 
                                                                                 value = "1", 
                                                                                 method = "equal", 
                                                                                 and = true
                                                                         }, dsl => dsl.IncludeAllFields());

        It should_be_data_with_inner_class = () => new ConditionalData<FakeModel.InnerFakeModel>(r => r.Prop1 == 123, true)
                                                           .GetData()
                                                           .ShouldEqualWeak(new
                                                                            {
                                                                                    type = ConditionalOfType.Data.ToString(), 
                                                                                    inverse = false, 
                                                                                    property = "Prop1", 
                                                                                    value = "123", 
                                                                                    method = "equal", 
                                                                                    and = true
                                                                            }, dsl => dsl.IncludeAllFields());

        It should_be_data_equal_model = () =>
                                        {
                                            var model = new FakeModel { Prop1 = "123" };
                                            new ConditionalData<FakeModel>(r => r.Prop1 == model.Prop1, true)
                                                    .GetData()
                                                    .ShouldEqualWeak(new
                                                                     {
                                                                             type = ConditionalOfType.Data.ToString(), 
                                                                             inverse = false, 
                                                                             property = "Prop1", 
                                                                             value = "123", 
                                                                             method = "equal", 
                                                                             and = true
                                                                     }, dsl => dsl.IncludeAllFields());
                                        };

        It should_be_data_equal_bool = () => new ConditionalData<FakeModel>(r => r.PropBool, true)
                                                     .GetData()
                                                     .ShouldEqualWeak(new
                                                                      {
                                                                              type = ConditionalOfType.Data.ToString(), 
                                                                              inverse = false, 
                                                                              property = "PropBool", 
                                                                              value = "True", 
                                                                              method = "equal", 
                                                                              and = true
                                                                      }, dsl => dsl.IncludeAllFields());

        It should_be_not_data_equal_bool = () => new ConditionalData<FakeModel>(r => !r.PropBool, true)
                                                         .GetData()
                                                         .ShouldEqualWeak(new
                                                                          {
                                                                                  type = ConditionalOfType.Data.ToString(), 
                                                                                  inverse = true, 
                                                                                  property = "PropBool", 
                                                                                  value = "True", 
                                                                                  method = "equal", 
                                                                                  and = true
                                                                          }, dsl => dsl.IncludeAllFields());

        It should_be_data_equal_to_context = () => new ConditionalData<FakeModel>(r => r.Prop1 == Selector.Jquery.Self(), true)
                                                           .GetData()
                                                           .ShouldEqualWeak(new
                                                                            {
                                                                                    type = ConditionalOfType.Data.ToString(), 
                                                                                    inverse = false, 
                                                                                    property = "Prop1", 
                                                                                    value = "$(this.self)", 
                                                                                    method = "equal", 
                                                                                    and = true
                                                                            }, dsl => dsl.IncludeAllFields());

        It should_be_data_bool_equal_with_selector = () => new ConditionalData<FakeModel>(r => r.PropBool == Selector.Jquery.Self(), true)
                                                                   .GetData()
                                                                   .ShouldEqualWeak(new
                                                                                    {
                                                                                            type = ConditionalOfType.Data.ToString(), 
                                                                                            inverse = false, 
                                                                                            property = "PropBool", 
                                                                                            value = "$(this.self)", 
                                                                                            method = "equal", 
                                                                                            and = true
                                                                                    }, dsl => dsl.IncludeAllFields());

        It should_be_data_not_equal = () => new ConditionalData<FakeModel>(r => r.Prop1 != "123", true)
                                                    .GetData()
                                                    .ShouldEqualWeak(new
                                                                     {
                                                                             type = ConditionalOfType.Data.ToString(), 
                                                                             inverse = false, 
                                                                             property = "Prop1", 
                                                                             value = "123", 
                                                                             method = "notequal", 
                                                                             and = true
                                                                     }, dsl => dsl.IncludeAllFields());

        It should_be_data_right = () => new ConditionalData<FakeModel>(r => "123" != r.Prop1, true)
                                                .GetData()
                                                .ShouldEqualWeak(new
                                                                 {
                                                                         type = ConditionalOfType.Data.ToString(), 
                                                                         inverse = false, 
                                                                         property = "Prop1", 
                                                                         value = "123", 
                                                                         method = "notequal", 
                                                                         and = true
                                                                 }, dsl => dsl.IncludeAllFields());

        It should_be_data_equal_with_method = () => new ConditionalData<FakeModel>(r => r.Prop1 == 123.ToString(), true)
                                                            .GetData()
                                                            .ShouldEqualWeak(new
                                                                             {
                                                                                     type = ConditionalOfType.Data.ToString(), 
                                                                                     inverse = false, 
                                                                                     property = "Prop1", 
                                                                                     value = "123", 
                                                                                     method = "equal", 
                                                                                     and = true
                                                                             }, dsl => dsl.IncludeAllFields());

        It should_be_data_contains = () => new ConditionalData<FakeModel>(r => r.Prop1.Contains("123"), true)
                                                   .GetData()
                                                   .ShouldEqualWeak(new
                                                                    {
                                                                            type = ConditionalOfType.Data.ToString(), 
                                                                            inverse = false, 
                                                                            property = "Prop1", 
                                                                            value = "123", 
                                                                            method = "contains", 
                                                                            and = true
                                                                    }, dsl => dsl.IncludeAllFields());

        It should_be_data_is_empty = () => new ConditionalData<FakeModel>(r => r.IsEmpty(), true)
                                                   .GetData()
                                                   .ShouldEqualWeak(new
                                                                    {
                                                                            type = ConditionalOfType.Data.ToString(), 
                                                                            inverse = false, 
                                                                            property = string.Empty, 
                                                                            value = string.Empty, 
                                                                            method = "isempty", 
                                                                            and = true
                                                                    }, dsl => dsl.IncludeAllFields());

        It should_be_data_not_is_empty = () => new ConditionalData<FakeModel>(r => !r.IsEmpty(), true)
                                                       .GetData()
                                                       .ShouldEqualWeak(new
                                                                        {
                                                                                type = ConditionalOfType.Data.ToString(), 
                                                                                inverse = true, 
                                                                                property = string.Empty, 
                                                                                value = string.Empty, 
                                                                                method = "isempty", 
                                                                                and = true
                                                                        }, dsl => dsl.IncludeAllFields());

        It should_be_data_without_properties = () => new ConditionalData<string>(r => r == "aws", true)
                                                             .GetData()
                                                             .ShouldEqualWeak(new
                                                                              {
                                                                                      type = ConditionalOfType.Data.ToString(), 
                                                                                      inverse = false, 
                                                                                      property = string.Empty, 
                                                                                      value = "aws", 
                                                                                      method = "equal", 
                                                                                      and = true
                                                                              }, dsl => dsl.IncludeAllFields());

        It should_be_data_is_empty_with_property = () => new ConditionalData<FakeModel>(r => r.Prop1.IsEmpty(), true)
                                                                 .GetData()
                                                                 .ShouldEqualWeak(new
                                                                                  {
                                                                                          type = ConditionalOfType.Data.ToString(), 
                                                                                          inverse = false, 
                                                                                          property = "Prop1", 
                                                                                          value = string.Empty, 
                                                                                          method = "isempty", 
                                                                                          and = true
                                                                                  }, dsl => dsl.IncludeAllFields());

        It should_be_data_is_id = () => new ConditionalDataIsId<FakeModel>(r => r.Prop1, true)
                                                .GetData()
                                                .ShouldEqualWeak(new
                                                                 {
                                                                         type = ConditionalOfType.DataIsId.ToString(), 
                                                                         inverse = false, 
                                                                         property = "Prop1", 
                                                                         and = true
                                                                 }, dsl => dsl.IncludeAllFields());

        #endregion
    }
}