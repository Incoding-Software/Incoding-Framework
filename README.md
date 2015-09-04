<p style="text-align: justify;"><em><strong>disclamer:</strong>this article is a step-by-step guide to help you to familiarize with the core functionality of Incoding Framework. Following the guide will result in an application that implements the work with the DB (CRUD + data filters) and fully covered with unit tests.</em></p>

<h1 style="text-align: justify;">Part 0. Introduction</h1>
<p style="text-align: justify;">Let us begin with a short description of Framework. Incoding Framework comprises three packages: Incoding framework – back-end project, Incoding Meta Language – front-end project and Incoding tests helpers – unit-tests for back-end. These packages are installed independently of each other, making it possible to integrate framework by parts into the project: You can connect only front or back end (tests are tightly coupled with the back end, so, they could be more considered as a complement).</p>
<p style="text-align: justify;">Projects developed in <strong>Incoding Framework</strong>,<strong> </strong>use <a title="Martin Fowler: CQRS" href="http://martinfowler.com/bliki/CQRS.html" target="_blank">CQRS</a> as a server architecture. <a title="Habrahabr: Incoding rapid development framework" href="http://habrahabr.ru/post/209734/" target="_blank">Incoding Meta Language</a>. В целом <strong>Incoding Framework </strong> is used as a basic tool for building front-end. All in all, Incoding Framework covers the entire application development cycle.</p>
<p style="text-align: justify;">Typical solution, that was developed using Incoding Framework, comprises 3 projects:</p>

<ol style="text-align: justify;">
	<li style="text-align: justify;"><b>1. Domain (<em>class library) </em></b><em>- </em>is responsible for business logic and database operations.</li>
	<li style="text-align: justify;"><b>UI (<em>ASP.NET MVC project</em>)<i> </i></b><i>- </i>front-end based on ASP.NET MVC.</li>
	<li style="text-align: justify;"><strong>UnitTests (<em>class library</em>) </strong>- unit-tests for Domain.</li>
</ol>
<h3 style="text-align: justify;">Domain</h3>
<p style="text-align: justify;">After installation of  <a title="Nuget: Incoding framework" href="https://www.nuget.org/packages/Incoding.Framework/" target="_blank">Incoding framework</a> through Nuget , along with the necessary dll, Bootstrapper.cs file will be added in the project. The file is mainly responsible for the initialization of an application: logging initialization, IoC registration, installation of Ajax-requests settings, etc. By default, <a title="StructureMap docs" href="http://docs.structuremap.net/">StructureMap</a>is installed as loC framework, but there is a provider for Ninject, and it is also possible to write your own implementations.</p>

<pre class="lang:c# decode:true">namespace Example.Domain
{
    #region &lt;&lt; Using &gt;&gt;

    using System;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using FluentValidation;
    using FluentValidation.Mvc;
    using Incoding.Block.IoC;
    using Incoding.Block.Logging;
    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.EventBroker;
    using Incoding.Extensions;
    using Incoding.MvcContrib;
    using NHibernate.Tool.hbm2ddl;
    using StructureMap.Graph;

    #endregion

    public static class Bootstrapper
    {
        public static void Start()
        {
            //Initialize LoggingFactory
            LoggingFactory.Instance.Initialize(logging =&gt;
                {
                    string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
                    logging.WithPolicy(policy =&gt; policy.For(LogType.Debug)
                                                        .Use(FileLogger.WithAtOnceReplace(path,
                                                                                        () =&gt; "Debug_{0}.txt".F(DateTime.Now.ToString("yyyyMMdd")))));
                });

            //Initialize IoCFactory
            IoCFactory.Instance.Initialize(init =&gt; init.WithProvider(new StructureMapIoCProvider(registry =&gt;
                {
                    //Регистрация Dispatcher
                    registry.For&lt;IDispatcher&gt;().Use&lt;DefaultDispatcher&gt;();
                    //Регистрация Event Broker
                    registry.For&lt;IEventBroker&gt;().Use&lt;DefaultEventBroker&gt;();
                    registry.For&lt;ITemplateFactory&gt;().Singleton().Use&lt;TemplateHandlebarsFactory&gt;();

                    //Настройка FluentlyNhibernate
                    var configure = Fluently
                            .Configure()
                            .Database(MsSqlConfiguration.MsSql2008.ConnectionString(ConfigurationManager.ConnectionStrings["Example"].ConnectionString))
                            .Mappings(configuration =&gt; configuration.FluentMappings.AddFromAssembly(typeof(Bootstrapper).Assembly))
                            .ExposeConfiguration(cfg =&gt; new SchemaUpdate(cfg).Execute(false, true))
                            .CurrentSessionContext&lt;NhibernateSessionContext&gt;(); //Настройка конфигурации базы данных

                    registry.For&lt;INhibernateSessionFactory&gt;().Singleton().Use(() =&gt; new NhibernateSessionFactory(configure));
                    registry.For&lt;IUnitOfWorkFactory&gt;().Use&lt;NhibernateUnitOfWorkFactory&gt;();
                    registry.For&lt;IRepository&gt;().Use&lt;NhibernateRepository&gt;();

                    //Scna currenlty Assembly and registrations all Validators and Event Subscribers
                    registry.Scan(r =&gt;
                                    {
                                        r.TheCallingAssembly();
                                        r.WithDefaultConventions();

                                        r.ConnectImplementationsToTypesClosing(typeof(AbstractValidator&lt;&gt;));
                                        r.ConnectImplementationsToTypesClosing(typeof(IEventSubscriber&lt;&gt;));
                                        r.AddAllTypesOf&lt;ISetUp&gt;();
                                    });
                })));

            ModelValidatorProviders.Providers.Add(new FluentValidationModelValidatorProvider(new IncValidatorFactory()));
            FluentValidationModelValidatorProvider.Configure();

            //Execute all SetUp
            foreach (var setUp in IoCFactory.Instance.ResolveAll&lt;ISetUp&gt;().OrderBy(r =&gt; r.GetOrder()))
            {
                setUp.Execute();
            }

            var ajaxDef = JqueryAjaxOptions.Default;
            ajaxDef.Cache = false; //Disable Ajax cache
        }
    }
}</pre>
<p style="text-align: justify;">Further on, commands (Command) and queries (Query) are added to Domain, that perform database operations or any action, related with business application logic.</p>

<h3 style="text-align: justify;">UI</h3>
<p style="text-align: justify;">During the installation of Package <a title="Nuget: Incoding Meta Language" href="https://www.nuget.org/packages/Incoding.MetaLanguage/">Incoding Meta Language</a> , it adds the necessary dll to the package, as well as IncodingStart.cs and DispatcherController.cs (part <a title="Habrahabr: Model View Dispatcher (cqrs over mvc)" href="http://habrahabr.ru/post/221585/">MVD</a>) files required to work Domain.</p>

<pre class="lang:c# decode:true">public static class IncodingStart
{
    public static void PreStart()
    {
        Bootstrapper.Start();
        new DispatcherController(); // init routes
    }
}</pre>
<pre class="lang:c# decode:true">public class DispatcherController : DispatcherControllerBase
{
    #region Constructors

    public DispatcherController()
            : base(typeof(Bootstrapper).Assembly) { }

    #endregion
}</pre>
<p style="text-align: justify;">After the installation, the client logic is added to <strong>UI</strong> using <a title="Habrahabr: Incoding rapid development framework" href="http://habrahabr.ru/post/209734/" target="_blank">IML</a>.</p>

<h3>UnitTests</h3>
<p style="text-align: justify;">During the installation of <a title="Nuget: Incoding tests helpers" href="https://www.nuget.org/packages/Incoding.MSpecContrib/">Incoding tests helpers</a>, the project is added by the MSpecAssemblyContext.csis file, in which connection is customize to the test dtabse.</p>

<pre class="lang:c# decode:true">public class MSpecAssemblyContext : IAssemblyContext
{
    #region IAssemblyContext Members

    public void OnAssemblyStart()
    {
        //Configuration data base
        var configure = Fluently
                .Configure()
                .Database(MsSqlConfiguration.MsSql2008
                                            .ConnectionString(ConfigurationManager.ConnectionStrings["Example_Test"].ConnectionString)
                                            .ShowSql())
                .Mappings(configuration =&gt; configuration.FluentMappings.AddFromAssembly(typeof(Bootstrapper).Assembly));

        PleasureForData.StartNhibernate(configure, true);
    }

    public void OnAssemblyComplete() { }

    #endregion
}</pre>
<h2>Part 1. Installation.</h2>
<p style="text-align: justify;">So, we proceed to the task of the <em>disclamer </em> and start writing our application. The first phase of building the application is to create solution structure of a project and to add the projects to it. The project solution will be called Example and, as was already mentioned in the introduction, will have 3 projects. We begin with the project that is responsible for business logic of the application - Domain.</p>
<p style="text-align: justify;">Create class library <strong>Domain</strong>.</p>
<p style="text-align: justify;"><a href="http://blog.incframework.com/wp-content/uploads/2015/06/Domain.png"><img class="aligncenter wp-image-1522 size-full" src="http://blog.incframework.com/wp-content/uploads/2015/06/Domain-e1433938652855.png" alt="Domain" width="800" height="553" /></a></p>
<p style="text-align: justify;">Then we proceed to the front-end – create and install ASP.NET Web Application UI with links to the MVC packages as template, empty project.</p>
<a href="http://blog.incframework.com/wp-content/uploads/2015/06/UI1.png"><img class="aligncenter wp-image-1523 size-full" src="http://blog.incframework.com/wp-content/uploads/2015/06/UI1-e1433938677813.png" alt="UI1" width="800" height="553" /></a>

<a href="http://blog.incframework.com/wp-content/uploads/2015/06/UI2.png"><img class="aligncenter wp-image-1524 size-full" src="http://blog.incframework.com/wp-content/uploads/2015/06/UI2-e1433938757884.png" alt="UI2" width="770" height="540" /></a>
<p style="text-align: justify;">Finally, we add class library UnitTests that is responsible for unit testing.</p>
<a href="http://blog.incframework.com/wp-content/uploads/2015/06/UnitTests.png"><img class="aligncenter wp-image-1525 size-full" src="http://blog.incframework.com/wp-content/uploads/2015/06/UnitTests-e1433938798326.png" alt="UnitTests" width="800" height="553" /></a>
<p style="text-align: justify;"><em><strong>Note: </strong>Alt<em>hough UnitTests are not an obligatory part of the application, we recommend you to cover the code with tests as it will help to avoid numerous problems in future with various possible faults in the code due to test automation. </em></em></p>
After having finished all the above activities, you will get following solution:

<a href="http://blog.incframework.com/wp-content/uploads/2015/06/Solution.png"><img class="aligncenter wp-image-1527 size-full" src="http://blog.incframework.com/wp-content/uploads/2015/06/Solution-e1433938901524.png" alt="Solution" width="664" height="547" /></a>

After we create the solution structure, we need to install<strong> Incoding Framework</strong> package from Nuget.

The installation carried out by Nuget. There is the same algorithm of installation for all the projects:
<ol>
	<li>Right-click the project and select <strong>Manage Nuget Packages</strong>… in the context menu</li>
	<li>Search <strong>incoding</strong></li>
	<li>Select necessary package and install it</li>
</ol>
First install <a title="Incoding framework" href="https://www.nuget.org/packages/Incoding.Framework/">Incoding framework</a> in <strong>Domain</strong>.

<a href="http://blog.incframework.com/wp-content/uploads/2015/06/Incoding_framework_1.png"><img class="aligncenter wp-image-1530 size-full" src="http://blog.incframework.com/wp-content/uploads/2015/06/Incoding_framework_1-e1433940738743.png" alt="Incoding_framework_1" width="800" height="539" /></a>
<p style="text-align: justify;">Then add to the file <strong>Domain -&gt; Infrastructure -&gt; Bootstrapper.cs</strong> the link to StructureMap.Graph.</p>
<a href="http://blog.incframework.com/wp-content/uploads/2015/06/StructureMap_ref.png"><img class="aligncenter wp-image-1531 size-full" src="http://blog.incframework.com/wp-content/uploads/2015/06/StructureMap_ref-e1433940776329.png" alt="StructureMap_ref" width="800" height="63" /></a>

2 packages must be installed to UI:
<ol>
	<li><a title="Nuget: Incoding Meta Language" href="https://www.nuget.org/packages/Incoding.MetaLanguage/">Incoding Meta Language</a></li>
	<li><a title="Nuget: Incoding Meta Language Contrib" href="https://www.nuget.org/packages/Incoding.MetaLanguage.Contrib/">Incoding Meta Language Contrib</a></li>
</ol>
<p style="text-align: justify;"><strong><img class="aligncenter wp-image-1539 size-full" src="http://blog.incframework.com/wp-content/uploads/2015/06/Incoding_Meta_Languge-e1433940844592.png" alt="Incoding_Meta_Languge" width="800" height="539" /></strong></p>
<p style="text-align: justify;"><a href="http://blog.incframework.com/wp-content/uploads/2015/06/MetaLanguageContrib_install.png"><img class="aligncenter wp-image-1562 size-full" src="http://blog.incframework.com/wp-content/uploads/2015/06/MetaLanguageContrib_install-e1433941058675.png" alt="MetaLanguageContrib_install" width="800" height="539" /></a></p>
<p style="text-align: justify;"><b><i>Note: </i></b><i>make sure that the Copy Local property is set to true in the</i><i><em> References -&gt; System.Web.Mvc.dll</em></i></p>
<p style="text-align: justify;">Now change the file  <strong>Example.UI -&gt; Views -&gt; Shared -&gt; _Layout.cshtml </strong>so that it looks as follows:</p>

<pre class="lang:c# decode:true">@using Incoding.MvcContrib
&lt;!DOCTYPE html&gt;
&lt;html &gt;
&lt;head&gt;
    &lt;script type="text/javascript" src="@Url.Content("~/Scripts/jquery-1.9.1.min.js")"&gt; &lt;/script&gt;
    &lt;script type="text/javascript" src="@Url.Content("~/Scripts/jquery-ui-1.10.2.min.js")"&gt;&lt;/script&gt;
    &lt;script type="text/javascript" src="@Url.Content("~/Scripts/underscore.min.js")"&gt; &lt;/script&gt;
    &lt;script type="text/javascript" src="@Url.Content("~/Scripts/jquery.form.min.js")"&gt; &lt;/script&gt;
    &lt;script type="text/javascript" src="@Url.Content("~/Scripts/jquery.history.js")"&gt; &lt;/script&gt;
    &lt;script type="text/javascript" src="@Url.Content("~/Scripts/jquery.validate.min.js")"&gt; &lt;/script&gt;
    &lt;script type="text/javascript" src="@Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js")"&gt; &lt;/script&gt;
    &lt;script type="text/javascript" src="@Url.Content("~/Scripts/handlebars-1.1.2.js")"&gt; &lt;/script&gt;
    &lt;script type="text/javascript" src="@Url.Content("~/Scripts/incoding.framework.min.js")"&gt; &lt;/script&gt;
    &lt;script type="text/javascript" src="@Url.Content("~/Scripts/incoding.meta.language.contrib.js")"&gt; &lt;/script&gt;
    &lt;script type="text/javascript" src="@Url.Content("~/Scripts/bootstrap.min.js")"&gt; &lt;/script&gt;
    &lt;link rel="stylesheet" type="text/css" href="@Url.Content("~/Content/bootstrap.min.css")"&gt;
    &lt;link rel="stylesheet" type="text/css" href="@Url.Content("~/Content/themes/base/jquery.ui.core.css")"&gt;
    &lt;link rel="stylesheet" type="text/css" href="@Url.Content("~/Content/themes/base/jquery.ui.datepicker.css")"&gt;
    &lt;link rel="stylesheet" type="text/css" href="@Url.Content("~/Content/themes/base/jquery.ui.dialog.css")"&gt;
    &lt;link rel="stylesheet" type="text/css" href="@Url.Content("~/Content/themes/base/jquery.ui.theme.css")"&gt;
    &lt;link rel="stylesheet" type="text/css" href="@Url.Content("~/Content/themes/base/jquery.ui.menu.css")"&gt;
    &lt;script&gt;
        TemplateFactory.Version = '@Guid.NewGuid().ToString()';
    &lt;/script&gt;
&lt;/head&gt;
@Html.Incoding().RenderDropDownTemplate()
&lt;body&gt;
@RenderBody()
&lt;/body&gt;
&lt;/html&gt;</pre>
Then add the link to Bootstrapper.cs to the files <strong>Example.UI -&gt; App_Start -&gt; IncodingStart.cs and Example.UI -&gt; Controllers -&gt; DispatcherController.cs.</strong>

<a href="http://blog.incframework.com/wp-content/uploads/2015/06/IncodingStart_bootstrapper.png"><img class="aligncenter wp-image-1540 size-full" src="http://blog.incframework.com/wp-content/uploads/2015/06/IncodingStart_bootstrapper-e1433941248848.png" alt="IncodingStart_bootstrapper" width="400" height="240" /></a>

<a href="http://blog.incframework.com/wp-content/uploads/2015/06/DispatcherController_bootstrapper.png"><img class="aligncenter wp-image-1541 size-full" src="http://blog.incframework.com/wp-content/uploads/2015/06/DispatcherController_bootstrapper-e1433941221806.png" alt="DispatcherController_bootstrapper" width="712" height="185" /></a>
<p style="text-align: justify;"><em><strong>Note: </strong></em><em>If you use MVC5, it’s necessary for framework to add following code to Web.config file.</em></p>

<pre class="lang:c# decode:true">&lt;dependentAssembly&gt;
  &lt;assemblyIdentity name="System.Web.Mvc" publicKeyToken="31bf3856ad364e35" culture="neutral" /&gt;
  &lt;bindingRedirect oldVersion="0.0.0.0-5.0.0.0" newVersion="5.0.0.0" /&gt;
&lt;/dependentAssembly&gt;</pre>
<p style="text-align: justify;">Now install  <a title="Nuget: Incoding tests helpers" href="https://www.nuget.org/packages/Incoding.MSpecContrib/">Incoding tests helpers</a> in<strong>UnitTests </strong>and add the link to Bootstrapper.cs in Example.UnitTests -&gt; MSpecAssemblyContext.cs.</p>
<a href="http://blog.incframework.com/wp-content/uploads/2015/06/Incoding_tests_helpers.png"><img class="aligncenter wp-image-1542 size-full" src="http://blog.incframework.com/wp-content/uploads/2015/06/Incoding_tests_helpers-e1433941531335.png" alt="Incoding_tests_helpers" width="800" height="539" /></a>

<a href="http://blog.incframework.com/wp-content/uploads/2015/06/MSpecAssemblyContext_bootstrapper.png"><img class="aligncenter wp-image-1544 size-full" src="http://blog.incframework.com/wp-content/uploads/2015/06/MSpecAssemblyContext_bootstrapper-e1433941543161.png" alt="MSpecAssemblyContext_bootstrapper" width="800" height="116" /></a>

The last phase of the preparation the projects to work is to create folders structure for the projects.

Add following folders to the <strong>Example.Domain </strong>project:
<ol>
	<li>Operations – command and query of the project</li>
	<li>Persistences – entities for DB mapping</li>
	<li>Specifications – where and order specifications for data cleaning when request is made</li>
</ol>
<a href="http://blog.incframework.com/wp-content/uploads/2015/06/Example.Domain_folders.png"><img class="aligncenter wp-image-1557 size-full" src="http://blog.incframework.com/wp-content/uploads/2015/06/Example.Domain_folders-e1433942308588.png" alt="Example.Domain_folders" width="350" height="154" /></a>

In the <strong>Example.UnitTests </strong>project create just the same folders structure as in <strong>Example.Domain.</strong>

<a href="http://blog.incframework.com/wp-content/uploads/2015/06/UnitTests_folders.png"><img class="aligncenter wp-image-1558 size-full" src="http://blog.incframework.com/wp-content/uploads/2015/06/UnitTests_folders-e1433942319802.png" alt="UnitTests_folders" width="310" height="172" /></a>
<h1><strong>Part 2. Setting up a DB connection.</strong></h1>
To begin this process, create DB with which you will work. Open SQL Managment Studio and create two DB: Example and Example_test.
<p style="text-align: justify;"><a href="http://blog.incframework.com/wp-content/uploads/2015/06/add_DB1.png"><img class="aligncenter wp-image-1597 size-full" src="http://blog.incframework.com/wp-content/uploads/2015/06/add_DB1-e1434366917892.png" alt="add_DB" width="525" height="291" /></a></p>
<p style="text-align: justify;"><a href="http://blog.incframework.com/wp-content/uploads/2015/06/example_db.png"><img class="aligncenter wp-image-1598 size-full" src="http://blog.incframework.com/wp-content/uploads/2015/06/example_db-e1434367010409.png" alt="example_db" width="525" height="471" /></a></p>
<p style="text-align: justify;"><a href="http://blog.incframework.com/wp-content/uploads/2015/06/example_test_db.png"><img class="aligncenter wp-image-1599 size-full" src="http://blog.incframework.com/wp-content/uploads/2015/06/example_test_db-e1434367032172.png" alt="example_test_db" width="525" height="471" /></a></p>
In order to work with DB, you need to set up a connection. Add to the file <strong>Example.UI -&gt; Web.config and Example.UnitTests -&gt; app.config connection </strong>string to the BD:
<pre class="lang:c# decode:true">  &lt;connectionStrings&gt;
    &lt;add name="Example" connectionString="Data Source=INCODING-PC\SQLEXPRESS;Database=Example;Integrated Security=false; User Id=sa;Password=1" providerName="System.Data.SqlClient" /&gt;
    &lt;add name="Example_Test" connectionString="Data Source=INCODING-PC\SQLEXPRESS;Database=Example_Test;Integrated Security=true" providerName="System.Data.SqlClient" /&gt;
  &lt;/connectionStrings&gt;</pre>
In the file<strong> Example.Domain -&gt; Infrastructure -&gt; Bootstrapper.cs</strong>, register the appropriate connection string using a key called Example:
<pre class="lang:c# decode:true">//Настройка FluentlyNhibernate
var configure = Fluently
        .Configure()
        .Database(MsSqlConfiguration.MsSql2008.ConnectionString(ConfigurationManager.ConnectionStrings["Example"].ConnectionString))
        .Mappings(configuration =&gt; configuration.FluentMappings.AddFromAssembly(typeof(Bootstrapper).Assembly))
        .ExposeConfiguration(cfg =&gt; new SchemaUpdate(cfg).Execute(false, true))
        .CurrentSessionContext(); //Настройка конфигурации базы данных</pre>
In the file <strong>Example.UnitTests -&gt; MSpecAssemblyContext.cs</strong>, register the connection string to the BD using the key called Example_test:
<pre class="lang:c# decode:true">//Настройка подключения к тестовой БД
var configure = Fluently
        .Configure()
        .Database(MsSqlConfiguration.MsSql2008
                                    .ConnectionString(ConfigurationManager.ConnectionStrings["Example_Test"].ConnectionString)
                                    .ShowSql())
        .Mappings(configuration =&gt; configuration.FluentMappings.AddFromAssembly(typeof(Bootstrapper).Assembly));</pre>
<strong>Note</strong>: Example and Example_test databases must exist.
<h1>Part 3. CRUD.</h1>
After the actions described above, we come to the most interesting part – code writing implementing the CRUD (<strong>c</strong>reate, <strong>r</strong>ead, <strong>u</strong>pdate, <strong>d</strong>elete) functionality of an application. To begin this process, create an entity class that will map to the DB. In our case, this is Human.cs that we add to the <strong>Example.Domain -&gt; Persistences folder</strong>.
<h6>Human.cs</h6>
<pre class="lang:c# decode:true">namespace Example.Domain
{
    #region &lt;&lt; Using &gt;&gt;

    using System;
    using Incoding.Data;

    #endregion

    public class Human : IncEntityBase
    {
        #region Properties

        public virtual DateTime Birthday { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string Id { get; set; }

        public virtual string LastName { get; set; }

        public virtual Sex Sex { get; set; }

        #endregion

        #region Nested Classes

        public class Map : NHibernateEntityMap&lt;Human&gt;
        {
            #region Constructors

            protected Map()
            {
                IdGenerateByGuid(r =&gt; r.Id);
                MapEscaping(r =&gt; r.FirstName);
                MapEscaping(r =&gt; r.LastName);
                MapEscaping(r =&gt; r.Birthday);
                MapEscaping(r =&gt; r.Sex);
            }

            #endregion
        }

        #endregion
    }

    public enum Sex
    {
        Male = 1,

        Female = 2
    }
}</pre>
Our class contains several fields where we will write data and Nested Class Map.
<p style="text-align: justify;"><em><strong>Note:</strong> after creating the <strong>Human</strong> class, you do not need to perform any operations (creating an XML mapping) due to  <a title="Fluent Nhibernate" href="http://www.fluentnhibernate.org/">FluentNhibernate</a>.</em></p>
We can now add commands and queries, which are responsible for realization of the CRUD operations. The first command will be responsible for adding a new or change an existing record of the Human type.  The command is quite simple: we either get an entity on a Repository using the key (ld) or, if no entity exist, we create a new one. Both of these entities get the values specified   in the properties of the AddOrEditHumanCommand class. Add <strong>Example.Domain -&gt; Operations -&gt; AddOrEditHumanCommand.cs to the project.</strong>
<h6>AddOrEditHumanCommand.cs</h6>
<pre class="lang:c# decode:true">namespace Example.Domain
{
    #region &lt;&lt; Using &gt;&gt;

    using System;
    using FluentValidation;
    using Incoding.CQRS;
    using Incoding.Extensions;

    #endregion

    public class AddOrEditHumanCommand : CommandBase
    {
        #region Properties

        public DateTime BirthDay { get; set; }

        public string FirstName { get; set; }

        public string Id { get; set; }

        public string LastName { get; set; }

        public Sex Sex { get; set; }

        #endregion

        public override void Execute()
        {
            var human = Repository.GetById&lt;Human&gt;(Id) ?? new Human();

            human.FirstName = FirstName;
            human.LastName = LastName;
            human.Birthday = BirthDay;
            human.Sex = Sex;

            Repository.SaveOrUpdate(human);
        }
    }
}</pre>
The Read command is the second part of the CRUD. This is a request for reading entities from the DB. Add the file <strong>Example.Domain -&gt; Operations -&gt; GetPeopleQuery.cs</strong>.
<h6>GetPeopleQuery.cs</h6>
<pre class="lang:c# decode:true">namespace Example.Domain
{
    #region &lt;&lt; Using &gt;&gt;

    using System.Collections.Generic;
    using System.Linq;
    using Incoding.CQRS;

    #endregion

    public class GetPeopleQuery : QueryBase&lt;List&lt;GetPeopleQuery.Response&gt;&gt;
    {
        #region Properties

        public string Keyword { get; set; }

        #endregion

        #region Nested Classes

        public class Response
        {
            #region Properties

            public string Birthday { get; set; }

            public string FirstName { get; set; }

            public string Id { get; set; }

            public string LastName { get; set; }

            public string Sex { get; set; }

            #endregion
        }

        #endregion

        protected override List&lt;Response&gt; ExecuteResult()
        {
            return Repository.Query&lt;Human&gt;().Select(human =&gt; new Response
                                                                 {
                                                                         Id = human.Id,
                                                                         Birthday = human.Birthday.ToShortDateString(),
                                                                         FirstName = human.FirstName,
                                                                         LastName = human.LastName,
                                                                         Sex = human.Sex.ToString()
                                                                 }).ToList();
        }
    }
}</pre>
The Delete command is the remaining part of the CRUD. The command deletes records from the DB using the key (ld). Add the file <strong>Example.Domain -&gt; Operations -&gt; DeleteHumanCommand.cs</strong>.
<h6>DeleteHumanCommand.cs</h6>
<pre class="lang:c# decode:true">namespace Example.Domain
{
    #region &lt;&lt; Using &gt;&gt;

    using Incoding.CQRS;

    #endregion

    public class DeleteHumanCommand : CommandBase
    {
        #region Properties

        public string HumanId { get; set; }

        #endregion

        public override void Execute()
        {
            Repository.Delete&lt;Human&gt;(HumanId);
        }
    }
}</pre>
In order to populate the DB with initial data, add the file <strong>Example.Domain -&gt; InitPeople.cs </strong>that is derived from the ISetUP interface.
<h6 style="text-align: justify;">ISetup</h6>
<pre class="lang:c# decode:true">using System;

namespace Incoding.CQRS
{
  public interface ISetUp : IDisposable
  {
    int GetOrder();

    void Execute();
  }
}</pre>
All the class instances from the ISetUp are registered with IoC in the Bootstrapper.cs (see Introduction) and run (public void Execute() ) in order (public int GetOrder() ).
<h6>InitPeople.cs</h6>
<pre class="lang:c# decode:true">namespace Example.Domain
{
    #region &lt;&lt; Using &gt;&gt;

    using System;
    using Incoding.Block.IoC;
    using Incoding.CQRS;
    using NHibernate.Util;

    #endregion

    public class InitPeople : ISetUp
    {
        public void Dispose() { }

        public int GetOrder()
        {
            return 0;
        }

        public void Execute()
        {
            //get Dispatcher for execute Query or Command
            var dispatcher = IoCFactory.Instance.TryResolve&lt;IDispatcher&gt;();
            
            //don't add new entity if exits
            if (dispatcher.Query(new GetEntitiesQuery&lt;Human&gt;()).Any())
                return;

            //Adding new entity
            dispatcher.Push(new AddOrEditHumanCommand
                                {
                                        FirstName = "Hellen",
                                        LastName = "Jonson",
                                        BirthDay = Convert.ToDateTime("06/05/1985"),
                                        Sex = Sex.Female
                                });
            dispatcher.Push(new AddOrEditHumanCommand
                                {
                                        FirstName = "John",
                                        LastName = "Carlson",
                                        BirthDay = Convert.ToDateTime("06/07/1985"),
                                        Sex = Sex.Male
                                });
        }
    }
}</pre>
The back-end implementation of the CRUD is ready. Now it is time to add a user code. As in the case of the back end, we begin the implementation with creating/editing a record. Add the file<strong> Example.UI -&gt; Views -&gt; Home -&gt; AddOrEditHuman.cshtml. </strong>
<h6>AddOrEditHuman.cshtml</h6>
<pre class="lang:c# decode:true">@using Example.Domain
@using Incoding.MetaLanguageContrib
@using Incoding.MvcContrib
@model Example.Domain.AddOrEditHumanCommand
@*Submit form for  AddOrEditHumanCommand*@
@using (Html.When(JqueryBind.Submit)
            @*Prevent default behavior and submit form by Ajax*@
            .PreventDefault()
            .Submit()
            .OnSuccess(dsl =&gt;
                           {
                               dsl.WithId("PeopleTable").Core().Trigger.Incoding();
                               dsl.WithId("dialog").JqueryUI().Dialog.Close();
                           })
            .OnError(dsl =&gt; dsl.Self().Core().Form.Validation.Refresh())
            .AsHtmlAttributes(new
                                  {
                                          action = Url.Dispatcher().Push(new AddOrEditHumanCommand()),
                                          enctype = "multipart/form-data",
                                          method = "POST"
                                  })
            .ToBeginTag(Html, HtmlTag.Form))
{
    &lt;div&gt;
        @Html.HiddenFor(r =&gt; r.Id)
        @Html.ForGroup(r =&gt; r.FirstName).TextBox(control =&gt; control.Label.Name = "First name")
        &lt;br/&gt;
        @Html.ForGroup(r =&gt; r.LastName).TextBox(control =&gt; control.Label.Name = "Last name")
        &lt;br/&gt;
        @Html.ForGroup(r =&gt; r.BirthDay).TextBox(control =&gt; control.Label.Name = "Birthday")
        &lt;br/&gt;
        @Html.ForGroup(r =&gt; r.Sex).DropDown(control =&gt; control.Input.Data = typeof(Sex).ToSelectList())
    &lt;/div&gt;

    &lt;div&gt;
        &lt;input type="submit" value="Save"/&gt;
        @*Закрытие диалога*@
        @(Html.When(JqueryBind.Click)
              .PreventDefault()
              .StopPropagation()
              .Direct()
              .OnSuccess(dsl =&gt; { dsl.WithId("dialog").JqueryUI().Dialog.Close(); })
              .AsHtmlAttributes()
              .ToButton("Cancel"))
    &lt;/div&gt;
}</pre>
The IML-code creates the standard HTML form and works with AddOrEditHumanCommand, sending the appropriate Ajax query to the server.

Then comes the template for data loading through the GetPeopleQuery. There is a description of the table that will be responsible not only for data output, but also for record deletion and editing: add the file <strong>Example.UI -&gt; Views -&gt; Home -&gt; HumanTmpl.cshtml.</strong>
<h6>HumanTmpl.cshtml</h6>
<pre class="lang:c# decode:true">@using Example.Domain
@using Incoding.MetaLanguageContrib
@using Incoding.MvcContrib
@{
    using (var template = Html.Incoding().Template&lt;GetPeopleQuery.Response&gt;())
    {
        &lt;table class="table"&gt;
            &lt;thead&gt;
            &lt;tr&gt;
                &lt;th&gt;
                    First name
                &lt;/th&gt;
                &lt;th&gt;
                    Last name
                &lt;/th&gt;
                &lt;th&gt;
                    Birthday
                &lt;/th&gt;
                &lt;th&gt;
                    Sex
                &lt;/th&gt;
                &lt;th&gt;&lt;/th&gt;
            &lt;/tr&gt;
            &lt;/thead&gt;
            &lt;tbody&gt;
            @using (var each = template.ForEach())
            {
                &lt;tr&gt;
                    &lt;td&gt;
                        @each.For(r =&gt; r.FirstName)
                    &lt;/td&gt;
                    &lt;td&gt;
                        @each.For(r =&gt; r.LastName)
                    &lt;/td&gt;
                    &lt;td&gt;
                        @each.For(r =&gt; r.Birthday)
                    &lt;/td&gt;
                    &lt;td&gt;
                        @each.For(r =&gt; r.Sex)
                    &lt;/td&gt;
                    &lt;td&gt;
                        @*Open edit dialog form*@
                        @(Html.When(JqueryBind.Click)
                              .AjaxGet(Url.Dispatcher().Model&lt;AddOrEditHumanCommand&gt;(new
                                                                                         {
                                                                                                 Id = each.For(r =&gt; r.Id),
                                                                                                 FirstName = each.For(r =&gt; r.FirstName),
                                                                                                 LastName = each.For(r =&gt; r.LastName),
                                                                                                 BirthDay = each.For(r =&gt; r.Birthday),
                                                                                                 Sex = each.For(r =&gt; r.Sex)
                                                                                         }).AsView("~/Views/Home/AddOrEditHuman.cshtml"))
                              .OnSuccess(dsl =&gt; dsl.WithId("dialog").Behaviors(inDsl =&gt;
                                                                                   {
                                                                                       inDsl.Core().Insert.Html();
                                                                                       inDsl.JqueryUI().Dialog.Open(option =&gt;
                                                                                                                        {
                                                                                                                            option.Resizable = false;
                                                                                                                            option.Title = "Edit human";
                                                                                                                        });
                                                                                   }))
                              .AsHtmlAttributes()
                              .ToButton("Edit"))
                        @*Button delete*@
                        @(Html.When(JqueryBind.Click)
                              .AjaxPost(Url.Dispatcher().Push(new DeleteHumanCommand() { HumanId = each.For(r =&gt; r.Id) }))
                              .OnSuccess(dsl =&gt; dsl.WithId("PeopleTable").Core().Trigger.Incoding())
                              .AsHtmlAttributes()
                              .ToButton("Delete"))
                    &lt;/td&gt;
                &lt;/tr&gt;
            }
            &lt;/tbody&gt;
        &lt;/table&gt;
    }
}</pre>
<strong>Note:</strong> <em>The task of opening a dialog box is quite common, so the code that is responsible for this task can be exported to the extension. </em>

Thus, it remains to change the start page so that during its loading AJAX query is transmitted to the server for obtaining data from the GetPeopleQuery and mapping of data using HumanTmpl: change the file <strong>Example.UI -&gt; Views -&gt; Home -&gt; Index.cshtml </strong>so that it looks as follows.
<h6>Index.cshtml</h6>
<pre class="lang:c# decode:true">@using Example.Domain
@using Incoding.MetaLanguageContrib
@using Incoding.MvcContrib
@{
    Layout = "~/Views/Shared/_Layout.cshtml";
}
&lt;div id="dialog"&gt;&lt;/div&gt;
@*Fetch data from GetPeopleQuery, through HumanTmpl*@
@(Html.When(JqueryBind.InitIncoding)
      .AjaxGet(Url.Dispatcher().Query(new GetPeopleQuery()).AsJson())
      .OnSuccess(dsl =&gt; dsl.Self().Core().Insert.WithTemplateByUrl(Url.Dispatcher().AsView("~/Views/Home/HumanTmpl.cshtml")).Html())
      .AsHtmlAttributes(new { id = "PeopleTable" })
      .ToDiv())
@*Button add*@
@(Html.When(JqueryBind.Click)
      .AjaxGet(Url.Dispatcher().AsView("~/Views/Home/AddOrEditHuman.cshtml"))
      .OnSuccess(dsl =&gt; dsl.WithId("dialog").Behaviors(inDsl =&gt;
                                                           {
                                                               inDsl.Core().Insert.Html();
                                                               inDsl.JqueryUI().Dialog.Open(option =&gt;
                                                                                                {
                                                                                                    option.Resizable = false;
                                                                                                    option.Title = "Add human";
                                                                                                });
                                                           }))
      .AsHtmlAttributes()
      .ToButton("Add new human"))</pre>
In real-world applications, validation of input form data is one of the most frequent task. Therefore, we add data validation on the adding/editing form of the Human entity. First, we need to add a server code. Add the following code in AddOrEditHumanCommand as a nested class:
<pre class="lang:c# decode:true">#region Nested Classes

public class Validator : AbstractValidator
{
    #region Constructors

    public Validator()
    {
        RuleFor(r =&gt; r.FirstName).NotEmpty();
        RuleFor(r =&gt; r.LastName).NotEmpty();
    }

    #endregion
}

#endregion</pre>
On the AddOrEditHuman.cshtml form, we used constructs like this:
<pre class="lang:c# decode:true">@Html.ForGroup()</pre>
It is therefore not necessary to add
<pre class="lang:c# decode:true">@Html.ValidationMessageFor()</pre>
<p style="text-align: justify;">for the fields - <a title="Советы и подсказки" href="http://blog.incframework.com/ru/tips-and-trick/">ForGroup()</a> will do it.</p>
So we have written the application code that implements the CRUD functionality for one DB entity.
<h1>Part 4. Specifications - data cleaning.</h1>
Another task that often occurs in real projects is to clean requested data.  Incoding Framework uses WhereSpecifications for convenient code writing and complying an encapsulation principle for cleaning data from Query. In the written code add a possibility to clean data from GetPeopleQuery by FirstName and LastName. First, add two specification files <strong>Example.Domain -&gt; Specifications -&gt; HumanByFirstNameWhereSpec.cs </strong>and <strong>Example.UI -&gt; Specifications -&gt; HumanByLastNameWhereSpec.cs</strong>
<h6 style="text-align: justify;">HumanByFirstNameWhereSpec.cs</h6>
<pre class="lang:c# decode:true">namespace Example.Domain
{
    #region &lt;&lt; Using &gt;&gt;

    using System;
    using System.Linq.Expressions;
    using Incoding;

    #endregion

    public class HumanByFirstNameWhereSpec : Specification
    {
        #region Fields

        readonly string firstName;

        #endregion

        #region Constructors

        public HumanByFirstNameWhereSpec(string firstName)
        {
            this.firstName = firstName;
        }

        #endregion

        public override Expression&lt;Func&lt;Human, bool&gt;&gt; IsSatisfiedBy()
        {
            if (string.IsNullOrEmpty(this.firstName))
                return null;

            return human =&gt; human.FirstName.ToLower().Contains(this.firstName.ToLower());
        }
    }
}</pre>
<h6>HumanByLastNameWhereSpec.cs</h6>
<pre class="lang:c# decode:true">namespace Example.Domain
{
    #region &lt;&lt; Using &gt;&gt;

    using System;
    using System.Linq.Expressions;
    using Incoding;

    #endregion

    public class HumanByLastNameWhereSpec : Specification
    {
        #region Fields

        readonly string lastName;

        #endregion

        #region Constructors

        public HumanByLastNameWhereSpec(string lastName)
        {
            this.lastName = lastName.ToLower();
        }

        #endregion

        public override Expression&lt;Func&lt;Human, bool&gt;&gt; IsSatisfiedBy()
        {
            if (string.IsNullOrEmpty(this.lastName))
                return null;

            return human =&gt; human.LastName.ToLower().Contains(this.lastName);
        }
    }
}</pre>
Now use the written specifications in GetPeopleQuery. .Or()/.And() relations allow to merge atomic specifications that helps to use the created specifications many times and fine-tune necessary data filters (in the example we use .Or() relation)
<h6>GetPeopleQuery.cs</h6>
<pre class="lang:c# decode:true ">namespace Example.Domain
{
    #region &lt;&lt; Using &gt;&gt;

    using System.Collections.Generic;
    using System.Linq;
    using Incoding.CQRS;
    using Incoding.Extensions;

    #endregion

    public class GetPeopleQuery : QueryBase&lt;List&lt;GetPeopleQuery.Response&gt;&gt;
    {
        #region Properties

        public string Keyword { get; set; }

        #endregion

        #region Nested Classes

        public class Response
        {
            #region Properties

            public string Birthday { get; set; }

            public string FirstName { get; set; }

            public string Id { get; set; }

            public string LastName { get; set; }

            public string Sex { get; set; }

            #endregion
        }

        #endregion

        protected override List&lt;Response&gt; ExecuteResult()
        {
            return Repository.Query(whereSpecification: new HumanByFirstNameWhereSpec(Keyword)
                                            .Or(new HumanByLastNameWhereSpec(Keyword)))
                             .Select(human =&gt; new Response
                                                  {
                                                          Id = human.Id,
                                                          Birthday = human.Birthday.ToShortDateString(),
                                                          FirstName = human.FirstName,
                                                          LastName = human.LastName,
                                                          Sex = human.Sex.ToString()
                                                  }).ToList();
        }
    }
}</pre>
Finally, it only remains to modify Index.cshtml in order to add a search box, which uses a Keyword field for data cleaning while a request is being processed.
<h6>Index.cshtml</h6>
<pre class="lang:c# decode:true">@using Example.Domain
@using Incoding.MetaLanguageContrib
@using Incoding.MvcContrib
@{
    Layout = "~/Views/Shared/_Layout.cshtml";
}
&lt;div id="dialog"&gt;&lt;/div&gt;
@*При нажатии кнопки Find инициируется событие InitIncoding и PeopleTable выполняет запрос GetPeopleQuery с параметром Keyword*@
&lt;div&gt;
    &lt;input type="text" id="Keyword"/&gt;
    @(Html.When(JqueryBind.Click)
          .Direct()
          .OnSuccess(dsl =&gt; dsl.WithId("PeopleTable").Core().Trigger.Incoding())
          .AsHtmlAttributes()
          .ToButton("Find"))
&lt;/div&gt;

@(Html.When(JqueryBind.InitIncoding)
      .AjaxGet(Url.Dispatcher().Query(new GetPeopleQuery { Keyword = Selector.Jquery.Id("Keyword") }).AsJson())
      .OnSuccess(dsl =&gt; dsl.Self().Core().Insert.WithTemplateByUrl(Url.Dispatcher().AsView("~/Views/Home/HumanTmpl.cshtml")).Html())
      .AsHtmlAttributes(new { id = "PeopleTable" })
      .ToDiv())

@(Html.When(JqueryBind.Click)
      .AjaxGet(Url.Dispatcher().AsView("~/Views/Home/AddOrEditHuman.cshtml"))
      .OnSuccess(dsl =&gt; dsl.WithId("dialog").Behaviors(inDsl =&gt;
                                                           {
                                                               inDsl.Core().Insert.Html();
                                                               inDsl.JqueryUI().Dialog.Open(option =&gt;
                                                                                                {
                                                                                                    option.Resizable = false;
                                                                                                    option.Title = "Add human";
                                                                                                });
                                                           }))
      .AsHtmlAttributes()
      .ToButton("Add new human"))</pre>
<h1>Part 5. Unit-test.</h1>
Let’s cover the written code with tests. The first one is responsible for testing of Human entity mapping. Add the file When_save_Human.cs to the folder Persisteces of the UnitTests project.
<h6><b>When_save_Human.cs</b></h6>
<pre class="lang:c# decode:true">namespace Example.UnitTests.Persistences
{
    #region &lt;&lt; Using &gt;&gt;

    using Example.Domain;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(Human))]
    public class When_save_Human : SpecWithPersistenceSpecification
    {
        #region Fields

        It should_be_verify = () =&gt; persistenceSpecification.VerifyMappingAndSchema();

        #endregion
    }
}</pre>
The test works with a test database (Example_test): an instance of the Human class with automatically populated fields is created, then stored in the DB, retrieved from and compared to the created instance.

Then add the tests for WhereSpecifications in a folder named Specifications.
<h6><strong>When_human_by_first_name.cs</strong></h6>
<pre class="lang:c# decode:true ">namespace Example.UnitTests.Specifications
{
    #region &lt;&lt; Using &gt;&gt;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Example.Domain;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(HumanByFirstNameWhereSpec))]
    public class When_human_by_first_name
    {
        #region Fields

        Establish establish = () =&gt;
                                  {
                                      Func&lt;string, Human&gt; createEntity = (firstName) =&gt;
                                                                         Pleasure.MockStrictAsObject(mock =&gt;
                                                                                                            mock.SetupGet(r =&gt; r.FirstName)
                                                                                                                .Returns(firstName));

                                      fakeCollection = Pleasure.ToQueryable(createEntity(Pleasure.Generator.TheSameString()),
                                                                            createEntity(Pleasure.Generator.String()));
                                  };

        Because of = () =&gt;
                         {
                             filterCollection = fakeCollection
                                     .Where(new HumanByFirstNameWhereSpec(Pleasure.Generator.TheSameString()).IsSatisfiedBy())
                                     .ToList();
                         };

        It should_be_filter = () =&gt;
                                  {
                                      filterCollection.Count.ShouldEqual(1);
                                      filterCollection[0].FirstName.ShouldBeTheSameString();
                                  };

        #endregion

        #region Establish value

        static IQueryable fakeCollection;

        static List filterCollection;

        #endregion
    }
}</pre>
<h6><strong>When_human_by_last_name.cs</strong></h6>
<pre class="lang:c# decode:true">namespace Example.UnitTests.Specifications
{
    #region &lt;&lt; Using &gt;&gt;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Example.Domain;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(HumanByLastNameWhereSpec))]
    public class When_human_by_last_name
    {
        #region Fields

        Establish establish = () =&gt;
                                  {
                                      Func&lt;string, Human&gt; createEntity = (lastName) =&gt;
                                                                         Pleasure.MockStrictAsObject(mock =&gt;
                                                                                                            mock.SetupGet(r =&gt; r.LastName)
                                                                                                                .Returns(lastName));

                                      fakeCollection = Pleasure.ToQueryable(createEntity(Pleasure.Generator.TheSameString()),
                                                                            createEntity(Pleasure.Generator.String()));
                                  };

        Because of = () =&gt;
                         {
                             filterCollection = fakeCollection
                                     .Where(new HumanByLastNameWhereSpec(Pleasure.Generator.TheSameString()).IsSatisfiedBy())
                                     .ToList();
                         };

        It should_be_filter = () =&gt;
                                  {
                                      filterCollection.Count.ShouldEqual(1);
                                      filterCollection[0].LastName.ShouldBeTheSameString();
                                  };

        #endregion

        #region Establish value

        static IQueryable fakeCollection;

        static List filterCollection;

        #endregion
    }
}</pre>
Now we have to add tests for the command and the query (Operations folder). For the command, you need to add two tests: the first one verifies the creation of a new entity; the second one verifies the editing of an existing entity.
<h6><strong>When_get_people_query.cs</strong></h6>
<pre class="lang:c# decode:true">namespace Example.UnitTests.Operations
{
    #region &lt;&lt; Using &gt;&gt;

    using System.Collections.Generic;
    using Example.Domain;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(GetPeopleQuery))]
    public class When_get_people
    {
        #region Fields

        Establish establish = () =&gt;
                                  {
                                      var query = Pleasure.Generator.Invent&lt;GetPeopleQuery&gt;();
                                      //Create entity for test with auto-generate
                                      human = Pleasure.Generator.Invent&lt;Human&gt;();

                                      expected = new List&lt;GetPeopleQuery.Response&gt;();

                                      mockQuery = MockQuery&lt;GetPeopleQuery, List&lt;GetPeopleQuery.Response&gt;&gt;
                                              .When(query)
                                              //"Stub" on query to repository
                                              .StubQuery(whereSpecification: new HumanByFirstNameWhereSpec(query.Keyword)
                                                                 .Or(new HumanByLastNameWhereSpec(query.Keyword)),
                                                         entities: human);
                                  };

        Because of = () =&gt; mockQuery.Original.Execute();
        
        // Compare result 
        It should_be_result = () =&gt; mockQuery.ShouldBeIsResult(list =&gt; list.ShouldEqualWeakEach(new List&lt;Human&gt;() { human },
                                                                                                (dsl, i) =&gt; dsl.ForwardToValue(r =&gt; r.Birthday, human.Birthday.ToShortDateString())
                                                                                                               .ForwardToValue(r =&gt; r.Sex, human.Sex.ToString())
                                                                               ));

        #endregion

        #region Establish value

        static MockMessage&lt;GetPeopleQuery, List&lt;GetPeopleQuery.Response&gt;&gt; mockQuery;

        static List&lt;GetPeopleQuery.Response&gt; expected;

        static Human human;

        #endregion
    }
}</pre>
<h6><strong>When_add_human.cs</strong></h6>
<pre class="lang:c# decode:true">namespace Example.UnitTests.Operations
{
    #region &lt;&lt; Using &gt;&gt;

    using Example.Domain;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(AddOrEditHumanCommand))]
    public class When_add_human
    {
        #region Fields

        Establish establish = () =&gt;
                                  {
                                      var command = Pleasure.Generator.Invent&lt;AddOrEditHumanCommand&gt;();

                                      mockCommand = MockCommand&lt;AddOrEditHumanCommand&gt;
                                              .When(command)
                                              //"Stub" on repository
                                              .StubGetById&lt;Human&gt;(command.Id, null);
                                  };

        Because of = () =&gt; mockCommand.Original.Execute();

        It should_be_saved = () =&gt; mockCommand.ShouldBeSaveOrUpdate&lt;Human&gt;(human =&gt; human.ShouldEqualWeak(mockCommand.Original));

        #endregion

        #region Establish value

        static MockMessage&lt;AddOrEditHumanCommand, object&gt; mockCommand;

        #endregion
    }
}</pre>
<h6><strong>When_edit_human.cs</strong></h6>
<pre class="lang:c# decode:true">namespace Example.UnitTests.Operations
{
    #region &lt;&lt; Using &gt;&gt;

    using Example.Domain;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(AddOrEditHumanCommand))]
    public class When_edit_human
    {
        #region Fields

        Establish establish = () =&gt;
                                  {
                                      var command = Pleasure.Generator.Invent&lt;AddOrEditHumanCommand&gt;();

                                      human = Pleasure.Generator.Invent&lt;Human&gt;();

                                      mockCommand = MockCommand&lt;AddOrEditHumanCommand&gt;
                                              .When(command)
                                              //"Stub" on repository
                                              .StubGetById(command.Id, human);
                                  };

        Because of = () =&gt; mockCommand.Original.Execute();

        It should_be_saved = () =&gt; mockCommand.ShouldBeSaveOrUpdate&lt;Human&gt;(human =&gt; human.ShouldEqualWeak(mockCommand.Original));

        #endregion

        #region Establish value

        static MockMessage&lt;AddOrEditHumanCommand, object&gt; mockCommand;

        static Human human;

        #endregion
    }
}</pre>
<h1>Study materials</h1>
<ol>
	<li><a title="Cqrs vs N-layer" href="http://blog.incframework.com/cqrs-vs-n-layer/">CQRS </a> and <a title="CQRS advanced course" href="http://blog.incframework.com/cqrs-advanced-course/">CQRS </a>(advanced course) , <a title="Repository" href="http://blog.incframework.com/repository/">Repository </a>- back end architecture</li>
	<li><a title="Blog: MVD" href="http://blog.incframework.com/ru/model-view-dispatcher/">MVD</a> -a description of a Model View Dispatcher pattern</li>
	<li><a title="IML TODO" href="http://blog.incframework.com/iml-todo/">IML </a>(TODO), <a title="AngularJs vs IML" href="http://blog.incframework.com/angularjs-vs-iml/">IML vs Angular</a> , <a title="Jquery vs IML" href="http://blog.incframework.com/jqyery-style-vs-iml-style/">Iml vs Jquery</a> , <a title="Ajax.ActionLink vs IML" href="http://blog.incframework.com/ajax-actionlink-vs-iml/">Iml vs ASP.NET Ajax</a> - incoding meta language</li>
	<li><a title="Blog: Мощь селекторов" href="http://blog.incframework.com/ru/power-selector/">IML</a> (selector)-  a description of the selectors’ usage in IML</li>
	<li><a title="Do,Action,Insert" href="http://blog.incframework.com/do-action-insert/">IML In Ajax</a> - a description of the IML Operation in relation to Ajax</li>
	<li><a title="Client template" href="http://blog.incframework.com/client-template/">IML template</a> - Templates for data insertion</li>
	<li><a title="Extensions" href="http://blog.incframework.com/extensions/">Extensions</a>- help with writing extensions to  comply the <a title="Wiki: DRY" href="https://en.wikipedia.org/wiki/Don%27t_repeat_yourself"><strong>D</strong>on't<strong>R</strong>epeat<strong>Y</strong>ourself</a> principle</li>
	<li><a title="Inc testing" href="http://blog.incframework.com/inc-testing/">Unit Test</a> and <a title="Command and query test scenario" href="http://blog.incframework.com/command-test-scenario/">Unit test scenario</a></li>
</ol>
