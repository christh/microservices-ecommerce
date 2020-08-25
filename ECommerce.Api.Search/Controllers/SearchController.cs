using ECommerce.Api.Search.Interfaces;
using ECommerce.Api.Search.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Controllers
{
    [ApiController]
    [Route("api/search")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService searchService;

        public SearchController(ISearchService searchService)
        {
            this.searchService = searchService;
        }

        [HttpPost]
        public async Task<IActionResult> SearchAsync(SearchTerm term)
        {
            var (IsSuccess, SearchResult) = await searchService.SearchAsync(term.CustomerId);

            if (IsSuccess)
            {
                return Ok(SearchResult);
            }

            return NotFound();

        }

    }
}
