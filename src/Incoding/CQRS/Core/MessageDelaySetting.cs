namespace Incoding.CQRS
{
    #region << Using >>

    using Incoding.Block.ExceptionHandling;

    #endregion

    public class MessageDelaySetting
    {
        #region Constructors

        public MessageDelaySetting()
        {
            Policy = ActionPolicy.Direct();
        }

        #endregion

        #region Properties

        public ActionPolicy Policy { get; set; }

        public string DataBaseInstance { get; set; }

        public string Connection { get; set; }

        public string UID { get; set; }

        #endregion
    }
}