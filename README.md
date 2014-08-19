# Incoding Framework

Incoding framework (IncFramework) is a rapid web-development powered by asp.net mvc.

## Benefits and Features:

* Define behaviour of your Ajax dynamic pages with NO Javascript code.
* CQRS implementation is simpler than you thinked of it.
* Mspec helpers would shorter time spent on unit testing
* Model View Disaptcher - cqrs over asp.net MVC

## Installation


We recommended installing ( read [Get Started](http://blog.incframework.com/en/get-started/) ) the NuGet package. Install on the command line from your solution directory:


```
cmd> PM> Install-Package Incoding.Framework
cmd> PM> Install-Package Incoding.MetaLanguage
cmd> PM> Install-Package Incoding.MSpecContrib
Or use the Package Manager console in Visual Studio
```


## IML

     Html.When(JqueryBind.Click)
         .Do()
         .AjaxPost(Url.Dispatcher().Query(new GetUsersQuery
                                     {
                                       Name = Html.Selector.Name(r=>r.Name)
                                     })
                                .AsView("~/Views/User/Index"))
         .OnSuccess(dsl = > dsl.Self().Core().Insert.Html())
         .AsHtmlAttributes()
         .ToDiv()
       
#### Article 
* [Iml vs Jquery](http://blog.incframework.com/en/jqyery-style-vs-iml-style/)
* [Iml vs Ajax asp.net mvc](http://blog.incframework.com/ru/ajax-actionlink-vs-iml/)
* [Selector](http://blog.incframework.com/ru/power-selector/)
* [Iml how it work](http://blog.incframework.com/ru/iml-how-it-work/)

#### Source code
* [Iml vs Jquery ](https://github.com/IncodingSoftware/Jquery-Vs-Iml)
* [Iml vs Ajax asp.net mvc](https://github.com/IncodingSoftware/AjaxAspNetVsIML)
         

## CRQS

### Command

    public class ChangeStatusOrderCommand : CommandBase
    {  
      public string Id { get; set; }
      
      public OrderOfStatus Status { get; set; } 
      
      public override void Execute()
      {
         var order = Repository.GetById<Order>(Id);
         order.ChangeStatus(Status); 
         
         EventBroker.Publish(new OnRefresh()
         {
            Restaurant = order.Device.Restaurant.Name,
            Type = TypeOfPartSystem.Client
         });
         
       }  
    }
    
### QUERY

    public class GetGapsQuery : QueryBase<List<GetGapsQuery.Response>>
      {
        public Guid Status { get; set; }
        
        public bool ShowHistory { get; set; }
        
        public class Response 
        { 
         public string Type { get; set; }
         
         public string Status { get; set; }
         
         public bool Active { get; set; }
        }
        
        protected override List<Response> ExecuteResult()
        {
           return Repository
                .Query(whereSpecification: new GapByStatusOptWhereSpec(Status)
                                              .And(new ActiveEntityWhereSpec<Gap>(ShowHistory)))
                .ToList()                 
                .Select(gap => new Response
                                   {                                                            
                                           Type = gap.Type.Name,
                                           Active = gap.Active,
                                           Status = gap.Status.Name,                            
                                   })
                .ToList();
         }
         
       } 
      

#### Article 
* [CQRS vs Nlayer](http://blog.incframework.com/en/jqyery-style-vs-iml-style/)
* [CQRS extend course](http://blog.incframework.com/ru/cqrs-advanced-course/)
* [Repository](http://blog.incframework.com/ru/repository/)


#### Source code
* [CQRS ](https://github.com/IncodingSoftware/CQRS)
* [Repository](https://github.com/IncodingSoftware/Repository)


## MVD

### Push

       
        Url.Dispatcher().Push(new AddUserCommand {
                                    Id = Html.Selector.Name(r=>r.Id),
                                    Name = Html.Selector.Name(r=>r.Name),
                                                 })
                                                 
### Query as JSON

       
        Url.Dispatcher().Query(new GetCurrentDtQuery()).AsJson()
        
 ### Query as View

       
        Url.Dispatcher().Query(new GetCurrentDtQuery())
                        .AsView("~/Views/Home/Template.cshtml")
        
### View

       
        Url.Dispatcher().AsView("~/Views/Home/Template.cshtml")
        

                                                 
                                                 
* [More sample](http://blog.incframework.com/en/model-view-dispatcher/)

       
## Time spent on unit testing

     [Subject(typeof(GetUsersQuery))]
     public class When_get_users
     {
         #region Estabilish value
         
         static MockMessage<GetUsersQuery, List<User>> mockQuery;
         
         static List<User> expected;
         
         static User user;
         
         #endregion
         
         Establish establish = () =>
                              {
            GetUsersQuery query = Pleasure.Generator.Invent<GetUsersQuery>();
            expected = Pleasure.ToList(Pleasure.Generator.Invent<User>());
            user = Pleasure.Generator.Invent<User>(dsl => dsl.GenerateTo<Classification>(r => r.Faculty)
                                                             .Tuning(r => r.Role, RoleOfType.Admin));
                                                                         
            mockQuery = MockQuery<GetUsersQuery, IncPaginatedResult<User>>
                   .When(query)
                   .StubGetById(CtrPleasure.TheUserId(), user)
                   .StubQuery(whereSpecification: new UserByContentOptWhereSpec(query.Content),
                              entities: expected);                          
                               };
                             
             Because of = () => mockQuery.Original.Execute();
             
             It should_be_result = () => mockQuery.ShouldBeIsResult(expected);
       }
       
       
      
# Documentation

* [blog](http://blog.incframework.com/)
* [site](http://incframework.com/)
* [video](http://www.techdays.ru/speaker/Wlad) 


                  
                 
                  




