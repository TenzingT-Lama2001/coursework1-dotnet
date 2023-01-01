

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
    public static List<RequestedItem> GetAllRequestedItems()
    {
        string requesteditemsFilePath = Utils.GetRequestedItemsFilePath();
        if (!File.Exists(requesteditemsFilePath))
        {
            return new List<RequestedItem>();
        }

        var json = File.ReadAllText(requesteditemsFilePath);

        return JsonSerializer.Deserialize<List<RequestedItem>>(json);
    }
    public static List<DetailedInventory> GetAllDetailedInventorys()
    {
        string itemsDetailsFilePath = Utils.GetInventoryDetailsFilePath();
        if (!File.Exists(itemsDetailsFilePath))
        {
            return new List<DetailedInventory>();
        }

        var json = File.ReadAllText(itemsDetailsFilePath);

        return JsonSerializer.Deserialize<List<DetailedInventory>>(json);
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





    private static void SaveAllRequestedItems(List<RequestedItem> items)
    {
        string appDataDirectoryPath = Utils.GetAppDirectoryPath();
        string requestedItemsFilePath = Utils.GetRequestedItemsFilePath();

        if (!Directory.Exists(appDataDirectoryPath))
        {
            Directory.CreateDirectory(appDataDirectoryPath);
        }

        var json = JsonSerializer.Serialize(items);
        File.WriteAllText(requestedItemsFilePath, json);
    }
    public static List<RequestedItem> RequestItem(string username, string itemname,int quantity )
    {

        List<RequestedItem> requestedItems = GetAllRequestedItems();
        requestedItems.Add(new RequestedItem
        {
        UserName = username,
        Quantity = quantity,
        ItemName = itemname,
        Status = Status.Pending

    });

        SaveAllRequestedItems(requestedItems);
        return requestedItems;

    }




    private static void SaveAllDetailedInventorys(List<DetailedInventory> items)
    {
        string appDataDirectoryPath = Utils.GetAppDirectoryPath();
        string itemsFilePath = Utils.GetInventoryDetailsFilePath();

        if (!Directory.Exists(appDataDirectoryPath))
        {
            Directory.CreateDirectory(appDataDirectoryPath);
        }

        var json = JsonSerializer.Serialize(items);
        File.WriteAllText(itemsFilePath, json);
    }


    public static List<DetailedInventory> UpdateDetailedInventory() {
        List<RequestedItem> requestedItems = GetAllRequestedItems();
        List<InventoryItem> allItems = GetAll();
        List<DetailedInventory> inventDetail = GetAllDetailedInventorys();



        var nameDate = requestedItems
         .GroupBy(item => item.ItemName)
         .Select(group => new
         {
             ItemName = group.Key,
             TakenOutDate = group.Max(x => x.TakenOutDate)
         })
         .ToList();

        List<DetailedInventory> DetailedInventorys = allItems
            .GroupJoin(nameDate,
                inventoryItem => inventoryItem.ItemName,
                latestDate => latestDate.ItemName,
                (inventoryItem, latestDate) => new { inventoryItem, latestDate })
            .SelectMany(x => x.latestDate.DefaultIfEmpty(),
                (x, y) => new DetailedInventory
                {
                    ItemName = x.inventoryItem.ItemName,
                    Quantity = x.inventoryItem.Quantity,
                    TakenOutDate = y?.TakenOutDate
                })
            .ToList();



        SaveAllDetailedInventorys(DetailedInventorys);

        return DetailedInventorys;
    }

    public static List<RequestedItem> ApproveItem(string adminName,string username,string itemname, int quantity,Guid requestItemId,bool isApproved)
    {

        List<RequestedItem> requestedItems = GetAllRequestedItems();
        List<InventoryItem> allItems = GetAll();


        InventoryItem itemToUpdate = allItems.FirstOrDefault(x => x.ItemName == itemname);
        RequestedItem requestedItemToUpdate= requestedItems.FirstOrDefault(x => x.Id == requestItemId);
        
        itemToUpdate.Quantity -= quantity;
        requestedItemToUpdate.AdminName = adminName;
        requestedItemToUpdate.Status = isApproved ? Status.Approved : Status.Pending;

        string dateNow = DateTime.Now.ToString("dd/MM/yyyy");
        requestedItemToUpdate.TakenOutDate = dateNow;




        SaveAll(allItems);
        SaveAllRequestedItems(requestedItems);




        return requestedItems;
    }
    public static List<RequestedItem> DeclineRequestedItem(Guid id)
    {
        List<RequestedItem> items = GetAllRequestedItems();
        RequestedItem item = items.FirstOrDefault(x => x.Id == id);

        if (item == null)
        {
            throw new Exception("Item request not found.");
        }

        items.Remove(item);
        SaveAllRequestedItems(items);
        return items;
    }

    



}
