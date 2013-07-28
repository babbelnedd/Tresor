namespace Tresor.Utilities.Mediator
{
    using System.Collections.Generic;
    using System.Linq;

    using Tresor.Contracts.Mediator;
    using Tresor.Contracts.Utilities;

    /// <summary>Implementierung des Mediator Pattern für <see cref="IPassword"/>.</summary>
    public class PasswordMediator
    {
        #region Fields

        /// <summary>Holt alle Empfänger.</summary>
        private readonly List<IPasswordSender> reciever = new List<IPasswordSender>();

        #endregion

        #region Public Methods and Operators

        /// <summary>Fügt dem Mediator einen Sender/Empfänger hinzu.</summary>
        /// <param name="item">Der Sender/Empfänger welcher hinzugefügt werden soll.</param>
        public void Add(IPasswordSender item)
        {
            if (!reciever.Contains(item))
            {
                reciever.Add(item);
            }
        }

        /// <summary>Sendet eine Nachricht an ein anderes Viewmodel.</summary>
        /// <param name="sender">Das ViewModel welche die Nachricht verschickt hat..</param>
        /// <param name="message">Die Nachricht die gesendet werden soll.</param>
        public void Send(IPasswordSender sender, IPassword message)
        {
            reciever.Where(item => item != sender).ToList().ForEach(item => ((IPasswordReciever)item).Recieve(message));
        }

        #endregion
    }
}