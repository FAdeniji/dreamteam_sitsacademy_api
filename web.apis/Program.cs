using common.data;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using System.Text;
using web.apis;
using web.apis.Models;

var builder = WebApplication.CreateBuilder(args);

// Add Cors
#region Add Cors
builder.Services.AddCors(options => {
    options.AddPolicy("Procent",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
            .Build()
            );
});
#endregion

#region MediaR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
#endregion

#region App Insights
builder.Services.AddApplicationInsightsTelemetry(builder.Configuration["ApplicationInsights:InstrumentationKey"]);
#endregion

// Add services to the container.
#region Add services to container
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Add database connection
builder.Services.AddDbContextPool<DbConn>(options => {
    options.UseNpgsql(builder.Configuration.GetValue<string>("ConnectionStrings:PostgressDb"), npgsqlOptions => npgsqlOptions.CommandTimeout(120));
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => {
    // options.SignIn.RequireConfirmedEmail = true;
    // options.SignIn.RequireConfirmedPhoneNumber = false;
}).AddEntityFrameworkStores<DbConn>()
.AddDefaultTokenProviders();

// Configure identity options
builder.Services.Configure<IdentityOptions>(options => {
    options.Password.RequireDigit = builder.Configuration.GetValue<bool>("PasswordConfig:RequireDigit");
    options.Password.RequireLowercase = builder.Configuration.GetValue<bool>("PasswordConfig:RequireLowercase");
    options.Password.RequireNonAlphanumeric = builder.Configuration.GetValue<bool>("PasswordConfig:RequireNonAlphanumeric");
    options.Password.RequiredLength = Convert.ToInt32(builder.Configuration.GetValue<string>("PasswordConfig:PasswordRequiredLength"));
    options.Password.RequireUppercase = builder.Configuration.GetValue<bool>("PasswordConfig:RequireUppercase");
    options.Password.RequiredUniqueChars = Convert.ToInt32(builder.Configuration.GetValue<string>("PasswordConfig:NoOfUniqueChar"));

    // lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(Convert.ToDouble(builder.Configuration.GetValue<string>("LockOut:LockoutMins")));
    options.Lockout.MaxFailedAccessAttempts = Convert.ToInt32(builder.Configuration.GetValue<string>("LockOut:LockOutAttempts"));
    options.Lockout.AllowedForNewUsers = builder.Configuration.GetValue<bool>("PasswordConfig:RequireNonAlphanumeric");

    // User settings
    options.User.AllowedUserNameCharacters = builder.Configuration.GetValue<string>("PasswordConfig:AllowedUserNameCharacters");
    //options.User.RequireUniqueEmail = true;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    var fromHours = builder.Configuration.GetValue<int>("Reminder:FromHours");

    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(fromHours);

    // options.LoginPath = "/Account/Login"; // Redirect to login page if unauthenticated
    // options.AccessDeniedPath = "/Account/AccessDenied"; // Redirect to access denied page if unauthorized
    options.SlidingExpiration = true;
});

// add health check
#region Health Check
builder.Services.AddHealthChecks();
#endregion

// add enum serializers
builder.Services.AddMvc().AddXmlSerializerFormatters();

#region Customise Swagger Document
builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc(builder.Configuration.GetValue<string>("Swagger:Version"), new OpenApiInfo
    {
        Title = builder.Configuration.GetValue<string>("Swagger:Title"),
        Version = builder.Configuration.GetValue<string>("Swagger:Version")
    });

    options.AddSecurityDefinition(builder.Configuration.GetValue<string>("Swagger:SchemeName"), new OpenApiSecurityScheme
    {
        Name = builder.Configuration.GetValue<string>("Swagger:Name"),
        Type = SecuritySchemeType.ApiKey,
        Scheme = builder.Configuration.GetValue<string>("Swagger:SchemeName"),
        BearerFormat = builder.Configuration.GetValue<string>("Swagger:BearerFormat"),
        In = ParameterLocation.Header,
        Description = builder.Configuration.GetValue<string>("Swagger:SchemeDescription"),
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = builder.Configuration.GetValue<string>("Swagger:SchemeName")
                }
            }, new string[]{}
        }
    });
});
#endregion

#region Add Authorization and Authentication
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
.AddJwtBearer(options =>
{
        var audience = builder.Configuration.GetValue<string>("JWT:Audience");
        var issuer = builder.Configuration.GetValue<string>("JWT:Issuer");
        var secret = builder.Configuration.GetValue<string>("JWT:Secret");

        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                {
                    context.Response.Headers.Add("Token-Expired", "true");
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization(config =>
{
    config.AddPolicy(CustomPolicies.Administrator, CustomPolicies.AdministratorPolicy());
    config.AddPolicy(CustomPolicies.IdeaOwner, CustomPolicies.IdeaOwnerPolicy());
    config.AddPolicy(CustomPolicies.CoIdeaOwner, CustomPolicies.CoIdeaOwnerPolicy());
    config.AddPolicy(CustomPolicies.Investor, CustomPolicies.InvestorPolicy());
    config.AddPolicy(CustomPolicies.Institution, CustomPolicies.InstitutionPolicy());
    config.AddPolicy(CustomPolicies.Student, CustomPolicies.StudentPolicy());
    config.AddPolicy(CustomPolicies.Lecturer, CustomPolicies.LecturerPolicy());
    config.AddPolicy(CustomPolicies.ALLLI, CustomPolicies.ALLLIPolicy());
    config.AddPolicy(CustomPolicies.LearningInstitution, CustomPolicies.LearningInstitutionPolicy());
    config.AddPolicy(CustomPolicies.Everyone, CustomPolicies.EveryonePolicy());    
    config.AddPolicy(CustomPolicies.LIManager, CustomPolicies.LIManagerPolicy());
    config.AddPolicy(CustomPolicies.LIAdmin, CustomPolicies.LIAdminPolicy());
});

// builder.Services.AddAutoMapper(typeof(Program));
#endregion

#region "Register Interfaces"
builder.Services.AddTransient<IEmailRepository, EmailRepository>();
builder.Services.AddTransient<INotificationRepository, NotificationRepository>();
builder.Services.AddTransient<IEmailTemplateRepository, EmailTemplateRepository>();
builder.Services.AddTransient<ILogRepository, LogRepository>();
builder.Services.AddTransient<IDashboardRepository, DashboardRepository>();
builder.Services.AddTransient<IUsersRepository, UsersRepository>();

builder.Services.Scan(scan => scan
    .FromAssemblyOf<Program>() // Or any type within your assembly
    .AddClasses(classes => classes
        .AssignableToAny(
            typeof(IEmailRepository),
            typeof(INotificationRepository),
            typeof(IEmailTemplateRepository),
            typeof(ILogRepository),
            typeof(IDashboardRepository),
            typeof(IUsersRepository)
        )
    )
    .AsImplementedInterfaces()
    .WithTransientLifetime()
);

#endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#endregion

#region Token lifespan
builder.Services.Configure<DataProtectionTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.FromMinutes(Convert.ToDouble(builder.Configuration.GetValue<string>("Token:Expiry"))));
#endregion

#region AutoMapper
//var config = new MapperConfiguration(cfg =>
//{
//   cfg.AddProfile(new AutoMapperProfile());
//});

//var mapper = config.CreateMapper();
// Register AutoMapper
// builder.Services.AddAutoMapper(typeof(Program));
// builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

#endregion

#region "Serilog"
builder.Services.AddSingleton(Log.Logger);
#endregion

var app = builder.Build();

app.UseCors("sitsacademy");

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
            var exception = exceptionHandlerPathFeature?.Error;

            if (exception != null)
            {
                var telemetryClient = app.Services.GetRequiredService<TelemetryClient>();
                telemetryClient.TrackException(exception);
            }

            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
        });
    });
//}

#region run Migration
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DbConn>();

    try
    {
        // Apply migrations to the database
        dbContext.Database.Migrate();
    }
    catch(Npgsql.PostgresException ex)
    {
        // If a migration fails due to existing tables, handle it gracefully
        if (ex.Message.Contains("already exists"))
        {
            // Log or handle the fact that the table exists and migration was skipped
            Console.WriteLine("Migration skipped: The table already exists.");
        }
        else
        {
            // Rethrow the exception if it's not related to an existing table
            //throw;
        }
    }
    catch (InvalidOperationException ex)
    {
        // If a migration fails due to existing tables, handle it gracefully
        if (ex.Message.Contains("already exists"))
        {
            // Log or handle the fact that the table exists and migration was skipped
            Console.WriteLine("Migration skipped: The table already exists.");
        }
        else
        {
            // Rethrow the exception if it's not related to an existing table
            //throw;
        }
    }
    catch (Exception ex)
    {
        // Log any other exceptions that may occur
        Console.WriteLine($"Migration failed: {ex.Message}");
        // throw;  // Rethrow if needed
    }
}
#endregion

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

