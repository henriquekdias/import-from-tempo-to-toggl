namespace ImportFromTempoToToggl
{
    class Program
    {
        static void Main(string[] args)
        {
            var w = new Core.Support.Watcher();
            w.Start("starting import time entries from Tempo to Toggl");
            Console.WriteLine();

            var engine = new Core.Engine();
            engine.Run();

            Console.WriteLine();
            Console.WriteLine();
            w.Stop();
            Console.WriteLine();

        }
    }
}