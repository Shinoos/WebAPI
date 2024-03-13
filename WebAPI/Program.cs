using MySql.Data.MySqlClient;
using System.Configuration;
using System.Linq.Expressions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

// Routing

app.MapGet("/user/{username}", (string username) =>
{
    var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    IConfigurationRoot configuration = builder.Build();
    string connectionString = configuration.GetConnectionString("MyBDD");

    MySqlConnection connection = new MySqlConnection(connectionString);

    try
    {
        using (connection)
        {
            connection.Open();
            const string query = "SELECT COUNT(*) FROM useraccounts WHERE username = @username";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@username", username);
                long userExist = (long)command.ExecuteScalar();

                return userExist;
            }
        }
    } catch (Exception)
    {
        return 2;
    }
    
        
});


app.MapGet("/user/{username}/{password}", (string username, string password) =>
{
    var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    IConfigurationRoot configuration = builder.Build();
    string connectionString = configuration.GetConnectionString("MyBDD");

    MySqlConnection connection = new MySqlConnection(connectionString);

    try
    {
        using (connection)
        {
            connection.Open();
            const string query = "SELECT COUNT(*) FROM useraccounts WHERE username = @username AND password = @password";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", password);
                long userExist = (long)command.ExecuteScalar();

                return userExist;
            }
        }
    } catch (Exception)
    {
        return 2;
    }
   

});

app.MapPost("/user/{username}/{password}", (string username, string password) =>
{
    var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    IConfigurationRoot configuration = builder.Build();
    string connectionString = configuration.GetConnectionString("MyBDD");

    MySqlConnection connection = new MySqlConnection(connectionString);
    try
    {
        using (connection)
        {
            connection.Open();
            const string query = "SELECT COUNT(*) FROM useraccounts WHERE username = @username";
            string usernameVerif = username;
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@username", usernameVerif);
                long userExist = (long)command.ExecuteScalar();
                if (userExist > 0)
                {
                    return 0;
                }
                else
                {
                    const string sql = "INSERT INTO useraccounts (username, password) VALUES (@username, @password)";
                    using (var command2 = new MySqlCommand(sql, connection))
                    {
                        command2.Parameters.AddWithValue("@username", username);
                        command2.Parameters.AddWithValue("@password", password);
                        int rowsAffected = command2.ExecuteNonQuery();
                        switch (rowsAffected)
                        {
                            case > 0:
                                return 1;
                            default:
                                return 0;
                        }
                    }
                }

            }
        }
    } catch (Exception)
    {
        return 2;
    }
});

app.Run();