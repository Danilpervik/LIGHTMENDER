namespace WinFormsApp1.Controller
{
    public class GameLoopTimer
    {
        private System.Windows.Forms.Timer timer;
        private int interval;
        private Action onTick;
        private bool isRunning;
        public GameLoopTimer()
        {
            timer = new System.Windows.Forms.Timer();
            interval = 16;
            timer.Interval = interval;
            onTick = null;
            isRunning = false;
            timer.Tick += (s, e) => 
            {
                if (onTick != null)
                {
                    onTick();
                }
            };
        }
        public void SetOnTick(Action onTick)
        {
            this.onTick = onTick;
        }
        public void Start() 
        {
            if (onTick == null)
                throw new InvalidOperationException("Не установлен метод для вызова (SetOnTick)");
            timer.Start();
            isRunning = true;
        }
        public void Stop() 
        {
            timer.Stop();
            isRunning = false;
        }
        public bool IsRunning() 
        { 
            return isRunning;
        }
    }
}