namespace Tresor.Utilities
{
    using System.Deployment.Application;
    using System.Timers;

    public class ClickOnceUpdateManager
    {
        private readonly ApplicationDeployment currentDeployment = ApplicationDeployment.CurrentDeployment;
        private readonly Timer timer = new Timer(60000);

        public bool IsUpdating { get; private set; }

        public double UpdateInterval
        {
            get { return timer.Interval; }
            set { timer.Interval = value; }
        }



        public ClickOnceUpdateManager()
        {
            timer.Elapsed += (obj, sender) => CheckForUpdate();
            currentDeployment.UpdateCompleted += (obj, sender) => UpdateCompleted();
            currentDeployment.CheckForUpdateCompleted += UpdateCheckCompleted;
        }



        public void Start()
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }



        private void CheckForUpdate()
        {
            if (IsUpdating) return;
            currentDeployment.CheckForUpdateAsync();
        }

        private void UpdateCompleted()
        {
            IsUpdating = false;
            // TODO: Über Update nach Außen informieren
        }

        private void UpdateCheckCompleted(object sender, CheckForUpdateCompletedEventArgs arguments)
        {
            if (arguments.UpdateAvailable)
            {
                Update();
            }
        }

        private void Update()
        {
            IsUpdating = true;
            currentDeployment.UpdateAsync();
            // TODO: Über Update Vorgang nach Außen informieren
        }
    }
}