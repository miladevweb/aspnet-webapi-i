We need to send this to server to update it

```json
[
   {
      "path": "/name",
      "op": "replace",
      "value": "NEW_VALUE"
   }
]
```

<br>

# Pull Docker Image

```bash
docker pull mcr.microsoft.com/mssql/server
```

<br>

# Create Docker Container

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=My@Password" -v sqlvolume:/var/opt/mssql -p 1433:1433 -d --rm --name mssql mcr.microsoft.com/mssql/server
```

<br>

# Add our connection to appsetings.json or secrets variables

```json
// appsettings.json
"ConnectionStrings": {
      "DefaultConnection": "Server=localhost;Database=MyDatabaseName;User ID=sa;Password=My@Password;Trusted_Connection=true;TrustServerCertificate=True;MultipleActiveResultSets=true"
   }
```

<br>

# Now, We should add our service to Program.cs

```cs
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
```

<br>

# Migrate and Create / Update our database

```bash
dotnet ef migrations add MigrationName --output-dir path/where/migrations/will/be
dotnet ef database update
```

<br>

# Dependency Injection

Luego inyectamos nuestro DbContext en nuestro controller

```cs
public class VillaController: ControllerBase
{
    private readonly ApplicationDbContext _db;
    public VillaController(ApplicationDbContext db)
    {
        _db = db;
    }
   //  ....
}
```


