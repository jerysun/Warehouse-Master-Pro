using API.DTOs;
using API.Extensions;
using AutoMapper;
using Core;
using Core.IRepositories;
using Core.Models;
using Core.UseCases;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection.PortableExecutable;

namespace API.Controllers;
[Route("[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IRepository<int, Product> _productRepository;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IRepository<int, Product> productRepository, IMapper mapper, ILogger<ProductsController> logger, IMediator mediator)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetProductsAsync([FromQuery] EntityParameters entityParameters, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting products...");

        var products = await _productRepository.AllByPagesAsync(entityParameters);
        var metadata = _mapper.Map<PagedDto>(products);

        Response.AddPaginationHeader(metadata);
        _logger.LogInformation($"Returned {metadata.TotalCount} products from database.");

        var res = products.Select(p => new ProductDetailsDto
        (
            Id: p.Id,
            Name: p.Name,
            QuantityInStock: p.QuantityInStock
        ));

        return Ok(res);
    }

    [HttpGet("{productId}")]
    public async Task<IActionResult> GetProductByIdAsync(int productId, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(productId, cancellationToken);

        if (product != null)
        {
            return Ok(product);
        }
        return BadRequest();
    }

    [HttpPost]
    [Route("{productId}/add-stocks")]
    public async Task<IActionResult> CreateProductAsync(int productId, IncreaseStock.Command command, CancellationToken cancellationToken)
    {
        try
        {
            command.ProductId = productId;
            var quantityInStock = await _mediator.Send(command, cancellationToken);
            var stockLevel = new StockLevel(quantityInStock);
            return Ok(stockLevel);
        }
        catch (ProductNotFoundException ex)
        {
            return NotFound(_mapper.Map<ProductNotFound>(ex));
        }
    }

    [HttpPost]
    [Route("{productId}/remove-stocks")]
    public async Task<IActionResult> RemoveProductAsync(int productId, DecreaseStocks.Command command, CancellationToken cancellationToken)
    {
        try
        {
            command.ProductId = productId;
            var quantityInStock = await _mediator.Send(command, cancellationToken);
            var stockLevel = new StockLevel(quantityInStock);
            return Ok(stockLevel);
        }
        catch (NotEnoughStockException ex)
        {
            return Conflict(_mapper.Map<NotEnoughStock>(ex));
        }
        catch (ProductNotFoundException ex)
        {
            return NotFound(_mapper.Map<ProductNotFound>(ex));
        }
    }
}
