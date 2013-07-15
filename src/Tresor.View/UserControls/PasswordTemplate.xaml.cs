namespace Tresor.View.UserControls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Tresor.Utilities;

    /// <summary>Interaktionslogik für PasswordTemplate.xaml.</summary>
    public partial class PasswordTemplate
    {
        #region Konstruktoren und Destruktoren

        /// <summary>Initialisiert eine neue Instanz der <see cref="PasswordTemplate"/> Klasse.</summary>
        public PasswordTemplate()
        {
            InitializeComponent();
        }

        #endregion

        #region Methoden

        /// <summary>Defokussiert eine <see cref="System.Windows.Controls.TextBox"/>.</summary>
        /// <param name="sender">Erwartet die <see cref="System.Windows.Controls.TextBox"/>.</param>
        private static void Defocus(object sender)
        {
            Keyboard.Focus(null);
            ((TextBox)sender).IsReadOnly = true;
        }

        /// <summary>Tritt ein wenn der Öffnen bzw Schließen Schalter gedrückt wurde.</summary>
        /// <param name="sender">Erwartet einen <see cref="System.Windows.Controls.Button"/> um die Eigenschaft Content zu bearbeiten.</param>
        /// <param name="arguments">Dieser Parameter wird nicht verwendet.</param>
        private void OnClick(object sender, RoutedEventArgs arguments)
        {
            MoreInformationGrid.Visibility = MoreInformationGrid.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            ((Button)sender).Content = ((Button)sender).Content.ToString() == "+" ? "-" : "+";
        }

        /// <summary>Tritt ein wenn ein Button doppelt geklickt wurde.</summary>
        /// <param name="sender">Erwartet einen <see cref="System.Windows.Controls.Button"/> dessen Content eine <see cref="System.Windows.Controls.TextBox"/> ist.</param>
        /// <param name="arguments">Dieser Parameter wird nicht verwendet.</param>
        private void OnTextBoxDoubleClick(object sender, MouseButtonEventArgs arguments)
        {
            ((TextBox)((Button)sender).Content).IsReadOnly = false;
            ((Password)DataContext).BeginEdit();
        }

        /// <summary>Tritt ein wenn in der Accountbearbeitung eine Taste gedrückt wurde.</summary>
        /// <param name="sender">Erwartet eine <see cref="System.Windows.Controls.TextBox"/>.</param>
        /// <param name="arguments">Überprüft anhand der Eigenschaft Key welche Taste gedrückt wurde.</param>
        private void OnTextBoxKeyDown(object sender, KeyEventArgs arguments)
        {
            if (arguments.Key == Key.Enter)
            {
                Defocus(sender);
                ((Password)DataContext).EndEdit();
            }

            if (arguments.Key == Key.Escape)
            {
                Defocus(sender);
                ((Password)DataContext).CancelEdit();
            }
        }

        /// <summary>Tritt ein wenn eine TextBox den Fokus verliert.</summary>
        /// <param name="sender">Erwartet eine <see cref="System.Windows.Controls.TextBox"/>.</param>
        /// <param name="arguments">Dieser Parameter wird nicht verwendet.</param>
        private void OnTextBoxLostFocus(object sender, RoutedEventArgs arguments)
        {
            ((TextBox)sender).IsReadOnly = true;
            ((Password)DataContext).EndEdit();
        }

        #endregion
    }
}