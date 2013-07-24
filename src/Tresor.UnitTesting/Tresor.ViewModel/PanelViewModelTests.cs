namespace Tresor.UnitTesting.Tresor.ViewModel
{
    using System;
    using System.Collections.Generic;

    using NUnit.Framework;

    using global::Tresor.Contracts.Model;
    using global::Tresor.Contracts.ViewModel;

    using global::Tresor.Model;
    using global::Tresor.Utilities;
    using global::Tresor.ViewModel;

    /// <summary>Unit Tests für das PanelViewModel.</summary>
    [TestFixture(Description = "Unit Tests für das PanelViewModel.")]
    public class PanelViewModelTests : ViewModelTest
    {
        #region Konstanten und Felder

        /// <summary>Das PanelModel.</summary>
        private IPanelModel model;

        /// <summary>Das PanelViewModel.</summary>
        private IPanelViewModel viewModel;

        #endregion

        #region Öffentliche Methoden und Operatoren

        /// <summary>Erweitert das Basisverhalten um die Initialisierung des ViewModels.</summary>
        public override void Initalize()
        {
            base.Initalize();
            var database = Guid.NewGuid().ToString();
            model = new SqlitePanelModel(string.Format("{0}.db", database));
            model.IsKeyCorrect(string.Empty);
            viewModel = new PanelViewModel(model);
        }

        /// <summary>Prüft ob IsDirty false zu beginn ist.</summary>
        [Test(Description = "Prüft ob IsDirty false zu beginn ist.")]
        [Repeat(Tests)]
        public void IsDirtyIsFalseOnStartup()
        {
            Assert.IsFalse(viewModel.IsDirty);
        }

        /// <summary>Prüft, dass die Eigenschaft Passwords nie null ist.</summary>
        [Test(Description = "Prüft, dass die Eigenschaft Passwords nie null ist.")]
        [Repeat(20)]
        public void PasswordsIsNotNull()
        {
            Assert.IsNotNull(viewModel.Passwords);
        }

        /// <summary>Prüft ob PropretyChanged für IsDirty ausgelöst wird, wenn das Model geändert wurde.</summary>
        [Test(Description = "Prüft ob PropretyChanged für Passwords und IsDirty ausgelöst wird, wenn das Model geändert wurde.")]
        [Repeat(Tests)]
        public void RaiseOnPropertyChangedPasswordsIfModelChanges()
        {
            var changes = new List<string>();
            viewModel.PropertyChanged += (sender, arguments) => changes.Add(arguments.PropertyName);
            model.AddPassword(new Password { RecordID = Guid.NewGuid() });
            Assert.IsNotEmpty(changes);
            Assert.IsTrue(changes.Count == 2);
            Assert.IsTrue(changes.Contains("IsDirty"));
            Assert.IsTrue(changes.Contains("Passwords"));
        }

        #endregion
    }
}