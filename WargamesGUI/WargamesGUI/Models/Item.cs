namespace WargamesGUI.Models
{
    public class Item
    {
        public int Item_Id { get; set; }
        public string TypeOfItem { get; set; }

        public override string ToString() => $"{TypeOfItem}";
    }
}
