    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using WaterTrackerApp.Application.Dtos;
    using WaterTrackerApp.Application.Interfaces;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    namespace WaterTrackerApp.API.Controllers
    {
        /// <summary>
        /// API controller for managing water intake records.
        /// </summary>
        [Route("api/[controller]")]
        [ApiController]
        public class WaterIntakeController : ControllerBase
        {
            private readonly IWaterIntakeService _waterIntakeService;

            /// <summary>
            /// Initializes a new instance of the "WaterIntakeController" class.
            /// </summary>
            /// <param name="waterIntakeService">Service for water intake operations.</param>
            public WaterIntakeController(IWaterIntakeService waterIntakeService)
            {
                _waterIntakeService = waterIntakeService;
            }

            /// <summary>
            /// Gets all water intake records for a specific user.
            /// </summary>
            /// <param name="userId">The user's unique identifier.</param>
            /// <returns>A list of water intake records for the user.</returns>
            [HttpGet("user/{userId}")]
            public async Task<ActionResult<IEnumerable<WaterIntakeDto>>> GetByUserId(int userId)
            {
                var records = await _waterIntakeService.GetByUserIdAsync(userId);
                return Ok(records);
            }

            /// <summary>
            /// Gets a specific water intake record by its unique identifier.
            /// </summary>
            /// <param name="id">The water intake record's unique identifier.</param>
            /// <returns>The water intake record if found; otherwise, NotFound.</returns>
            [HttpGet("{id}")]
            public async Task<ActionResult<WaterIntakeDto>> GetById(int id)
            {
                var record = await _waterIntakeService.GetByIdAsync(id);
                if (record == null) return NotFound();
                return Ok(record);
            }

            /// <summary>
            /// Creates a new water intake record.
            /// </summary>
            /// <param name="dto">The water intake data transfer object.</param>
            /// <returns>The created water intake record.</returns>
            [HttpPost]
            public async Task<ActionResult<WaterIntakeDto>> Create(WaterIntakeDto dto)
            {
                var created = await _waterIntakeService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }

            /// <summary>
            /// Updates an existing water intake record.
            /// </summary>
            /// <param name="id">The unique identifier of the water intake record to update.</param>
            /// <param name="dto">The updated water intake data transfer object.</param>
            /// <returns>No content if successful; otherwise, NotFound.</returns>
            [HttpPut("{id}")]
            public async Task<IActionResult> Update(int id, WaterIntakeDto dto)
            {
                var success = await _waterIntakeService.UpdateAsync(id, dto);
                if (!success) return NotFound();
                return NoContent();
            }

            /// <summary>
            /// Deletes a water intake record by its unique identifier.
            /// </summary>
            /// <param name="id">The unique identifier of the water intake record to delete.</param>
            /// <returns>No content if successful; otherwise, NotFound.</returns>
            [HttpDelete("{id}")]
            public async Task<IActionResult> Delete(int id)
            {
                var success = await _waterIntakeService.DeleteAsync(id);
                if (!success) return NotFound();
                return NoContent();
            }
        }
    }
