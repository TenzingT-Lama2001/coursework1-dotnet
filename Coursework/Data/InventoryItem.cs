

namespace Coursework.Data;


    public class Item
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string ItemName { get; set; }


    public int Quantity { get; set; }

}
    public class InventoryItem : Item
    {
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public class RequestedItem : Item
{

    public string AdminName { get; set; }
    public string UserName { get; set; }
    public Status Status { get; set; }
    public string TakenOutDate { get; set; }

}

public  class InventoryDetail
{
    public Guid Id { get; set; }
    public string ItemName { get; set; }
    public int Quantity { get; set; }
    public string TakenOutDate { get; set; }
}

