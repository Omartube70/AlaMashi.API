//using Application.Offers.Commands;
//using Application.Offers.Dtos;
//using Application.Offers.Queries;
//using MediatR;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//[ApiController]
//[Route("api/[controller]")] 
//[Authorize]
//public class OffersController : ControllerBase
//{
//    private readonly ISender _mediator;

//    public OffersController(ISender mediator)
//    {
//        _mediator = mediator;
//    }

//    [HttpGet("active")]
//    public async Task<IActionResult> GetActiveOffers()
//    {
//        // ستحتاج لإنشاء GetActiveOffersQuery
//        var query = new GetActiveOffersQuery();
//        var offers = await _mediator.Send(query);

//        // إرجاع قائمة العروض النشطة
//        return Ok(new { status = "success", data = offers });
//    }

//    // GET: api/offers/{offerId}
//    [HttpGet("{offerId}")]
//    public async Task<IActionResult> GetOfferById(int offerId)
//    {
//        var query = new GetOfferByIdQuery() { OfferId = offerId };
//        var offerDto = await _mediator.Send(query);

//        return Ok(new { status = "success", data = offerDto });
//    }

//    // --- Endpoints للإدارة (Admin) ---

//    // POST: api/offers/create
//    [HttpPost("Create")]
//    [Authorize(Roles = "Admin")]
//    public async Task<IActionResult> CreateOffer([FromForm] CreateOfferCommand command)
//    {
//        var createdOfferDto = await _mediator.Send(command);
//        return CreatedAtAction(nameof(GetOfferById), new { offerId = createdOfferDto.OfferID }, createdOfferDto);
//    }

//    // PUT: api/offers/{offerId}
//    [HttpPut("{offerId}")]
//    [Authorize(Roles = "Admin")]
//    public async Task<IActionResult> UpdateOffer(int offerId, [FromBody] UpdateOfferCommand command)
//    {
//        if (offerId != command.OfferID) return BadRequest("Offer ID mismatch.");

//        await _mediator.Send(command);

//        return Ok(new { status = "success", data = "Offer updated successfully" });
//    }

//    // DELETE: api/offers/{offerId}
//    [HttpDelete("{offerId}")]
//    [Authorize(Roles = "Admin")]
//    public async Task<IActionResult> DeleteOffer(int offerId)
//    {
//        var command = new DeleteOfferCommand() { OfferID = offerId };
//        await _mediator.Send(command);

//        return Ok(new { status = "success", data = "Offer deleted successfully" });
//    }
//}