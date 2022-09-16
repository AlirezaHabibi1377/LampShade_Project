using _01_LampshadeQuery.Contracts.ArticleCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Application;
using BlogManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using _01_LampshadeQuery.Contracts.Article;
using BlogManagement.Domain.ArticleAgg;

namespace _01_LampshadeQuery.Query
{
    public class ArticleCategoryQuery : IArticleCategoryQuery
    {
        private readonly BlogContext _context;

        public ArticleCategoryQuery(BlogContext context)
        {
            _context = context;
        }

        public ArticleCategoryQueryModel GetArticleCategory(string slug)
        {
            var articleCategory = _context.ArticleCategories
                .Include(x=>x.Articles)
                .Select(x => new ArticleCategoryQueryModel
            {
                Slug = x.Slug,
                Name = x.Name,
                Description = x.Description,
                ArticlesCount = x.Articles.Count,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                Keywords = x.Keywords,
                MetaDescription = x.MetaDescription,
                CanonicalAddress = x.CanonicalAddress,
                Articles = MapArticles(x.Articles)
            })
                .FirstOrDefault(x => x.Slug == slug);

            if (!string.IsNullOrWhiteSpace(articleCategory.Keywords))
            {
                articleCategory.KeywordList = articleCategory.Keywords.Split(",").ToList();
            }
            

            return articleCategory;
        }

        private static List<ArticleQueryModel> MapArticles(List<Article> articles)
        {
            return articles.Select(x => new ArticleQueryModel
            {
                Slug = x.Slug,
                Title = x.Title,
                ShortDescription = x.ShortDescription,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                PublishDate = x.PublishDate.ToFarsi()
            }).ToList();
        }

        public List<ArticleCategoryQueryModel> GetArticleCategories()
        {
            return _context.ArticleCategories
                .Include(x => x.Articles)
                .Select(x => new ArticleCategoryQueryModel
                {
                    Picture = x.Picture,
                    ArticlesCount = x.Articles.Count,
                    Name = x.Name,
                    PictureTitle = x.PictureTitle,
                    Slug = x.Slug,
                    PictureAlt = x.PictureAlt
                }).ToList();
        }
    }
}
