using BE_Glowpurea.Helpers;
using BE_Glowpurea.IRepositories;
using BE_Glowpurea.IServices;
using BE_Glowpurea.Models;
using BE_Glowpurea.Repositories;
using BE_Glowpurea.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.FileProviders;
using BE_Glowpurea.Filters;


namespace BE_Glowpurea
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                Args = args,
                WebRootPath = "wwwroot"
            });

            /* =====================================================
             * DATABASE
             * ===================================================== */
            builder.Services.AddDbContext<DbGlowpureaContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                ));

            /* =====================================================
             * REPOSITORIES
             * ===================================================== */
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IShapeRepository, ShapeRepository>();

            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();
            builder.Services.AddScoped<IEmailOtpRepository, EmailOtpRepository>();
            builder.Services.AddScoped<IBlogCategoryRepository, BlogCategoryRepository>();
            builder.Services.AddScoped<IBlogRepository, BlogRepository>();
            builder.Services.AddScoped<ICartRepository, CartRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
            builder.Services.AddScoped<IDashboardService, DashboardService>();

            /* =====================================================
             * SERVICES
             * ===================================================== */
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IProductImageService, ProductImageService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IShapeService, ShapeService>();
            builder.Services.AddScoped<IBlogService, BlogService>();
            builder.Services.AddScoped<SingleSessionFilter>();
            builder.Services.AddScoped<IBlogCategoryService, BlogCategoryService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<JwtHelper>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<IAddressService, AddressService>();
            builder.Services.AddScoped<IOrderService, OrderService>();

            /* =====================================================
             * AUTHENTICATION & AUTHORIZATION (JWT)
             * ===================================================== */
            var jwtConfig = builder.Configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtConfig["Key"]);
            //builder.Services.AddDbContext<DbGlowpureaContext>(options =>
            //{
            //    options.UseSqlServer(
            //        builder.Configuration.GetConnectionString("DefaultConnection")
            //    );
            //});

            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtConfig["Issuer"],
                        ValidAudience = jwtConfig["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });

            builder.Services.AddAuthorization();
            builder.Services.AddScoped<IProfileService, ProfileService>();


            /* =====================================================
             * CONTROLLERS, CORS, SWAGGER
             * ===================================================== */
            builder.Services.AddControllers();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy
                        .WithOrigins(
                                 "http://localhost:3000", // Admin
                                 "http://localhost:3001",  // User
                                 "https://glowpurea.id.vn",        // Customer FE
                                 "https://admin.glowpurea.id.vn"
                              )
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();



            /* =====================================================
             * BUILD APP
             * ===================================================== */
            var app = builder.Build();

            app.UseStaticFiles(); // wwwroot

            app.UseSwagger();
            app.UseSwaggerUI();

            // deploy lan 2
            //app.UseHttpsRedirection();
            app.UseCors("AllowFrontend");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}
