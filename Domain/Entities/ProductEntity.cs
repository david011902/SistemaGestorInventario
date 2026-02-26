using System.Xml.Linq;

namespace Domain.Entities
{
    public class ProductEntity
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Sku { get; private set; } = string.Empty;
        public decimal Price { get; private set; }
        public int Stock { get; private set; }
        public int CategoryId { get; private set; }

        //Constructor 
        public ProductEntity(string name, string sku, decimal price, int stock, int categoryId)
        {
            ValidateName(name);
            ValidateSku(sku);
            ValidatePrice(price);
            ValidateStock(stock);

            Id = Guid.NewGuid();
            Name = name.Trim();
            Sku = sku.Trim().ToUpper();
            Price = price;
            Stock = stock;
            CategoryId = categoryId;
        }


        public void UpdateProduct(string name, string sku, decimal price, int stock, int categoryId)
        {
            ValidateName(name);
            ValidateSku(sku);
            ValidatePrice(price);
            ValidateStock(stock);

            Name = name.Trim();
            Sku = sku.Trim().ToUpper();
            Price = price;
            Stock = stock;
            CategoryId = categoryId;
        }

        //Reglas de negocio
        private void ValidateSku(string sku)
        {
            if (string.IsNullOrEmpty(sku))
                throw new ArgumentException("SKU no puede estar vacío.");
            if (sku.Trim().Length < 3)
                throw new ArgumentException("El SKU debe tener al menos 3 caracteres.", nameof(sku));

            if (sku.Trim().Length > 20)
                throw new ArgumentException("El SKU no debe ser mayor a 20 caracteres", nameof(sku));
        }

        private void ValidateName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("El nombre no puede estar vacío.");
            if (name.Trim().Length < 3)
                throw new ArgumentException("El nombre debe tener al menos 3 caracteres.", nameof(name));
            if (name.Trim().Length > 100)
                throw new ArgumentException("El nombre no debe ser mayor a 100 caracteres", nameof(name));
        }
        private void ValidatePrice(decimal price) 
        {
            if (price < 0)
                throw new ArgumentException("El precio no puede ser negativo.", nameof(price));
        }

        private void ValidateStock(decimal stock) {
            if (stock < 0)
                throw new ArgumentException("El stock no puede ser negativo.", nameof(stock));
        }
    }
}
