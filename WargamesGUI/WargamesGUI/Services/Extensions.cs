using System.Collections.Generic;
using System.Linq;
using WargamesGUI.Models;

namespace WargamesGUI.Services
{
    public static class Extensions
    {
        public static List<string> FilterSearchBookList(this List<Book2> list, string search)
        {
            var titleResult = list.Select(x => x.Title).Where(x => x != null && x.ToUpper().Contains(search.ToUpper())).ToList();
            var publisherResult = list.Select(x => x.Publisher).Where(x => x != null && x.ToUpper().Contains(search.ToUpper())).ToList();
            var authorResult = list.Select(x => x.Author).Where(x => x != null && x.ToUpper().Contains(search.ToUpper())).ToList();
            var isbnResult = list.Select(x => x.ISBN).Where(x => x != null && x.ToUpper().Contains(search.ToUpper())).ToList();
            var subCategoryResult = list.Select(x => x.DeweySub.SubCategoryName).Where(x => x != null && x.ToUpper().Contains(search.ToUpper())).ToList();
            var mainCategoryResult = list.Select(x => x.DeweyMain.MainCategoryName).Where(x => x != null && x.ToString().ToUpper().Contains(search.ToUpper())).ToList();
            return titleResult.Concat(publisherResult).Concat(authorResult).Concat(isbnResult).Concat(subCategoryResult).Concat(mainCategoryResult).Distinct().ToList() ?? null;
        }
    }
}
