using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using StudentManageApp_Codef.Data;
using StudentManageApp_Codef.Data.R_IRepository;
using StudentManageApp_Codef.Data.Repository;
using StudentManageApp_Codef.Service;
using StudentManageApp_Codef.helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

// Thêm MapIdentityApiEndpoints

using System.Text;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleDbConnection")));

//identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorization();

//builder.Services.AddIdentityApiEndpoints<IdentityUser>()
//    .AddEntityFrameworkStores<AppDbContext>();


//   Cấu hình JWT Authentication
IConfiguration configuration = builder.Configuration;
var appSettingsSection = configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);


var appSettings = appSettingsSection.Get<AppSettings>();
var key = Encoding.ASCII.GetBytes(appSettings.Secret);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

//login with google

builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    });


builder.Services.AddTransient<IStudentRepository, StudentRepository>();

builder.Services.AddTransient<IClassRepository, ClassRepository>();

builder.Services.AddTransient<IEnrollmentRepository, EnrollmentRepository>();

builder.Services.AddTransient<IScheduleRepository, ScheduleRepository>();

builder.Services.AddTransient<IUserRepository, UserRepository>();

builder.Services.AddTransient<IGradeRepository, GradeRepository>();

builder.Services.AddTransient<IExamRepository, ExamRepository>();

builder.Services.AddScoped<IAuthRepository, AuthRepository>();

builder.Services.AddScoped<ILecturerRepository, LecturerRepository>();

builder.Services.AddScoped<IRoleRepository, RoleRepository>();






builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<EnrollmentService>();
builder.Services.AddTransient<GradeService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Thêm SwaggerFileOperationFilter vào Swagger
    c.OperationFilter<SwaggerFileOperationFilter>();
});

var app = builder.Build();

app.UseCors("AllowAllOrigins");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();
//app.MapIdentityApiEndpoints();

app.UseHttpsRedirection();


app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();


app.Run();
