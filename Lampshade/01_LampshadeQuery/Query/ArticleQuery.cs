using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Application;
using _01_LampshadeQuery.Contracts.Article;
using _01_LampshadeQuery.Contracts.Product;
using BlogManagement.Infrastructure.EFCore;
using CommentManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;

namespace _01_LampshadeQuery.Query
{
    public class ArticleQuery : IArticleQuery
    {
        private readonly BlogContext _context;
        private readonly CommentContext _commentContext;

        public ArticleQuery(BlogContext context, CommentContext commentContext)
        {
            _context = context;
            _commentContext = commentContext;
        }

        public List<ArticleQueryModel> LatestArticles()
        {
            return _context.Articles
                .Include(x => x.Category)
                .Where(x => x.PublishDate < DateTime.Now)
                .Select(x => new ArticleQueryModel
                {
                    Id = x.Id,
                    ShortDescription = x.ShortDescription,
                    Picture = x.Picture,
                    PictureAlt = x.PictureAlt,
                    Slug = x.Slug,
                    Title = x.Title,
                    PictureTitle = x.PictureTitle,
                    PublishDate = x.PublishDate.ToFarsi(),
                }).ToList();
        }

        public ArticleQueryModel GetArticleDetails(string slug)
        {
            var article = _context.Articles
                .Include(x => x.Category)
                .Where(x => x.PublishDate < DateTime.Now)
                .Select(x => new ArticleQueryModel
                {
                    Id = x.Id,
                    Slug = x.Slug,
                    CanonicalAddress = x.CanonicalAddress,
                    CategorySlug = x.Category.Slug,
                    Category = x.Category.Name,
                    CategoryId = x.CategoryId,
                    ShortDescription = x.ShortDescription,
                    Picture = x.Picture,
                    PictureAlt = x.PictureAlt,
                    PictureTitle = x.PictureTitle,
                    PublishDate = x.PublishDate.ToFarsi(),
                    Keywords = x.Keywords,
                    Description = x.Description,
                    MetaDescription = x.MetaDescription,
                }).FirstOrDefault(x => x.Slug == slug);

            if (!string.IsNullOrWhiteSpace(article.Keywords))
            {
                article.KeywordList = article.Keywords.Split(",").ToList();
            }

            var Comments = _commentContext.Comments
                .Where(x => x.Type == CommentType.Article)
                .Where(x => !x.IsCanceled)
                .Where(x => x.IsConfirmed)
                .Where(x => x.OwnerRecordId == article.Id)
                .Select(x => new CommentQueryModel()
                {
                    Id = x.Id,
                    Message = x.Message,
                    ParentId = x.ParentId,
                    Name = x.Name,
                    CreationDate = DateTime.Now.ToFarsi(),
                })
                .ToList();

            foreach (var comment in Comments)
            {
                if (comment.ParentId > 0)
                {
                    comment.ParentName = Comments.FirstOrDefault(x => x.Id == comment.ParentId)?.Name;
                }
            }

            article.Comments = Comments;
            return article;
        }
    }
}
