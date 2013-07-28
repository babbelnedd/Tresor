namespace Tresor.UnitTesting.Utilities.Mediator
{
    using NUnit.Framework;

    using global::Tresor.Utilities;

    using global::Tresor.Utilities.Mediator;

    /// <summary>Testet den <see cref="PasswordMediator"/>.</summary>
    [TestFixture(Description = "Testet den PasswordMediator.")]
    public class PasswordMediatorTests
    {
        #region Public Methods and Operators

        /// <summary>Testet die Methoden Notify und Recieve.</summary>
        [Test(Description = "Testet die Methoden Notify und Recieve.")]
        public void NotifyAndRecieve()
        {
            var mediator = new PasswordMediator();
            var sender1 = new Sender1(mediator);
            var sender2 = new Sender2(mediator);
            var sender3 = new Sender3(mediator);

            mediator.Add(sender1);
            mediator.Add(sender2);
            mediator.Add(sender3);

            sender1.Send(new Password());
            Assert.That(sender2.result, Is.True);
            Assert.That(sender3.result, Is.True);
        }

        #endregion
    }
}