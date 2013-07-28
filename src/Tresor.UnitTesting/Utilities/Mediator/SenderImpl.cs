namespace Tresor.UnitTesting.Utilities.Mediator
{
    using global::Tresor.Contracts.Mediator;

    using global::Tresor.Contracts.Utilities;

    using global::Tresor.Utilities.Mediator;

    public class Sender1 : IPasswordSender, IPasswordReciever
    {
        #region Fields

        public bool? result;

        internal PasswordMediator mediator;

        #endregion

        #region Constructors and Destructors

        /// <summary> Initialisiert eine neue Instanz der <see cref="Sender1"/> Klasse. </summary>
        public Sender1(PasswordMediator mediator)
        {
            this.mediator = mediator;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>Empfängt ein <see cref="IPassword"/>.</summary>
        /// <param name="password">Das <see cref="IPassword"/> welches empfangen wird.</param>
        public void Recieve(IPassword password)
        {
            result = true;
        }

        /// <summary>Sendet ein <see cref="IPassword"/>. </summary>
        /// <param name="password">Das <see cref="IPassword"/> welches verschickt wird.</param>
        public void Send(IPassword password)
        {
            mediator.Send(this, password);
        }

        #endregion
    }

    public class Sender2 : IPasswordSender, IPasswordReciever
    {
        #region Fields

        public bool? result;

        internal PasswordMediator mediator;

        #endregion

        #region Constructors and Destructors

        /// <summary> Initialisiert eine neue Instanz der <see cref="Sender1"/> Klasse. </summary>
        public Sender2(PasswordMediator mediator)
        {
            this.mediator = mediator;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>Empfängt ein <see cref="IPassword"/>.</summary>
        /// <param name="password">Das <see cref="IPassword"/> welches empfangen wird.</param>
        public void Recieve(IPassword password)
        {
            result = true;
        }

        /// <summary>Sendet ein <see cref="IPassword"/>. </summary>
        /// <param name="password">Das <see cref="IPassword"/> welches verschickt wird.</param>
        public void Send(IPassword password)
        {
            mediator.Send(this, password);
        }

        #endregion
    }

    public class Sender3 : IPasswordSender, IPasswordReciever
    {
        #region Fields

        public bool? result;

        internal PasswordMediator mediator;

        #endregion

        #region Constructors and Destructors

        /// <summary> Initialisiert eine neue Instanz der <see cref="Sender1"/> Klasse. </summary>
        public Sender3(PasswordMediator mediator)
        {
            this.mediator = mediator;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>Empfängt ein <see cref="IPassword"/>.</summary>
        /// <param name="password">Das <see cref="IPassword"/> welches empfangen wird.</param>
        public void Recieve(IPassword password)
        {
            result = true;
        }

        /// <summary>Sendet ein <see cref="IPassword"/>. </summary>
        /// <param name="password">Das <see cref="IPassword"/> welches verschickt wird.</param>
        public void Send(IPassword password)
        {
            mediator.Send(this, password);
        }

        #endregion
    }
}