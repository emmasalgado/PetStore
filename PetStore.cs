
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

class Program
{   
    CreateServiceCollection
    private static void Main(string[] args)
    {
        DogLeash dogLeash = new DogLeash();
        ProductLogic productLogic = new ProductLogic("ball","bone");

        string userInput = HandleUserInput();
        while (userInput.ToLower() != "exit")
        {
            if (userInput == "1")
            {
                CatFood catFood = new CatFood();
                productLogic.AddProduct(catFood);
                Console.WriteLine("Your product was added!" + JsonSerializer.Serialize(catFood));
            }
            if (userInput == "2")
            {
                Console.WriteLine("What product do you want to see?");
                userInput = Console.ReadLine();
                var food = productLogic.GetCatFoodByName(userInput);
                if (food is null)
                {
                    Console.WriteLine("Sorry, couldn't find your product.");
                }
                else
                {
                    Console.WriteLine(JsonSerializer.Serialize(food));

                }
            }
            if (userInput == "3")
            {
               // Console.WriteLine("What product do you want to see?");
                //userInput = Console.ReadLine();
                var listOfProducts = productLogic.GetOnlyInStockProducts();
               
                Console.WriteLine(JsonSerializer.Serialize(listOfProducts));
                
            }
            if (userInput == "4")
            {
                Console.WriteLine($"The total price of inventory on hand is {productLogic.GetTotalPriceOfInventory()}");
                Console.WriteLine();
            }
            userInput = HandleUserInput();
        }
        static CreateServiceCollection IServiceProvider;
        {
            new ServiceCollection()
            .BuildServiceProvider();

        }


    }

    private static string HandleUserInput()
    {
        Console.WriteLine("Press 1 to add a product");
        Console.WriteLine("Type 'exit' to quit");
        Console.WriteLine("Press 2 to view a product");
        Console.WriteLine("Press 3 to view only in stock products");
        Console.WriteLine("Press 4 to view the total price of current inventory");
        string userInput = Console.ReadLine();
        return userInput;
    }
}

public abstract class Product
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string Description { get; set; }
}

public class CatFood : Product
{
    public double WeightPounds { get; set; }
    public bool KittenFood { get; set; }

}
public class DogLeash : Product
{
    public int LengthInches { get; set; }
    public string Material { get; set; }
}
public class ProductLogic : IProductLogic
{
    private List<Product> _products = new List<Product>();
    private Dictionary<string, DogLeash> _dogproducts = new Dictionary<string, DogLeash>();
    private Dictionary<string, CatFood> _catproducts = new Dictionary<string, CatFood>();
    public ProductLogic(string toy, string treat)
    {
        DogLeash product1 = new DogLeash();
        DogLeash product2 = new DogLeash();
        product1.Name = toy;
        product2.Name = treat;
        product1.Quantity = 10;
        product2.Quantity = 0;
        AddProduct(product1);
        AddProduct(product2);
        //Our products list has two things inside of it
    }
    public List<string> GetOnlyInStockProducts()
    {
        return _products.Where(x => x.Quantity > 0).Select(x => x.Name).ToList();
    }
    public decimal GetTotalPriceOfInventory()
    {
        return _products.InStock().Select(x => x.Price).Sum().;
    }
    public void AddProduct(Product product)
    {
        _products.Add(product);
        if(product is DogLeash)
        {
            _dogproducts.Add("Leash", product as DogLeash);
        } else if (product is CatFood)
        {
            _catproducts.Add("Food", product as CatFood);
        }
        
    }
    //public GetProductByName(string name)
    //{
    //    if (_catproducts.ContainsKey(name))
    //    {
    //        return _catproducts[name];
    //    } else if (_dogproducts.ContainsKey(name))
    //    {
    //        return _dogproducts[name];
    //    }
    //}
    
    public DogLeash GetDogLeashByName(string name)
    {
        try
        {
            return _dogproducts[name];
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    public CatFood GetCatFoodByName(string name)
    {
        try
        {
        return _catproducts[name];
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    public List<Product> GetAllProducts()
    {
        return _products;
    }
}
public interface IProductLogic
{
    public void AddProduct(Product product);
    public List<string> GetOnlyInStockProducts();
}
internal static class ListExtensions
{
    public static List<T> InStock<T>(this List<T> list) where T : Product
    {
        return list.Where(x=>x.Quantity > 0).ToList();
    }
}
    public interface IProductRepository
{
    void AddProduct(Product product);
    Product GetProductById(int productId);

    public List<Product> GetAllProducts();
}
    [DbContext(typeof(ProductContext))]
    partial class ProductContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder.HasAnnotation("ProductVersion", "6.0.7");

        modelBuilder.Entity("PetStore.Data.Product", b =>
        {
            b.Property<int>("ProductId")
                .ValueGeneratedOnAdd()
                .HasColumnType("INTEGER");
            b.Property<string>("Description")
                .IsRequired()
                .HasColumnType("TEXT");
            b.Property<string>("Name")
                .IsRequired()
                .HasColumnType("TEXT");
            b.Property<decimal>("Price")
                .HasColumnType("TEXT");
            b.Property<int>("Quantity")
                .HasColumnType("INTEGER");
            b.HasKey("ProductId");
            b.ToTable("Products");
        });
#pragma warning restore 612, 618
    }
}
    public class ProductRepository : IProductRepository
{
    private readonly ProductContext _dbContext;

    public ProductRepository()
    {
        _dbContext = new ProductContext();
    }

    public void AddProduct(Product product)
    {
        _dbContext.Products.Add(product);
        _dbContext.SaveChanges();
    }

    public Product GetProductById(int productId)
    {
        return _dbContext.Products.SingleOrDefault(x => x.ProductId == productId);
    }

    public List<Product> GetAllProducts()
    {
        return _dbContext.Products.ToList();
    }
}
    public class ProductContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    public string DbPath { get; set; }

    public ProductContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "products.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options) =>
        options.UseSqlite($"Data Source={DbPath}");
}