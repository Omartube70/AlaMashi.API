using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Category
    {
        public int CategoryID { get; private set; }
        public string CategoryName { get; private set; }
        public int? ParentID { get; private set; }
        public virtual Category? Parent { get; private set; }
        public virtual ICollection<Category> SubCategories { get; private set; } = new List<Category>();



        private Category(string categoryName, int? parentId)
        {
            CategoryName = categoryName;
            ParentID = parentId;
        }

#pragma warning disable CS8618
        private Category()
        {
        }
#pragma warning restore CS8618

        public static Category Create(string categoryName, int? parentId)
        {
            // --- Validation ---
            if (string.IsNullOrWhiteSpace(categoryName))
                throw new ArgumentException("CategoryName is required.", nameof(categoryName));

            // --- Object Creation ---
            return new Category(categoryName, parentId);
        }

        public void UpdateCategoryName(string categoryName)
        {
            // --- Validation ---
            if (string.IsNullOrWhiteSpace(categoryName))
                throw new ArgumentException("CategoryName is required.", nameof(categoryName));

            // --- State Update ---
            CategoryName = categoryName;
        }

    }
}
