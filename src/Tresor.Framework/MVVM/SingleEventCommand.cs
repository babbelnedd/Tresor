namespace Tresor.Framework.MVVM
{
    using System;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>This class allows a single command to event mappings. It is used to wire up View events to a ViewModel ICommand implementation.</summary>
    /// <example>
    /// <![CDATA[<ListBox ...    
    /// Cinch:SingleEventCommand.RoutedEventName="SelectionChanged"     
    /// Cinch:SingleEventCommand.TheCommandToRun="{Binding Path=BoxEditCommand}"     
    /// Cinch:SingleEventCommand.CommandParameter=
    ///         "{Binding ElementName=ListBoxVehicle, Path=SelectedItem}">
    /// </ListBox>]]>
    /// </example>
    public static class SingleEventCommand
    {
        #region Static Fields

        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(SingleEventCommand), new UIPropertyMetadata(null));

        /// <summary>
        /// RoutedEventName : The event that should actually execute the
        /// ICommand
        /// </summary>
        public static readonly DependencyProperty RoutedEventNameProperty = DependencyProperty.RegisterAttached("RoutedEventName", typeof(String), typeof(SingleEventCommand), new FrameworkPropertyMetadata((String)string.Empty, new PropertyChangedCallback(OnRoutedEventNameChanged)));

        /// <summary>
        /// TheCommandToRun : The actual ICommand to run
        /// </summary>
        public static readonly DependencyProperty TheCommandToRunProperty = DependencyProperty.RegisterAttached("TheCommandToRun", typeof(ICommand), typeof(SingleEventCommand), new FrameworkPropertyMetadata((ICommand)null));

        #endregion

        #region Public Methods and Operators

        /// <summary>Gets the CommandParameter property.</summary>
        public static object GetCommandParameter(DependencyObject obj)
        {
            return (object)obj.GetValue(CommandParameterProperty);
        }

        /// <summary>
        /// Gets the RoutedEventName property. 
        /// </summary>
        public static string GetRoutedEventName(DependencyObject d)
        {
            return (String)d.GetValue(RoutedEventNameProperty);
        }

        /// <summary>
        /// Gets the TheCommandToRun property. 
        /// </summary>
        public static ICommand GetTheCommandToRun(DependencyObject d)
        {
            return (ICommand)d.GetValue(TheCommandToRunProperty);
        }

        /// <summary>Sets the CommandParameter property.</summary>
        public static void SetCommandParameter(DependencyObject obj, object value)
        {
            obj.SetValue(CommandParameterProperty, value);
        }

        /// <summary>
        /// Sets the RoutedEventName property. 
        /// </summary>
        public static void SetRoutedEventName(DependencyObject d, string value)
        {
            d.SetValue(RoutedEventNameProperty, value);
        }

        /// <summary>
        /// Sets the TheCommandToRun property. 
        /// </summary>
        public static void SetTheCommandToRun(DependencyObject d, ICommand value)
        {
            d.SetValue(TheCommandToRunProperty, value);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Hooks up a Dynamically created EventHandler (by using the 
        /// <see cref="EventHooker">EventHooker</see> class) that when
        /// run will run the associated ICommand
        /// </summary>
        private static void OnRoutedEventNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            string routedEvent = (String)e.NewValue;

            if (d == null || string.IsNullOrEmpty(routedEvent))
            {
                return;
            }

            // If the RoutedEvent string is not null, create a new
            // dynamically created EventHandler that when run will execute
            // the actual bound ICommand instance (usually in the ViewModel)
            EventHooker eventHooker = new EventHooker();
            eventHooker.ObjectWithAttachedCommand = d;

            EventInfo eventInfo = d.GetType().GetEvent(routedEvent, BindingFlags.Public | BindingFlags.Instance);

            // Hook up Dynamically created event handler
            if (eventInfo != null)
            {
                eventInfo.RemoveEventHandler(d, eventHooker.GetNewEventHandlerToRunCommand(eventInfo));

                eventInfo.AddEventHandler(d, eventHooker.GetNewEventHandlerToRunCommand(eventInfo));
            }
        }

        #endregion
    }
}