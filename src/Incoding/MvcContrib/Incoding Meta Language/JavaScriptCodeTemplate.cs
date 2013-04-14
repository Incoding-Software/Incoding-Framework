namespace Incoding.MvcContrib
{
    public static class JavaScriptCodeTemplate
    {
        #region Constants
        
        public const string Target_Increment = "$(this.target).val(parseInt($(this.target).val()) + 1);";

        public const string Target_Decrement = "$(this.target).val(parseInt($(this.target).val()) - 1);";

        public const string Target_UnBind = "$(this.target).unbind({0});";

        public const string Target_Remove = "$(this.target).remove();";

        public const string Target_Empty = "$(this.target).empty();";

        public const string Target_Detach = "$(this.target).detach();";

        public const string Target_Wrap = "$(this.target).wrap(this.tryGetVal({0}));";

        public const string Target_WrapAll = "$(this.target).wrapAll(this.tryGetVal({0}));";

        public const string Target_Insert = "$(this.target).{0}(this.tryGetVal({1}).toString());";

        public const string Target_RemoveClass = "$(this.target).removeClass('{0}');";

        public const string Target_ToggleClass = "$(this.target).toggleClass('{0}');";

        public const string Target_AddClass = "$(this.target).addClass('{0}');";

        public const string Target_ToggleProp = "$(this.target).toggleProp('{0}');";

        public const string Target_Val = "$(this.target).val({0});";

        public const string Target_ValFromSelector = "$(this.target).val(this.tryGetVal({0}));";

        public const string Target_SetAttr = "$(this.target).attr('{0}', this.tryGetVal({1}));";

        public const string Target_RemoveAttr = "$(this.target).removeAttr('{0}');";

        public const string Target_SetProp = "$(this.target).prop('{0}', this.tryGetVal({1}));";

        public const string Target_RemoveProp = "$(this.target).removeProp('{0}');";

        public const string Target_SetCss = "$(this.target).css('{0}', this.tryGetVal({1}));";

        public const string Target_Height = "$(this.target).height(this.tryGetVal({0}));";

        public const string Target_ScrollLeft = "$(this.target).scrollLeft(this.tryGetVal({0}));";

        public const string Target_ScrollTop = "$(this.target).scrollTop(this.tryGetVal({0}));";

        public const string Target_Width = "$(this.target).width(this.tryGetVal({0}));";

        public const string Target_ReplaceWith = "$(this.target).replaceWith(this.tryGetVal({0}));";

        public const string Document_SetTitle = "document.title = '{0}';";

        public const string Document_HistoryGo = "history.go({0});";

        public const string Conditional_Value = "ExecutableHelper.Compare(this.tryGetVal({0}), this.tryGetVal({1}), '{2}');";

        public const string Conditional_Confirm = "window.confirm(this.tryGetVal({0}));";

        public const string Conditional_Is_Valid_Form = "{0}.valid();";

        public const string Conditional_ModernizrSupport = "$('html').hasClass('{0}');";

        public const string Conditional_Exists_Jquery_Selector = "{0}.length != 0;";

        public const string Conditional_Exists_Incoding_Selector = "!ExecutableHelper.IsNullOrEmpty(this.tryGetVal({0}))";

        public const string Window_Alert = "window.alert('{0}');";

        public const string Window_Location_Reload = "window.location.reload({0});";

        public const string Window_Clear_Interval = "window.clearInterval(ExecutableBase.IntervalIds['{0}'])";

        #endregion
    }
}