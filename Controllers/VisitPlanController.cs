using Microsoft.AspNetCore.Mvc;
using Unilever.CDExcellent.API.Models.Dto;
using Unilever.CDExcellent.API.Services;

namespace Unilever.CDExcellent.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VisitPlanController : ControllerBase
    {
        private readonly IVisitPlanService _visitPlanService;

        public VisitPlanController(IVisitPlanService visitPlanService)
        {
            _visitPlanService = visitPlanService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateVisitPlan([FromBody] VisitPlanDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _visitPlanService.CreateVisitPlanAsync(dto);
            return CreatedAtAction(nameof(GetVisitPlanById), new { id = result.Id }, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllVisitPlans()
        {
            var visitPlans = await _visitPlanService.GetAllVisitPlansAsync();
            return Ok(visitPlans);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetVisitPlanById(int id)
        {
            var visitPlan = await _visitPlanService.GetVisitPlanByIdAsync(id);
            if (visitPlan == null)
                return NotFound();

            return Ok(visitPlan);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateVisitPlan(int id, [FromBody] VisitPlanDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedVisitPlan = await _visitPlanService.UpdateVisitPlanAsync(id, dto);
            if (updatedVisitPlan == null)
                return NotFound();

            return Ok(updatedVisitPlan);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteVisitPlan(int id)
        {
            var isDeleted = await _visitPlanService.DeleteVisitPlanAsync(id);
            if (!isDeleted)
                return NotFound();

            return NoContent();
        }
    }
}
