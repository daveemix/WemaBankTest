using AbiaPayCollectionMiddleware.Helpers;
using MediatR;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NLog.Web;
using System.Reflection;
using WemaCustomer.Application.Data;
using WemaCustomer.Application.Data.Dto;
using WemaCustomer.Application.Data.Models;
using WemaCustomer.Application.Features.Command;
using WemaCustomer.Application.Features.Commands;
using WemaCustomer.Application.Features.Queries;
using WemaCustomer.Application.Services;
using WemaCustomer.Helpers;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddHttpClient();
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped<IRequestHandler<CreateCustomerRequest, ApiResponse<CreateCustomerResponse>>, CreateCustomerHandler>();
builder.Services.AddScoped<IRequestHandler<CompleteOnboardingRequest, ApiResponse<CompleteOnboardingResponse>>, CompleteOnboardingHandler>();
builder.Services.AddScoped<IRequestHandler<GetAllCustomersRequest, ApiResponse<PaginatedList<Customer>>>, GetAllCustomersHandler>();
builder.Services.AddScoped<IRequestHandler<GetAllBanksRequest, ApiResponse<List<BankInfo>>>, GetAllBanksHandler>();
builder.Services.AddScoped<IBankApiService, BankApiService>();


// NLog: Setup NLog for Dependency injection
builder.Logging.ClearProviders();
builder.Host.UseNLog();
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true; 
    options.Providers.Add<GzipCompressionProvider>(); 
});
builder.Services.AddDbContext<wemaDBContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
builder.Services.Configure<BankApiSettings>(configuration.GetSection("WemaConfig"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthorization();
app.UseResponseCompression();

app.MapControllers();

app.Run();
