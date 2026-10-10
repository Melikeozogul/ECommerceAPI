using ECommerceAPI.Application.DTOs;
using ECommerceAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;

namespace ECommerceAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IValidator<CreateProductDto> _validator;

    public ProductsController(
        IProductService productService,
        IValidator<CreateProductDto> validator)
    {
        _productService = productService;
        _validator = validator;
    }

    [Authorize(Roles = "Admin")]

    [HttpGet]
    public async Task<ActionResult<List<ProductDto>>> GetAll()
    {
        var products = await _productService.GetAllAsync();

        return Ok(products);
    }

    [Authorize(Roles = "Admin")]

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetById(int id)
    {
        var product = await _productService.GetByIdAsync(id);

        if (product is null)
            return NotFound();

        return Ok(product);
    }

    [Authorize(Roles = "Admin")]

    [HttpPost]
public async Task<ActionResult<ProductDto>> Create(
    CreateProductDto productDto)
{
    var validationResult = await _validator.ValidateAsync(productDto);

    if (!validationResult.IsValid)
    {
        return BadRequest(validationResult.Errors);
    }

    var product = await _productService.CreateAsync(productDto);

    return CreatedAtAction(
        nameof(GetById),
        new { id = product.Id },
        product);
}

    [Authorize(Roles = "Admin")]

    [HttpPut("{id}")]
public async Task<ActionResult<ProductDto>> Update(
    int id,
    UpdateProductDto productDto)
{
    var product = await _productService.UpdateAsync(id, productDto);

    if (product is null)
        return NotFound();

    return Ok(product);
}
    [Authorize(Roles = "Admin")]

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _productService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }

    
}