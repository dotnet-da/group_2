using backend;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            //ConnectionStringStorer.Instance.ConnectionString = Configuration["ConnectionStrings:DefaultConnection"];
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddTransient(_ => new Database("User Id=dotnet_group2user;Password=joYaNTEROPKiNs;Server=141.100.70.93;Port=5432;Database=stjucloo;"));
            //ConnectionStringStorer.Instance.ConnectionString = Configuration["ConnectionStrings:DefaultConnection"];

            services
                .AddAuthentication()
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", options => { });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("BasicAuthentication", new AuthorizationPolicyBuilder("BasicAuthentication").RequireAuthenticatedUser().Build());
            });
           
            ConnectionStringStorer.Instance.ConnectionString = "User Id=dotnet_group2user;Password=joYaNTEROPKiNs;Server=141.100.70.93;Port=5432;Database=stjucloo;";
            Console.WriteLine("initted");
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}