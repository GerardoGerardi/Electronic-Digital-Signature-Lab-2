using Common.InstanceManagement;
using Common.TextGeneration;
using Crypt;

namespace Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddTransient<ICryptMaster, RSAMaster>(); //не хранит важные данные может создаваться по требованию
            builder.Services.AddSingleton<IInstanceManager, InstanceManager>(); //хранит важные данные об инстансах пользователей должен создаваться один раз и жить вечно
            builder.Services.AddSingleton<ITextGenerator>(provider => { return new TextGenerator(builder.Configuration.GetValue<string>("apiKey")); });//генератор текста, пусть будет жить вечно из-за использования httpClient
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
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