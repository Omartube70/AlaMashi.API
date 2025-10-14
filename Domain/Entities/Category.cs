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
            public string IconName { get; private set; }
            public int? ParentID { get; private set; }
            public virtual Category? Parent { get; private set; }
            public virtual ICollection<Category> SubCategories { get; private set; } = new List<Category>();
            public virtual ICollection<Product> Products { get; set; } = new List<Product>();


        private Category(string categoryName,string iconName, int? parentId)
            {
                CategoryName = categoryName;
                IconName = iconName;
                ParentID = parentId;
            }

    #pragma warning disable CS8618
            private Category()
            {
            }
    #pragma warning restore CS8618

            public static Category Create(string categoryName,string iconName, int? parentId)
            {
                // --- Validation ---
                if (string.IsNullOrWhiteSpace(categoryName))
                    throw new ArgumentException("CategoryName is required.", nameof(categoryName));

                if (string.IsNullOrWhiteSpace(iconName))
                    throw new ArgumentException("iconName is required.", nameof(iconName));


                // --- Object Creation ---
                return new Category(categoryName, iconName, parentId);
            }

            public void UpdateCategoryName(string categoryName)
            {
                // --- Validation ---
                if (string.IsNullOrWhiteSpace(categoryName))
                    throw new ArgumentException("CategoryName is required.", nameof(categoryName));

                // --- State Update ---
                CategoryName = categoryName;
            }

            public void UpdateIconName(string iconName)
            {
                // --- Validation ---
                if (string.IsNullOrWhiteSpace(iconName))
                    throw new ArgumentException("iconName is required.", nameof(iconName));
                // --- State Update ---
                IconName = iconName;
            }

        }
    }
