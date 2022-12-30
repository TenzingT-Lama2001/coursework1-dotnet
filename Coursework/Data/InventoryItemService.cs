using System.Text.Json;

namespace Coursework.Data;

public static class InventoryItemService
{
    private static void SaveAll(List<InventoryItem> items)
    {
        string appDataDirectoryPath = Utils.GetAppDirectoryPath();
        string itemsFilePath = Utils.GetItemsFilePath();

        if (!Directory.Exists(appDataDirectoryPath))
        {
            Directory.CreateDirectory(appDataDirectoryPath);
        }

        var json = JsonSerializer.Serialize(items);
        File.WriteAllText(itemsFilePath, json);
    }

    public static List<InventoryItem> GetAll()
    {
        string itemsFilePath = Utils.GetItemsFilePath();
        if (!File.Exists(itemsFilePath))
        {
            return new List<InventoryItem>();
        }

        var json = File.ReadAllText(itemsFilePath);

        return JsonSerializer.Deserialize<List<InventoryItem>>(json);
    }

    public static List<InventoryItem> Create( string ItemName, int Quantity)
    {
        List<InventoryItem> items = GetAll();

        bool itemExists = items.Any(x => x.ItemName == ItemName);

        if (itemExists)
        {
            InventoryItem inventoryUpdate = items.FirstOrDefault(x => x.ItemName == ItemName);
            inventoryUpdate.Quantity += Quantity;
        }
        else
        {
            items.Add(new InventoryItem
            {
                ItemName = ItemName,
                Quantity = Quantity
            });

        }
        SaveAll(items);
        return items;

    }

    public static List<InventoryItem> Delete( Guid id)
    {
        List<InventoryItem> items = GetAll();
        InventoryItem item = items.FirstOrDefault(x => x.Id == id);

        if (item == null)
        {
            throw new Exception("Item not found.");
        }

        items.Remove(item);
        SaveAll( items);
        return items;
    }

    public static void DeleteByUserId()
    {
        string todosFilePath = Utils.GetItemsFilePath();
        if (File.Exists(todosFilePath))
        {
            File.Delete(todosFilePath);
        }
    }

    public static List<InventoryItem> Update(Guid id, string itemName, int quantity)
    {
        List<InventoryItem> items = GetAll();
        InventoryItem itemToUpdate = items.FirstOrDefault(x => x.Id == id);

        if (itemToUpdate == null)
        {
            throw new Exception("Item not found.");
        }

        itemToUpdate.ItemName = itemName;
        itemToUpdate.Quantity = quantity;
        SaveAll( items);
        return items;
    }
}
