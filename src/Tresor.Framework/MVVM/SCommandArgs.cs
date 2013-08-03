namespace Tresor.Framework.MVVM
{
    #region SCommandArgs Class

    /// <summary>Allows a CommandParameter to be associated with a SingleEventCommand.</summary>
    public class SCommandArgs
    {
        #region Constructors and Destructors

        /// <summary>Initialisiert eine neue Instanz der <see cref="SCommandArgs"/> Klasse.</summary>
        public SCommandArgs()
        {
        }

        public SCommandArgs(object sender, object eventArgs, object commandParameter)
        {
            Sender = sender;
            EventArgs = eventArgs;
            CommandParameter = commandParameter;
        }

        #endregion

        #region Public Properties

        public object CommandParameter { get; set; }

        public object EventArgs { get; set; }

        public object Sender { get; set; }

        #endregion
    }

    #endregion
}