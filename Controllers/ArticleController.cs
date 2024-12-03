using ArticleManagement.Models;
using ArticleManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace ArticleManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticleController: ControllerBase
    {
        private readonly ArticleService _articleService;

        public ArticleController(ArticleService articleService) =>
            _articleService = articleService;

        [HttpGet]
        public async Task<List<Article>> Get() =>
        
            await _articleService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Article>> Get(string id)
        {
            var article = await _articleService.GetAsync(id);

            if (article is null)
            {
                return NotFound();
            }

            return article;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Article newArticle)
        {
            await _articleService.CreateAsync(newArticle);

            return CreatedAtAction(nameof(Get), new { id = newArticle.ArticleId }, newArticle);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Article updatedArticle)
        {
            var article = await _articleService.GetAsync(id);

            if (article is null)
            {
                return NotFound();
            }

            updatedArticle.ArticleId = article.ArticleId;

            await _articleService.UpdateAsync(id, updatedArticle);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var article = await _articleService.GetAsync(id);

            if (article is null)
            {
                return NotFound();
            }

            await _articleService.RemoveAsync(id);

            return NoContent();
        }
    }
}
