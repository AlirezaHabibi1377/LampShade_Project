using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Domain;
using BlogManagement.Application.Contracts.ArticleCategory;

namespace BlogManagement.Domain.ArticleCategoryAgg
{
    public interface IArticleCategoryRepository : IRepository<long, ArticleCategory>
    {
        List<ArticleCategoryViewModel> Search(ArticleCategorySearchModel searchModel);
        EditArticleCategory GetDetails(long id);
    }
}
