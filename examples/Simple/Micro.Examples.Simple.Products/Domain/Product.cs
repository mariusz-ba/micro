using Micro.Domain.Abstractions.BuildingBlocks;
using Micro.Examples.Simple.Products.Domain.Events;

namespace Micro.Examples.Simple.Products.Domain;

public class Product : Aggregate<Guid>
{
    public string Name { get; private set; }
    public decimal Price { get; private set; }

    private Product(Guid id, string name, decimal price) : base(id)
    {
        Name = name;
        Price = price;
    }

    public static Product Create(Guid id, string name, decimal price)
    {
        var product = new Product(id, name, price);
        product.AddDomainEvent(new ProductCreatedDomainEvent(product.Id, product.Name, product.Price));
        return product;
    }
    
    public void Update(string name, decimal price)
    {
        if (name == Name && price == Price)
        {
            return;
        }

        Name = name;
        Price = price;
        
        AddDomainEvent(new ProductUpdatedDomainEvent(Id, name, price));
    }
}