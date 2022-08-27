namespace backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    //if you want to change the port, you can use one of below examples
                    //webBuilder.UseUrls("http://localhost:5000");
                    //webBuilder.UseUrls("https://localhost:5001");
                    //webBuilder.UseUrls("http://localhost:5000","https://localhost:5001");
                });
    }
}