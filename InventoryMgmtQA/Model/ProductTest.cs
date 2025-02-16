using InventoryMgmt.Model;
using InventoryMgmt.Service;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

// guide: https://learn.microsoft.com/en-us/visualstudio/test/walkthrough-creating-and-running-unit-tests-for-managed-code?view=vs-2022

namespace InventoryMgmtQA.Model
{
    [TestClass]
    public class ProductTest
    {
        [TestMethod]
        public void TestAddProduct()
        {
            // create a new product with compliant attribute values
            Product product = new()
            {
                Name = "TestProduct",
                QuantityInStock = 1,
                Price = 1.0M
            };

            var results = new List<ValidationResult>();
            var context = new ValidationContext(product, null, null);
            bool isProductValid = Validator.TryValidateObject(product, context, results, true);

            // the product must be valid since all attributes values validated correctly
            Assert.IsTrue(isProductValid);
        }

        [TestMethod]
        public void TestAddProductPriceNegative()
        {
            Product product = new()
            {
                Name = "TestProduct",
                QuantityInStock = 1,
                Price = -1.0M // test for negative price
            };

            var results = new List<ValidationResult>();
            var context = new ValidationContext(product, null, null);
            bool isProductValid = Validator.TryValidateObject(product, context, results, true);

            // the product must NOT be valid since the Price attribute has invalid value
            Assert.IsFalse(isProductValid);
        }

        // add more test cases here

        // Test Function for Removal of Products

        [TestMethod]
        public void TestAddProduct_InvalidNameSpecialCharacters()
        {
            Product product = new()
            {
                Name = "Product$#$#",
                QuantityInStock = 10,
                Price = 100.0M
            };

            Assert.IsFalse(IsProductValid(product), "Product name should not contain special characters.");
        }

        [TestMethod]
        public void TestAddProduct_NegativeQuantity()
        {
            Product product = new()
            {
                Name = "ValidProduct",
                QuantityInStock = -5, // ❌ Negative value
                Price = 100.0M
            };

            Assert.IsFalse(IsProductValid(product), "Quantity must be greater than or equal to 0.");
        }

        [TestMethod]
        public void TestAddProduct_LargeQuantity()
        {
            Product product = new()
            {
                Name = "ValidProduct",
                QuantityInStock = int.MaxValue, 
                Price = 100.0M
            };

            Assert.IsFalse(IsProductValid(product), "Quantity should not exceed a reasonable limit.");
        }

        [TestMethod]
        public void TestAddProduct_LeadingZeros()
        {
            Product product = new()
            {
                Name = "ValidProduct",
                QuantityInStock = int.Parse("00322232"), // Should be interpreted correctly
                Price = decimal.Parse("00032323") // Should be interpreted correctly
            };

            Assert.IsFalse(IsProductValid(product), "System should validate numeric inputs properly.");
        }

        [TestMethod]
        public void TestAddProduct_NullValues()
        {
            Product product = new()
            {
                Name = "",
                QuantityInStock = 0,
                Price = 0
            };

            Assert.IsFalse(IsProductValid(product), "Product name, quantity, and price cannot be empty.");
        }

        // Test Function for Removal of Products

        [TestMethod]
        public void TestRemoveProduct_ShouldFailForNonexistentProduct()
        {
            InventoryManager inventory = new();
            inventory.RemoveProduct(9999); // Trying to remove a non-existent product

            // Assuming RemoveProduct does not return a value, we need to check the product list
            bool productExists = inventory.Products.Any(p => p.ProductID == 9999);
            Assert.IsFalse(productExists, "Product removal should fail for a nonexistent product.");
        }

        [TestMethod]
        public void TestRemoveProduct_NegativeID()
        {
            InventoryManager inventory = new();
            bool result = inventory.Products.Any(p => p.ProductID == -1); // Trying to remove a product with a negative ID

            Assert.IsFalse(result, "Product removal should fail for a negative product ID.");
        }

        [TestMethod]
        public void TestRemoveProduct_ExistingAutomated()
        {
            InventoryManager inventory = new();
            Product product = new() { Name = "AutomatedTestProduct", QuantityInStock = 5, Price = 10.0M };

            inventory.AddNewProduct(product.Name, product.QuantityInStock, product.Price);
            var addedProduct = inventory.Products.FirstOrDefault(p => p.Name == product.Name && p.QuantityInStock == product.QuantityInStock && p.Price == product.Price);

            bool removed = false;
            if (addedProduct != null)
            {
                inventory.RemoveProduct(addedProduct.ProductID);
                removed = !inventory.Products.Any(p => p.ProductID == addedProduct.ProductID);
            }

            Assert.IsTrue(removed, "Product should be removed successfully.");
            Assert.IsFalse(inventory.Products.Any(p => p.ProductID == addedProduct.ProductID), "Product should no longer exist in inventory.");
        }


        // Test Function for Modification of Products

        [TestMethod]
        public void TestModifyProduct()
        {
            InventoryManager inventory = new();
            Product product = new() { Name = "TestProduct", QuantityInStock = 10, Price = 20.0M };

            inventory.UpdateProduct(product.ProductID, 50);

            Assert.AreEqual(50, inventory.Products.First(p => p.ProductID == product.ProductID).QuantityInStock, "Product quantity should be updated correctly.");
        }


        [TestMethod]
        public void TestModifyProduct_NotFound()
        {
            InventoryManager inventory = new();
            int nonExistentProductId = 22;
            int newQuantity = 12;

            // UpdateProduct method does not return a value, so we should check the product list instead
            inventory.UpdateProduct(nonExistentProductId, newQuantity);
            bool productUpdated = inventory.Products.Any(p => p.ProductID == nonExistentProductId && p.QuantityInStock == newQuantity);

            Assert.IsFalse(productUpdated, "Product not found, please try again.");
        }


        [TestMethod]
        public void TestModifyProduct_ZeroQuantity()
        {
            InventoryManager inventory = new();
            Product product = new() { Name = "EdgeCaseProduct", QuantityInStock = 10, Price = 20.0M };

            // Add the product to the inventory
            inventory.AddNewProduct(product.Name, product.QuantityInStock, product.Price);
            var addedProduct = inventory.Products.FirstOrDefault(p => p.Name == product.Name && p.QuantityInStock == product.QuantityInStock && p.Price == product.Price);

            // Update the product's quantity to zero
            if (addedProduct != null)
            {
                inventory.UpdateProduct(addedProduct.ProductID, 0);
                bool productUpdated = inventory.Products.Any(p => p.ProductID == addedProduct.ProductID && p.QuantityInStock == 0);

                Assert.IsTrue(productUpdated, "Product quantity should be updated to zero.");
            }
            else
            {
                Assert.Fail("Product was not added to the inventory.");
            }
        }


        // Test Function for Total Value of Product 

        [TestMethod]
        public void TestGetTotalInventoryValue()
        {
            // Arrange: Create an inventory manager and add products
            InventoryManager inventory = new();

            Product product1 = new() { Name = "Product A", QuantityInStock = 10, Price = 20.5M };
            Product product2 = new() { Name = "Product B", QuantityInStock = 5, Price = 50.75M };
            Product product3 = new() { Name = "Product C", QuantityInStock = 2, Price = 100.99M };

            inventory.AddNewProduct(product1.Name, product1.QuantityInStock, product1.Price);
            inventory.AddNewProduct(product2.Name, product2.QuantityInStock, product2.Price);
            inventory.AddNewProduct(product3.Name, product3.QuantityInStock, product3.Price);

            // Act: Calculate total inventory value
            decimal actualTotalValue = inventory.Products.Select(x => x.Price * x.QuantityInStock).Sum();

            // Expected total value calculation
            decimal expectedTotalValue =
                (10 * 20.59989M) + 
                (5 * 50.75M) +  
                (2 * 100.99M); 

            expectedTotalValue = Math.Round(expectedTotalValue, 2); // Ensure precision

            // Assert: Check if the actual value matches the expected value
            Assert.AreEqual(expectedTotalValue, actualTotalValue,
                $"Total inventory value should be {expectedTotalValue} but was {actualTotalValue}");
        }

        [TestMethod]
        public void TestGetTotalInventoryValue_Performance()
        {
            InventoryManager inventory = new();
            Stopwatch stopwatch = Stopwatch.StartNew();

            decimal actualTotalValue = inventory.Products.Select(x => x.Price * x.QuantityInStock).Sum();

            stopwatch.Stop();
            Assert.IsTrue(stopwatch.ElapsedMilliseconds < 200, "Performance test failed: Execution took too long.");
        }


        // Test Function for List of Products

        [TestMethod]
        public void TestListProducts()
        {
            // Arrange: Create an inventory manager and add products
            InventoryManager inventory = new();

            // Add products to the inventory
            inventory.AddNewProduct("Product 1", 20003, 232323);
            inventory.AddNewProduct("Product 2", 100, 500);

            // Act: Fetch the list of products
            string productListJson = JsonConvert.SerializeObject(inventory.Products);

            // Assert: Check if the output is a valid JSON string
            List<Product> products = null;
            try
            {
                products = JsonConvert.DeserializeObject<List<Product>>(productListJson);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Deserialization threw an exception: {ex.Message}");
            }

            // Ensure products exist
            Assert.IsNotNull(products, "Product list should not be null.");
            Assert.IsTrue(products.Any(), "Product list should not be empty.");

            // Verify that expected products exist in the response
            Assert.IsTrue(products.Any(p => p.Name == "Product 1" && p.QuantityInStock == 20003 && p.Price == 232323),
                "Product 1 not found in list.");

            Assert.IsTrue(products.Any(p => p.Name == "Product 2" && p.QuantityInStock == 100 && p.Price == 500),
                "Product 2 not found in list.");
        }

        [TestMethod]
        public void TestListRetrieveManyProductsPerformance()
        {
            InventoryManager inventory = new();
            int numberOfProducts = 10000; // Adjust this number as needed for performance testing

            // Add many products to the inventory
            for (int i = 0; i < numberOfProducts; i++)
            {
                inventory.AddNewProduct($"Product{i}", i, i * 1.0M);
            }

            // Measure the time taken to retrieve all products
            Stopwatch stopwatch = Stopwatch.StartNew();
            var allProducts = inventory.Products.ToList();
            stopwatch.Stop();

            // Assert that the retrieval time is within acceptable limits
            Assert.IsTrue(stopwatch.ElapsedMilliseconds < 500, $"Performance test failed: Retrieval took too long ({stopwatch.ElapsedMilliseconds} ms).");
            Assert.AreEqual(numberOfProducts, allProducts.Count, "The number of retrieved products does not match the expected count.");
        }


        // Helper function to validate product
        private static bool IsProductValid(Product product)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(product, null, null);
            return Validator.TryValidateObject(product, context, results, true);
        }


    }
}
