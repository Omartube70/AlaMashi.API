//using AlaMashi.BLL;
//using Microsoft.AspNetCore.Mvc;
//using System.Collections.Generic; 
//using System.Linq;
//using Microsoft.AspNetCore.JsonPatch;

//// DTOs (Data Transfer Objects)
//public class AddressResponseDto
//{
//    public int AddressID { get; set; }
//    public int UserID { get; set; }
//    public string Street { get; set; }
//    public string City { get; set; }
//    public string AddressDetails { get; set; }
//    public string AddressType { get; set; }
//}
//public class CreateAddressRequestDto
//{
//    public int UserID { get; set; }
//    public string Street { get; set; }
//    public string City { get; set; }
//    public string AddressDetails { get; set; }
//    public int AddressType { get; set; } // 0: home, 1: Work, 2: Another
//}
//public class UpdateAddressRequestDto
//{
//    public string Street { get; set; }
//    public string City { get; set; }
//    public string AddressDetails { get; set; }
//    public int AddressType { get; set; } // 0: home, 1: Work, 2: Another
//}

//[ApiController]
//[Route("api/[controller]")]
//public class AddressesController : ControllerBase
//{
//    // GET: api/addresses/{id}
//    [HttpGet("{AddressID}")]
//    public IActionResult GetAddressById(int AddressID)
//    {
//        var addressBll = AddressBLL.GetAddressByAddressID(AddressID);

//        if (addressBll == null)
//        {
//            // Middleware سيلتقط هذا الخطأ ويحوله إلى 404 Not Found
//            throw new KeyNotFoundException($"Address with id {AddressID} not found.");
//        }

//        var addressDto = MapToResponseDto(addressBll);

//        return Ok(new { status = "success", data = addressDto });
//    }

//    // GET: api/addresses/all/{userId}
//    [HttpGet("all/{UserId}")]
//    public IActionResult GetAddressesByUserId(int UserId)
//    {
//        var addressesBll = AddressBLL.GetAllAddressByUserID(UserId);

//        var addressDtos = addressesBll.Select(MapToResponseDto).ToList();

//        return Ok(addressDtos );
//    }

//    // GET: api/addresses/all
//    [HttpGet("all")]
//    public IActionResult GetAllAddresses()
//    {
//        var addressesBll = AddressBLL.GetAllAddress();

//        var addressDtos = addressesBll.Select(MapToResponseDto).ToList();

//        return Ok(addressDtos);
//    }


//    // POST: api/addresses
//    [HttpPost("Create")]
//    public IActionResult CreateAddress([FromBody] CreateAddressRequestDto createDto)
//    {
//        var newAddressBll = new AddressBLL
//        {
//            UserID = createDto.UserID,
//            Street = createDto.Street,
//            City = createDto.City,
//            AddressDetails = createDto.AddressDetails,
//            AddressType = (AddressBLL.enAddressType) createDto.AddressType
//        };

//        // الـ Save() سترمي ArgumentException إذا فشل التحقق من البيانات
//        if (newAddressBll.Save())
//        {
//            var addressDto = MapToResponseDto(newAddressBll);

//            // 201 Created هي الاستجابة الأفضل عند الإنشاء
//            return CreatedAtAction(nameof(GetAddressById), new { AddressID = addressDto.AddressID }, new { status = "success", data = addressDto });
//        }

//        // لن يتم الوصول هنا غالبًا لأن BLL.Save يرمي استثناءً عند الفشل
//        return BadRequest(new { status = "error", message = "Failed to save the address." });
//    }

//    // PATCH: api/addresses/{AddressID}
//    [HttpPatch("{AddressID}")]
//    public IActionResult PatchAddress(int AddressID, [FromBody] JsonPatchDocument<UpdateAddressRequestDto> patchDoc)
//    {
//        if (patchDoc == null)
//        {
//            return BadRequest();
//        }

//        // 1. ابحث عن العنوان الأصلي في قاعدة البيانات
//        var addressBll = AddressBLL.GetAddressByAddressID(AddressID);
//        if (addressBll == null)
//        {
//            throw new KeyNotFoundException($"Address with id {AddressID} not found.");
//        }

//        // 2. أنشئ DTO من البيانات الحالية لتطبيق التعديلات عليه
//        var addressToPatchDto = new UpdateAddressRequestDto
//        {
//            Street = addressBll.Street,
//            City = addressBll.City,
//            AddressDetails = addressBll.AddressDetails,
//            AddressType = (int)addressBll.AddressType
//        };

//        // 3. طبّق التغييرات القادمة من الـ request على الـ DTO
//        // سيتم وضع أي أخطاء في ModelState تلقائيًا
//        patchDoc.ApplyTo(addressToPatchDto, ModelState);

//        // 4. تحقق من أن الموديل ما زال صالحًا بعد تطبيق التعديلات
//        if (!TryValidateModel(addressToPatchDto))
//        {
//            return ValidationProblem(ModelState);
//        }

//        // 5. انقل البيانات المحدثة من الـ DTO إلى كائن الـ BLL الأصلي
//        addressBll.Street = addressToPatchDto.Street;
//        addressBll.City = addressToPatchDto.City;
//        addressBll.AddressDetails = addressToPatchDto.AddressDetails;
//        addressBll.AddressType = (AddressBLL.enAddressType)addressToPatchDto.AddressType;

//        // 6. احفظ التغييرات في قاعدة البيانات
//        if (addressBll.Save())
//        {
//            return Ok(new { status = "success", data = MapToResponseDto(addressBll) });
//        }

//        throw new Exception("An error occurred while updating the user.");
//    }


//    // DELETE: api/addresses/{AddressID}
//    [HttpDelete("{AddressID}")]
//    public IActionResult DeleteAddress(int AddressID)
//    {
//        if (!AddressBLL.isAddressExist(AddressID))
//        {
//            throw new KeyNotFoundException($"Address with ID {AddressID} not found.");
//        }

//        if (AddressBLL.DeleteAddress(AddressID))
//        {
//            return Ok(new { status = "success", message = $"Address with ID {AddressID} was deleted successfully." });
//        }

//        throw new Exception("An error occurred while deleting the user.");
//    }

//    // دالة مساعدة لتجنب تكرار كود التحويل
//    private AddressResponseDto MapToResponseDto(AddressBLL bll)
//    {
//        return new AddressResponseDto
//        {
//            AddressID = bll.AddressID,
//            UserID = bll.UserID,
//            Street = bll.Street,
//            City = bll.City,
//            AddressDetails = bll.AddressDetails,
//            AddressType = bll.AddressType.ToString()
//        };
//    }
//}