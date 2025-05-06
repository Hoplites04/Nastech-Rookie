namespace WebApplication1.Models.Brand
{
    public class BrandListModel
    {
        public List<BrandModel> Brands { get; set; }

        public List<int> SelectedBrandIds { get; set; } = new();
    }
}
