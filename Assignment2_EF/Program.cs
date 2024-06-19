using Core;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using Core.Entities;
using Core.Interfaces;

class Program
{
    private static IUserRepo _repository;

    static void Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddDbContext<DataContext>(options => options.UseSqlite("Data Source=users.db"))
            .AddScoped<IUserRepo, UserRepo>()
            .BuildServiceProvider();

        _repository = serviceProvider.GetService<IUserRepo>();

        // Initialize the database
        InitializeDatabase(serviceProvider);

        while (true)
        {
            Console.WriteLine("\nChoose an option:");
            Console.WriteLine("1. Add User");
            Console.WriteLine("2. Get User");
            Console.WriteLine("3. Get All Persons");
            Console.WriteLine("4. Update Person");
            Console.WriteLine("5. Delete Person");
            Console.WriteLine("6. Exit");

            switch (Console.ReadLine())
            {
                case "1":
                    AddUsers();
                    break;
                case "2":
                    GetUsers();
                    break;
                case "3":
                    GetAllUsers();
                    break;
                case "4":
                    UpdateUsers();
                    break;
                case "5":
                    DeleteUsers();
                    break;
                case "6":
                    return;
                default:
                    Console.WriteLine("Invalid option. Try again.");
                    break;
            }
        }
    }

    private static void InitializeDatabase(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();
            context.Database.Migrate();
        }
    }

    private static void AddUsers()
    {
        Console.Write("Enter name: ");
        string name = Console.ReadLine();
        Console.Write("Enter email: ");
        string email = Console.ReadLine();

        var user = new User { Name = name, Email = email };
        _repository.AddUser(user);
        Console.WriteLine("Person added successfully.");
    }

    private static void GetUsers()
    {
        Console.Write("Enter ID: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var user = _repository.GetUser(id);
            if (user != null)
            {
                Console.WriteLine($"ID: {user.ID}, Name: {user.Name}, Email: {user.Email}");
            }
            else
            {
                Console.WriteLine("User not found.");
            }
        }
        else
        {
            Console.WriteLine("Invalid ID.");
        }
    }

    private static void GetAllUsers()
    {
        var users = _repository.GetAllUsers();
        foreach (var u in users)
        {
            Console.WriteLine($"ID: {u.ID}, Name: {u.Name}, Email: {u.Email}");
        }
    }

    private static void UpdateUsers()
    {
        Console.Write("Enter ID: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var user = _repository.GetUser(id);
            if (user != null)
            {
                Console.Write("Enter new name: ");
                user.Name = Console.ReadLine();
                Console.Write("Enter new email: ");
                user.Email = Console.ReadLine();

                _repository.UpdateUser(user);
                Console.WriteLine("User updated successfully.");
            }
            else
            {
                Console.WriteLine("User not found.");
            }
        }
        else
        {
            Console.WriteLine("Invalid ID.");
        }
    }

    private static void DeleteUsers()
    {
        Console.Write("Enter ID: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            _repository.DeleteUser(id);
            Console.WriteLine("User deleted successfully.");
        }
        else
        {
            Console.WriteLine("Invalid ID.");
        }
    }
}