using System.Collections.Generic;

namespace cod.application.Category.Common
{
    public class CategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public List<SubCategoryModel> SubCategories { get; set; }
    }

    public class SubCategoryModel
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
    }
}
