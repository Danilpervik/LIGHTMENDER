using WinFormsApp1.View;  

namespace WinFormsApp1
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm()); 
        }
    }
}