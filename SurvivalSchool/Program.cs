using BusinessLogic.Services;
using DataAccess.Wrapper;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Buffers;
using System.Reflection;
using System.Xml.Linq;

namespace SurvivalSchool
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<SurvivalSchool1Context>(
                 options => options.UseSqlServer(builder.Configuration["ConnectionString"]));

            builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IVideoService, VideoService>();
            builder.Services.AddScoped<IUserTestResultService, UserTestResultService>();
            builder.Services.AddScoped<IForumPostService, ForumPostService>();
            builder.Services.AddScoped<IForumThreadService, ForumThreadService>();
            builder.Services.AddScoped<ILectureService, LectureService>();
            builder.Services.AddScoped<ITestQuestionService, TestQuestionService>();
            builder.Services.AddScoped<ITestService, TestService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {

                options.SwaggerDoc("v1", new OpenApiInfo
                {

                    Version = "v1",
                    Title = "SurvivalSchollAPI",
                    Description = "Школа выживания",
                    Contact = new OpenApiContact
                    {
                        Name = "Бекэндер",
                        Url = new Uri("https://t.me/Ares250678")
                    }
                });
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var sevices = scope.ServiceProvider;

                var context = sevices.GetRequiredService<SurvivalSchool1Context>();
                context.Database.Migrate();

                context.Database.EnsureCreated();
                context.Roles.AddRange(
                    new Role { RoleName = "USER" },
                    new Role { RoleName = "ADMIN" },
                    new Role { RoleName = "SUPPORT" }
                );
                context.Categories.AddRange(
                    new Category { CategoryName = "FOREST" },
                    new Category { CategoryName = "ISLAND" },
                    new Category { CategoryName = "COLLEGE TSARITSINO" }
                );
                context.SaveChanges();
            }


            // Configure the HTTP request pipeline.
            //  if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}