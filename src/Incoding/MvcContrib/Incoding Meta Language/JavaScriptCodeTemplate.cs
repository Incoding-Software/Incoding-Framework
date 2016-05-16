namespace Incoding.MvcContrib
{
    public static class JavaScriptCodeTemplate
    {
        #region Constants

        public const string Target_Increment = "$(this.target).increment({0});";
        
        public const string Target_Jquery_Plug_In = "$(this.target).{0}({1});";

        public const string Target_Val = "$(this.target).val({0});";
        
        public const string Window_Location_Reload = "window.location.reload({0});";

        public const string Window_Clear_Interval = "window.clearInterval(ExecutableBase.IntervalIds['{0}'])";

        #endregion
    }
}