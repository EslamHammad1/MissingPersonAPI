using MissingPersonAPI.Models;
using MissingPersonAPI.Services;

var builder = WebApplication.CreateBuilder(args);

var CS = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Configuration.AddUserSecrets<ApplicationUser>();// new
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddHttpClient();

builder.Services.AddCors
(CorsOptions =>
{
    CorsOptions.AddPolicy("MyPolicy", CorsPolicyBuilder =>
    {
        CorsPolicyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});
//builder.Services.AddCors();
//var origins = builder.Configuration.GetSection("Cors")?.GetSection("Origins")?.Value?.Split(',');
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("MyPolicy", policyBuilder =>
//    {
//        policyBuilder.AllowAnyHeader();
//        policyBuilder.AllowAnyMethod();
//        if (origins is not { Length: > 0 })
//        {
//            return;
//        }

//        if (origins.Contains("*"))
//        {
//            policyBuilder.AllowAnyHeader();
//            policyBuilder.AllowAnyMethod();
//            policyBuilder.SetIsOriginAllowed(host => true);
//            policyBuilder.AllowCredentials();
//        }
//        else
//        {
//            foreach (var origin in origins)
//            {
//                policyBuilder.WithOrigins(origin);
//            }
//        }
//    });
//}); 
//(options =>
//{
//    options.AddDefaultPolicy(
//        builder =>
//        {
//            builder.AllowAnyOrigin()
//                .AllowAnyMethod()
//                .AllowAnyHeader();
//        });
//});

builder.Services.AddDbContext<MissingPersonEntity>(options =>
{
    options.UseSqlServer(CS);
});
//___________________________
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().
   AddEntityFrameworkStores<MissingPersonEntity>().AddDefaultTokenProviders(); // add AddDefaultTokenProviders for resat pass or forget
builder.Services.AddScoped<IFoundPersonService, FoundPersonService >();
builder.Services.AddScoped<ILostPersonService, LostPersonService>();
builder.Services.AddScoped<ISearchService, SearchService>();
//todo:check Jwt

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudiance"],
        IssuerSigningKey =
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
});

/// swagger buttom 
builder.Services.AddSwaggerGen(Options =>
{
    Options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Missing person",
        Version = "v1",
        Description = "{\r\n  \"userName\": \"Eslam_Hammad\",\r\n  \"password\": \"Eslam1234@\"\r\n}",

        Contact = new OpenApiContact
        {
            Name = "Eslam Hammad",
            Email = "eslamhammadxz13@gmail.com\r\n",
            Url = new Uri("https://www.linkedin.com/in/eslamhamma74/")

        }
    });
    Options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
    });
    Options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                    new OpenApiSecurityScheme
                    {
                    Reference = new OpenApiReference
                    {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer",
                    },
                    Name ="Bearer",
                    In = ParameterLocation.Header
                    },
                    new List<string>()

                    }
    });

});

var app = builder.Build();
//app.Use((context, next) =>
//{
//    context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
//    context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, PATCH, OPTIONS");
//    context.Response.Headers.Add("Access-Control-Allow-Headers",
//    "Content-Type, Authorization");
//    //context.Response.Headers.Add("Access-Control-Allow-Headers",
//    // "Content-Type, Authorization, x-requested-with, x-signalr-user-agent");
//    //context.Response.Headers.Add("Access-Control-Allow-Credentials", "true"); // Add this line
//    if (context.Request.Method == "OPTIONS")
//    {
//        context.Response.StatusCode = 200;
//        return Task.CompletedTask;
//    }
//    return next();
//});
// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("MyPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
