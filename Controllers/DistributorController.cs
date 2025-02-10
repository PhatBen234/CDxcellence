using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unilever.CDExcellent.API.Models.Dto;
using Unilever.CDExcellent.API.Services.IService;

namespace Unilever.CDExcellent.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // 🔒 Yêu cầu đăng nhập cho tất cả API
    public class DistributorController : ControllerBase
    {
        private readonly IDistributorService _distributorService;

        public DistributorController(IDistributorService distributorService)
        {
            _distributorService = distributorService;
        }

        // 🔒 Chỉ Admin hoặc Owner được tạo Distributor
        [Authorize(Roles = "Admin,Owner")]
        [HttpPost]
        public async Task<IActionResult> CreateDistributor([FromBody] DistributorDto dto)
        {
            try
            {
                var createdDistributor = await _distributorService.CreateDistributorAsync(dto);
                return CreatedAtAction(nameof(GetDistributorById), new { id = createdDistributor.Id }, createdDistributor);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // 🔒 Phải đăng nhập mới xem danh sách Distributor
        [HttpGet]
        public async Task<IActionResult> GetAllDistributors()
        {
            var distributors = await _distributorService.GetAllDistributorsAsync();
            return Ok(distributors);
        }

        // 🔒 Phải đăng nhập mới xem chi tiết Distributor
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDistributorById(int id)
        {
            var distributor = await _distributorService.GetDistributorByIdAsync(id);
            if (distributor == null)
                return NotFound();

            return Ok(distributor);
        }

        // 🔒 Chỉ Admin hoặc Owner được cập nhật Distributor
        [Authorize(Roles = "Admin,Owner")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDistributor(int id, [FromBody] DistributorDto dto)
        {
            var updatedDistributor = await _distributorService.UpdateDistributorAsync(id, dto);
            if (updatedDistributor == null)
                return NotFound();

            return Ok(updatedDistributor);
        }

        // 🔒 Chỉ Admin hoặc Owner được xóa Distributor
        [Authorize(Roles = "Admin,Owner")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDistributor(int id)
        {
            var result = await _distributorService.DeleteDistributorAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
