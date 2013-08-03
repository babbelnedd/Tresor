namespace Tresor.Resources.Converter
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    using Tresor.Contracts.Utilities;

    /// <summary>Konvertiert ein IPassword oder ein IEnumerable vom Typ IPassword in eine Visibility.</summary>
    public class ContentIsDirtyToVisibility : IValueConverter
    {
        #region Public Methods and Operators

        /// <summary>Konvertiert einen Wert.</summary>
        /// <returns>Ein konvertierter Wert. Wenn die Methode null zurückgibt, wird der gültige NULL-Wert verwendet.</returns>
        /// <param name="value">Der von der Bindungsquelle erzeugte Wert.</param><param name="targetType">Der Typ der Bindungsziel-Eigenschaft.</param><param name="parameter">Der zu verwendende Konverterparameter.</param><param name="culture">Die im Konverter zu verwendende Kultur.</param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IPassword)
            {
                return ((IPassword)value).IsDirty ? Visibility.Visible : Visibility.Collapsed;
            }

            if (value is IEnumerable<IPassword>)
            {
                return Visibility.Collapsed;
            }

            throw new ArgumentException("value");
        }

        /// <summary>Konvertiert einen Wert.</summary>
        /// <returns>Ein konvertierter Wert. Wenn die Methode null zurückgibt, wird der gültige NULL-Wert verwendet.</returns>
        /// <param name="value">Der Wert, der vom Bindungsziel erzeugt wird.</param><param name="targetType">Der Typ, in den konvertiert werden soll.</param><param name="parameter">Der zu verwendende Konverterparameter.</param><param name="culture">Die im Konverter zu verwendende Kultur.</param>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        #endregion
    }
}