using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server_dotnet.Dtos;
using server_dotnet.Services.Interfaces;

namespace server_dotnet.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class OrganizationsController : ControllerBase
{
    private readonly IOrganizationService _organizationService;

    public OrganizationsController(IOrganizationService organizationService)
    {
        _organizationService = organizationService;
    }

    [HttpGet]
    [ResponseCache(Duration = 600)]
    [ProducesResponseType(typeof(IEnumerable<OrganizationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10)
    {
        var orgs = await _organizationService.GetAllAsync(pageNumber, pageSize);

        return Ok(orgs);
    }

    [HttpGet("{id:int}")]
    [ResponseCache(Duration = 600)]
    [ProducesResponseType(typeof(OrganizationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var org = await _organizationService.GetByIdAsync(id);
        if (org == null) return NotFound();

        return Ok(org);
    }

    [HttpPost]
    [ProducesResponseType(typeof(OrganizationDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] OrganizationCreateUpdateDto dto)
    {
        var createdOrg = await _organizationService.CreateAsync(dto);

        return CreatedAtAction(nameof(GetById), new { id = createdOrg.Id }, createdOrg);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] OrganizationCreateUpdateDto dto)
    {
        await _organizationService.UpdateAsync(id, dto);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await _organizationService.DeleteAsync(id);

        return NoContent();
    }
}

