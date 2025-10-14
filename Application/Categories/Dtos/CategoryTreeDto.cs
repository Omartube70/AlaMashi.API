using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Categories.Dtos
{
    public class CategoryTreeDto
    {
        public int CategoryId { get; init; }
        public string CategoryName { get; init; }
        public string IconName { get; init; }  // ✅ الأيقونة


        // قائمة تحتوي على الأبناء (الفئات الفرعية) لهذه الفئة
        public List<CategoryTreeDto> SubCategories { get; init; } = new();
    }
}
