using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Categories.Commands
{
    public class UpdateCategoryRequest
    {
        public string NewCategoryName { get; set; }
    }
}
