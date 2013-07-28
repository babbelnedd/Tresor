namespace Tresor.UnitTesting.Tresor.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using NUnit.Framework;

    using global::Tresor.Contracts.Model;

    using global::Tresor.Contracts.Utilities;

    using global::Tresor.Model;

    using global::Tresor.Utilities;

    using global::Tresor.ViewModel.Application;

    /// <summary>Unit Tests für das SplashViewModel.</summary>
    [TestFixture(Description = "Unit Tests für das SplashViewModel.")]
    public class SplashViewModelTests : Test
    {
        #region Fields

        /// <summary>Das Datenmodel.</summary>
        private IPanelModel model;

        /// <summary>Das ViewModel für die Unit Tests.</summary>
        private SplashViewModel viewModel;

        #endregion

        #region Public Methods and Operators

        /// <summary>Erweitert das Basisverhalten um die Erzeugung des ViewModels.</summary>
        public override void Initalize()
        {
            base.Initalize();
            var database = Guid.NewGuid().ToString();
            model = new SqlitePanelModel(string.Format("{0}.db", database));
            viewModel = new SplashViewModel(model);
        }

        /// <summary>Prüft ob die Eigenschaft KeyIsCorrect beim Durchlaufen des Setters das PropertyChanged Ereignis auslöst.</summary>
        [Test(Description = "Prüft ob die Eigenschaft KeyIsCorrect beim Durchlaufen des Setters das PropertyChanged Ereignis auslöst.")]
        [Repeat(Tests)]
        public void KeyIsCorrectRaisesPropertyChanged()
        {
            viewModel.Input = new Random().Next(9, 9999999).ToString();

            var changes = new List<string>();
            viewModel.PropertyChanged += (sender, arguments) => changes.Add(arguments.PropertyName);

            viewModel.CheckInputCommand.Execute(null);

            Assert.IsNotEmpty(changes);
            Assert.IsTrue(changes.Count == 1);
            Assert.IsTrue(changes[0] == "KeyIsCorrect");
        }

        /// <summary>Prüft ob die Eigenschaft KeyIsCorrect false bei falschem Schlüssel zurückgibt.</summary>
        [Test(Description = "Prüft ob die Eigenschaft KeyIsCorrect false bei falschem Schlüssel zurückgibt.")]
        [Repeat(Tests)]
        public void KeyIsCorrectReturnsFalseOnWrongKey()
        {
            model.IsKeyCorrect("5");
            model.Save(new ObservableCollection<IPassword> { new Password { RecordID = Guid.NewGuid() } }, "5");

            viewModel.Input = "6";
            viewModel.CheckInputCommand.Execute(null);
            Assert.IsFalse(viewModel.KeyIsCorrect);
        }

        /// <summary>Prüft ob die Eigenschaft KeyIsCorrect nach Initalisierung des ViewModels true zurückgibt.</summary>
        [Test(Description = "Prüft ob die Eigenschaft KeyIsCorrect nach Initalisierung des ViewModels true zurückgibt.")]
        [Repeat(Tests)]
        public void KeyIsCorrectReturnsTrueOnStartup()
        {
            Assert.IsTrue(viewModel.KeyIsCorrect);
        }

        #endregion
    }
}