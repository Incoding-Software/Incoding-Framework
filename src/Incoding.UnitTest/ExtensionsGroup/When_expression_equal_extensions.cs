namespace Incoding.UnitTest.ExtensionsGroup
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;
    using Incoding.Data;
    using Incoding.ExpressionCombining;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(ExpressionEqualExtensions))]
    public class When_expression_equal_extensions
    {
        #region Fake classes

        class FakeParameter
        {
            #region Properties

            public string AwsValue { get; set; }

            #endregion
        }

        class FakeEntity
        {
            #region Properties

            public string Id { get; set; }

            public float Cost { get; set; }

            public FakeEntity Entity { get; set; }

            #endregion
        }

        #endregion

        #region Establish value

        static string aws;

        static FakeParameter fakeParameter;

        #endregion

        #region Operation

        It should_be_is_expression_equal_constant = () =>
                                                        {
                                                            Expression<Func<IEntity, object>> left = entity => entity.Id == "aws";
                                                            Expression<Func<IEntity, object>> right = entity => entity.Id == "aws";
                                                            left.IsExpressionEqual(right).ShouldBeTrue();
                                                        };

        It should_be_is_expression_equal_constant_with_to_upper = () =>
                                                                      {
                                                                          Expression<Func<IEntity, object>> left = entity => entity.Id.ToString().ToUpper() == "aws".ToUpper();
                                                                          Expression<Func<IEntity, object>> right = entity => entity.Id.ToString().ToUpper() == "aws".ToUpper();
                                                                          left.IsExpressionEqual(right).ShouldBeTrue();
                                                                      };

        It should_be_is_expression_not_equal_constant = () =>
                                                            {
                                                                Expression<Func<IEntity, object>> left = entity => entity.Id != "aws";
                                                                Expression<Func<IEntity, object>> right = entity => entity.Id == "aws";
                                                                left.IsExpressionEqual(right).ShouldBeFalse();
                                                            };

        It should_be_is_expression_not_equal = () =>
                                                   {
                                                       Expression<Func<IEntity, object>> left = entity => entity.Id != "aws";
                                                       Expression<Func<IEntity, object>> right = entity => entity.Id != "aws";
                                                       left.IsExpressionEqual(right).ShouldBeTrue();
                                                   };

        It should_be_is_expression_and_also = () =>
                                                  {
                                                      Expression<Func<IEntity, bool>> expression = entity => entity.Id == "aws";
                                                      var left = expression.AndAlso(entity => entity.Id == "aws");
                                                      var right = expression.AndAlso(entity => entity.Id == "aws");
                                                      left.IsExpressionEqual(right).ShouldBeTrue();
                                                  };

        It should_be_is_expression_or_else = () =>
                                                 {
                                                     Expression<Func<IEntity, bool>> expression = entity => entity.Id == "aws";
                                                     var left = expression.OrElse(entity => entity.Id == "aws");
                                                     var right = expression.OrElse(entity => entity.Id == "aws");
                                                     left.IsExpressionEqual(right).ShouldBeTrue();
                                                 };

        It should_be_is_expression_not_and = () =>
                                                 {
                                                     Expression<Func<IEntity, object>> left = entity => entity.Id == "aws" && entity.Id == "swa";
                                                     Expression<Func<IEntity, object>> right = entity => entity.Id == "aws" && entity.Id == "aws";
                                                     left.IsExpressionEqual(right).ShouldBeFalse();
                                                 };

        It should_be_is_expression_and_vs_or = () =>
                                                   {
                                                       Expression<Func<IEntity, object>> left = entity => entity.Id == "aws" && entity.Id == "swa";
                                                       Expression<Func<IEntity, object>> right = entity => entity.Id == "aws" || entity.Id == "swa";
                                                       left.IsExpressionEqual(right).ShouldBeFalse();
                                                   };

        It should_be_is_expression_or = () =>
                                            {
                                                Expression<Func<IEntity, object>> expression = entity => entity.Id == "aws" || entity.Id == "swa";
                                                var left = expression;
                                                var right = expression;
                                                left.IsExpressionEqual(right).ShouldBeTrue();
                                            };

        It should_be_is_expression_not_or = () =>
                                                {
                                                    Expression<Func<IEntity, object>> left = entity => entity.Id == "aws" || entity.Id == "swa";
                                                    Expression<Func<IEntity, object>> right = entity => entity.Id == "aws" || entity.Id == "aws";
                                                    left.IsExpressionEqual(right).ShouldBeFalse();
                                                };

        It should_be_is_expression_not_equal_method = () =>
                                                          {
                                                              Expression<Func<IEntity, object>> left = entity => entity.Id != Pleasure.Generator.TheSameString();
                                                              Expression<Func<IEntity, object>> right = entity => entity.Id == Pleasure.Generator.TheSameString();
                                                              left.IsExpressionEqual(right).ShouldBeFalse();
                                                          };

        It should_be_is_expression_less_operation = () =>
                                                        {
                                                            Expression<Func<FakeEntity, object>> left = entity => entity.Cost < 100;
                                                            Expression<Func<FakeEntity, object>> right = entity => entity.Cost < 100;
                                                            left.IsExpressionEqual(right).ShouldBeTrue();
                                                        };

        It should_be_is_expression_not_less = () =>
                                                  {
                                                      Expression<Func<FakeEntity, object>> left = entity => entity.Cost < 100;
                                                      Expression<Func<FakeEntity, object>> right = entity => entity.Cost < 50;
                                                      left.IsExpressionEqual(right).ShouldBeFalse();
                                                  };

        It should_be_is_expression_less_or_equal = () =>
                                                       {
                                                           Expression<Func<FakeEntity, object>> left = entity => entity.Cost <= 100;
                                                           Expression<Func<FakeEntity, object>> right = entity => entity.Cost <= 100;
                                                           left.IsExpressionEqual(right).ShouldBeTrue();
                                                       };

        It should_be_is_expression_not_less_or_equal = () =>
                                                           {
                                                               Expression<Func<FakeEntity, object>> left = entity => entity.Cost <= 100;
                                                               Expression<Func<FakeEntity, object>> right = entity => entity.Cost <= 50;
                                                               left.IsExpressionEqual(right).ShouldBeFalse();
                                                           };

        It should_be_is_expression_not_greater = () =>
                                                     {
                                                         Expression<Func<FakeEntity, object>> left = entity => entity.Cost > 100;
                                                         Expression<Func<FakeEntity, object>> right = entity => entity.Cost > 50;
                                                         left.IsExpressionEqual(right).ShouldBeFalse();
                                                     };

        It should_be_is_expression_greater_or_equal = () =>
                                                          {
                                                              Expression<Func<FakeEntity, object>> left = entity => entity.Cost >= 100;
                                                              Expression<Func<FakeEntity, object>> right = entity => entity.Cost >= 100;
                                                              left.IsExpressionEqual(right).ShouldBeTrue();
                                                          };

        It should_be_is_expression_less = () =>
                                              {
                                                  Expression<Func<IEntity, object>> left = entity => entity.Id.ToString().Length < 5;
                                                  Expression<Func<IEntity, object>> right = entity => entity.Id.ToString().Length > 5;
                                                  left.IsExpressionEqual(right).ShouldBeFalse();
                                              };

        #endregion

        It should_be_is_expression_return_bool = () =>
                                                     {
                                                         Expression<Func<IEntity, object>> left = entity => true;
                                                         Expression<Func<IEntity, object>> right = entity => false;
                                                         left.IsExpressionEqual(right).ShouldBeFalse();
                                                     };

        It should_be_is_expression_with_null = () =>
                                                   {
                                                       Expression<Func<IEntity, object>> left = null;
                                                       Expression<Func<IEntity, object>> right = entity => false;
                                                       left.IsExpressionEqual(right).ShouldBeFalse();
                                                   };

        It should_be_is_expression_with_func_by_argument_count = () =>
                                                                     {
                                                                         Func<IEntity, int, object> func = (entity, arg) => entity.Id == Pleasure.Generator.TheSameString();
                                                                         Expression<Func<IEntity, object>> left = s => func(s, 2);

                                                                         Func<IEntity, object> func2 = entity => entity.Id == "Not same id";
                                                                         Expression<Func<IEntity, object>> right = s => func2(s);
                                                                         left.IsExpressionEqual(right).ShouldBeFalse();
                                                                     };

        It should_be_is_expression_with_func_by_argument_value = () =>
                                                                     {
                                                                         Func<int, object> func = entity => true;
                                                                         Expression<Func<IEntity, object>> left = s => func(2);
                                                                         Expression<Func<IEntity, object>> right = s => func(5);
                                                                         left.IsExpressionEqual(right).ShouldBeFalse();
                                                                     };

        #region Member class and complexy constant.

        It should_be_is_expression_not_equal_constant_as_member_class = () =>
                                                                            {
                                                                                aws = "aws";
                                                                                Expression<Func<IEntity, object>> left = entity => entity.Id == aws;
                                                                                Expression<Func<IEntity, object>> right = entity => entity.Id == "saw";
                                                                                left.IsExpressionEqual(right).ShouldBeFalse();
                                                                            };

        It should_be_is_expression_not_equal_constant_as_member_class_with_to_upper = () =>
                                                                                          {
                                                                                              aws = "aws";
                                                                                              Expression<Func<IEntity, object>> left = entity => entity.Id == aws.ToUpper();
                                                                                              Expression<Func<IEntity, object>> right = entity => entity.Id == "swa".ToUpper();
                                                                                              left.IsExpressionEqual(right).ShouldBeFalse();
                                                                                          };

        It should_be_is_expression_equal_constant_as_member_class = () =>
                                                                        {
                                                                            aws = "aws";
                                                                            Expression<Func<IEntity, object>> left = entity => entity.Id == aws;
                                                                            Expression<Func<IEntity, object>> right = entity => entity.Id == "aws";
                                                                            left.IsExpressionEqual(right).ShouldBeTrue();
                                                                        };

        It should_be_is_expression_equal_class_as_member_class = () =>
                                                                     {
                                                                         fakeParameter = new FakeParameter { AwsValue = "aws" };
                                                                         Expression<Func<IEntity, object>> left = entity => entity.Id == fakeParameter.AwsValue;
                                                                         Expression<Func<IEntity, object>> right = entity => entity.Id == "aws";
                                                                         left.IsExpressionEqual(right).ShouldBeTrue();
                                                                     };

        It should_be_is_expression_equal_class_as_member_class_to_lower = () =>
                                                                              {
                                                                                  fakeParameter = new FakeParameter { AwsValue = "aws" };
                                                                                  Expression<Func<IEntity, object>> left = entity => entity.Id == fakeParameter.AwsValue.ToLower();
                                                                                  Expression<Func<IEntity, object>> right = entity => entity.Id == "aws".ToLower();
                                                                                  left.IsExpressionEqual(right).ShouldBeTrue();
                                                                              };

        It should_be_is_expression_equal_constant_as_class = () =>
                                                                 {
                                                                     var localParameter = new FakeParameter { AwsValue = "aws" };
                                                                     Expression<Func<IEntity, object>> left = entity => entity.Id == localParameter.AwsValue;
                                                                     Expression<Func<IEntity, object>> right = entity => entity.Id == "aws";
                                                                     left.IsExpressionEqual(right).ShouldBeTrue();
                                                                 };

        It should_be_is_expression_equal_constant_as_class_with_to_lower_invariant = () =>
                                                                                         {
                                                                                             var localParameter = new FakeParameter { AwsValue = "aws" };
                                                                                             Expression<Func<IEntity, object>> left = entity => entity.Id == localParameter.AwsValue.ToLowerInvariant();
                                                                                             Expression<Func<IEntity, object>> right = entity => entity.Id == "aws".ToLowerInvariant();
                                                                                             left.IsExpressionEqual(right).ShouldBeTrue();
                                                                                         };

        It should_be_is_expression_with_constant_as_local_variable = () =>
                                                                         {
                                                                             // ReSharper disable ConvertToConstant.Local
                                                                             string local = "aws";

                                                                             // ReSharper restore ConvertToConstant.Local
                                                                             Expression<Func<IEntity, object>> left = entity => entity.Id == local;
                                                                             Expression<Func<IEntity, object>> right = entity => entity.Id == "aws";
                                                                             left.IsExpressionEqual(right).ShouldBeTrue();
                                                                         };

        It should_be_is_expression_with_constant_as_local_const = () =>
                                                                      {
                                                                          const string localConst = "aws";
                                                                          Expression<Func<IEntity, object>> left = entity => entity.Id == localConst;
                                                                          Expression<Func<IEntity, object>> right = entity => entity.Id == "aws";
                                                                          left.IsExpressionEqual(right).ShouldBeTrue();
                                                                      };

        #endregion

        #region naming

        It should_be_is_expression_with_different_name = () =>
                                                             {
                                                                 Expression<Func<IEntity, object>> left = s => s.Id == Pleasure.Generator.TheSameString();
                                                                 Expression<Func<IEntity, object>> right = entity => entity.Id == Pleasure.Generator.TheSameString();
                                                                 left.IsExpressionEqual(right).ShouldBeTrue();
                                                             };

        It should_be_is_expression_with_deep_complexly_name = () =>
                                                                  {
                                                                      Expression<Func<FakeEntity, object>> left = s => s.Id == "aws";
                                                                      Expression<Func<FakeEntity, object>> right = s => s.Entity.Id == "aws";
                                                                      left.IsExpressionEqual(right).ShouldBeFalse();
                                                                  };

        It should_be_is_expression_with_double_deep_complexly_name = () =>
                                                                         {
                                                                             Expression<Func<FakeEntity, object>> left = s => s.Entity.Id == "aws";
                                                                             Expression<Func<FakeEntity, object>> right = s => s.Entity.Entity.Id == "aws";
                                                                             left.IsExpressionEqual(right).ShouldBeFalse();
                                                                         };

        #endregion
    }
}