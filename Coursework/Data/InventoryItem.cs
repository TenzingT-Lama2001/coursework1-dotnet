

namespace Coursework.Data;

    public class InventoryItem
    {

    public Guid Id { get; set; } = Guid.NewGuid();
    public string ItemName { get; set; }
       
            
        public int Quantity { get; set; }
        public Guid CreatedBy { get; set; }

    
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    
    }

