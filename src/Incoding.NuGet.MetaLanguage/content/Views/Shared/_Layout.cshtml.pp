@using Incoding.MvcContrib
<!DOCTYPE html>
<html >
    <head>                 
        <script type="text/javascript" src="@Url.Content("~/Scripts/jquery-1.8.0.min.js")"> </script>
        <script type="text/javascript" src="@Url.Content("~/Scripts/underscore.min.js")"> </script>                
        <script type="text/javascript" src="@Url.Content("~/Scripts/jquery.form.min.js")"> </script>
        <script type="text/javascript" src="@Url.Content("~/Scripts/jquery.history.js")"> </script>        
        <script type="text/javascript" src="@Url.Content("~/Scripts/jquery.validate.min.js")"> </script>
        <script type="text/javascript" src="@Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js")"> </script>
        <script type="text/javascript" src="@Url.Content("~/Scripts/handlebars-1.1.2.js")"> </script>
        <script type="text/javascript" src="@Url.Content("~/Scripts/incoding.framework.min.js")"> </script>                      
        <script>
            TemplateFactory.Version = '@Guid.NewGuid().ToString()';
        </script>
    </head>
    @Html.Incoding().RenderDropDownTemplate()
    <body>        
        @RenderBody()
    </body>
</html>
