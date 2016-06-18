#region copyright

// @incoding 2011

#endregion

namespace Incoding.CQRS
{
    /// <summary>
    ///     Interface Dispatcher
    /// </summary>
    public interface IDispatcher
    {
        void Push(CommandComposite composite);
        
        TResult Query<TResult>(QueryBase<TResult> message, MessageExecuteSetting executeSetting = null);
    }
}