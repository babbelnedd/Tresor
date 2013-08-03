namespace Tresor.Framework.MVVM
{
    using System;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>Contains the event that is hooked into the source RoutedEvent that was specified to run the ICommand.</summary>
    internal sealed class EventHooker
    {
        #region Public Properties

        /// <summary>The DependencyObject, that holds a binding to the actual ICommand to execute.</summary>
        public DependencyObject ObjectWithAttachedCommand { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>Creates a Dynamic EventHandler that will be run the ICommand when the user specified RoutedEvent fires.</summary>
        /// <param name="eventInfo">The specified RoutedEvent EventInfo.</param>
        /// <returns>An Delegate that points to a new EventHandler that will be run the ICommand.</returns>
        public Delegate GetNewEventHandlerToRunCommand(EventInfo eventInfo)
        {
            Delegate del = null;

            if (eventInfo == null)
            {
                throw new ArgumentNullException("eventInfo");
            }

            if (eventInfo.EventHandlerType == null)
            {
                throw new ArgumentException("EventHandlerType is null");
            }

            if (del == null)
            {
                del = Delegate.CreateDelegate(eventInfo.EventHandlerType, this, GetType().GetMethod("OnEventRaised", BindingFlags.NonPublic | BindingFlags.Instance));
            }

            return del;
        }

        #endregion

        #region Methods

        /// <summary>Runs the ICommand when the requested RoutedEvent fires.</summary>
        private void OnEventRaised(object sender, EventArgs e)
        {
            ICommand command = (ICommand)(sender as DependencyObject).GetValue(SingleEventCommand.TheCommandToRunProperty);

            object commandParameter = (sender as DependencyObject).GetValue(SingleEventCommand.CommandParameterProperty);

            SCommandArgs commandArgs = new SCommandArgs(sender, e, commandParameter);
            if (command != null)
            {
                command.Execute(commandArgs);
            }
        }

        #endregion
    }
}