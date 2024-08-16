namespace OCD.Services
{
    public class NotificationState
    {
        public event Action OnChange;
        private int _newCommentsCount;

        public int NewCommentsCount
        {
            get => _newCommentsCount;
            set
            {
                _newCommentsCount = value;
                NotifyStateChanged();
            }
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
