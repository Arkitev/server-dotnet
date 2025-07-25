﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server_dotnet.Dtos;
using server_dotnet.Helpers;
using server_dotnet.Services.Interfaces;
using server_dotnet_dal.Entities;
using System.Text.Json;

namespace server_dotnet.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<OrderDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status304NotModified)]
    public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10)
    {
        var orders = await _orderService.GetAllAsync(pageNumber, pageSize);

        var json = JsonSerializer.Serialize(orders);
        var eTag = ETagHelper.GenerateETag(json);

        var requestETag = Request.Headers["If-None-Match"].FirstOrDefault();
        if (requestETag == eTag)
        {
            return StatusCode(StatusCodes.Status304NotModified);
        }

        Response.Headers["ETag"] = eTag;

        return Ok(orders);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status304NotModified)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var order = await _orderService.GetByIdAsync(id);
        if (order == null) return NotFound();

        var json = JsonSerializer.Serialize(order);
        var eTag = ETagHelper.GenerateETag(json);

        var requestETag = Request.Headers["If-None-Match"].FirstOrDefault();
        if (requestETag == eTag)
        {
            return StatusCode(StatusCodes.Status304NotModified);
        }

        Response.Headers["ETag"] = eTag;

        return Ok(order);
    }

    [HttpPost]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] OrderCreateUpdateDto dto)
    {
        var createdOrder = await _orderService.CreateAsync(dto);

        return CreatedAtAction(nameof(GetById), new { id = createdOrder.Id }, createdOrder);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] OrderCreateUpdateDto dto)
    {
        await _orderService.UpdateAsync(id, dto);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await _orderService.DeleteAsync(id);

        return NoContent();
    }
}
