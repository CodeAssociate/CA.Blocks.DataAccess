using System.Collections.Generic;
using System.Linq;

namespace CA.
    Blocks.DataAccess.Paging
{
    public class PagingRequest
    {
        public PagingRequest()
        {

        }

        // please note that is merely a way to avoid specifying an ordering and does NOT guarantee that any original data ordering will be preserved.
        // There are other factors that can cause the result to be ordered, such as an ORDER BY in the outer query.
        // However if there are no other order by clauses this is a very convenient way to get paging without an order by.
        public PagingRequest(int take, int skip)
        {
            Take = take;
            Skip = skip;
            SortOrder = new List<Sort> { new Sort { Field = "(Select 1)", Dir = "ASC" } };
        }

        public PagingRequest(int take, int skip, string defaultOrderByCol)
        {
            Take = take;
            Skip = skip;
            SortOrder = new List<Sort> { new Sort { Field = defaultOrderByCol, Dir = "ASC" } };
        }
        public PagingRequest(int take, int skip, string defaultOrderByCol, string dir)
        {
            Take = take;
            Skip = skip;
            SortOrder = new List<Sort> { new Sort { Field = defaultOrderByCol, Dir = dir } };
        }


        public int Take { get; set; }
        public int Skip { get; set; }
        public IList<Sort> SortOrder { get; set; }
        public string GetOrderBy()
        {
            return SortOrder.Count > 0 ? string.Join(",", SortOrder.Select(sort => $"{sort.Field} {sort.Dir}").ToArray()) : "1 asc";
        }
    }
}
