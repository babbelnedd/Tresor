﻿namespace Tresor.UnitTesting.Tresor.Model
{
    using global::Tresor.Contracts.Model;

    using global::Tresor.Model;

    /// <summary>Erweiterter <see cref="Test"/>.</summary>
    public abstract class ModelTest : Test
    {
        #region Eigenschaften

        /// <summary>Holt oder setzt das <see cref="PanelModel"/>.</summary>
        internal IPanelModel Model { get; set; }

        #endregion

        #region Öffentliche Methoden und Operatoren

        /// <summary>Erweitert das Basisverhalten um die Erzeugung des <see cref="PanelModel"/>.</summary>
        public override void Initalize()
        {
            base.Initalize();
            Model = new PanelModel();
        }

        #endregion
    }
}