namespace Incoding.UnitTest.MvcContribGroup
{
    using System.Linq;
    using System.Web.Mvc;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    [Subject(typeof(IncSelectList))]
    public class When_inc_select_list
    {
        It should_be_inc_select_list_optiona = () =>
                                               {
                                                   var newValues = Pleasure.Generator.Invent<KeyValueVm[]>();
                                                   IncSelectList selectList = newValues;
                                                   selectList.Optional = Pleasure.Generator.TheSameString();

                                                   var allValues = newValues.ToList();
                                                   allValues.Insert(0, new KeyValueVm(Pleasure.Generator.TheSameString()));

                                                   ((SelectList)selectList).Items.ShouldEqualWeak(allValues);
                                               };


    }
}