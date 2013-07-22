namespace Tresor.View.Application
{
    using System.Windows;
    using System.Windows.Controls;

    using Tresor.Contracts.ViewModel.Application;

    /// <summary>Interaktionslogik für SplashView.xaml.</summary>
    public partial class SplashView
    {
        #region Konstruktoren und Destruktoren

        /// <summary>Initialisiert eine neue Instanz der <see cref="SplashView"/> Klasse.</summary>
        public SplashView()
        {
            InitializeComponent();
        }

        #endregion

        #region Methoden

        /// <summary>Tritt ein wenn sich die Eingabe einer PasswordBox geändert hat.</summary>
        /// <param name="sender">Erwartet eine <see cref="PasswordBox"/>.</param>
        /// <param name="arguments">Dieser Parameter wird nicht verwendet.</param>
        /// <remarks>An PasswordBox kann nicht direkt gebunden werden. Deshalb dieser kleine Umweg.</remarks>
        private void PasswordChanged(object sender, RoutedEventArgs arguments)
        {
            var pwdBox = (PasswordBox)sender;
            ((ISplashViewModel)DataContext).Input = pwdBox.Password;
        }

        #endregion
    }
}