using Microsoft.VisualStudio.TestTools.UnitTesting;
using InventoryMgmt.Service;


[TestClass]
public class InventoryManagerTest
{
    private InventoryManager _inventoryManager;

    [TestInitialize]
    public void Setup()
    {
        _inventoryManager = new InventoryManager();
    }

    [TestMethod]
    public void AddNewProduct_ShouldIncreaseCount()
    {
        _inventoryManager.AddNewProduct("Product 1", 10, 99.99m);
        Assert.AreEqual(1, _inventoryManager.Products.Count);
    }
}
