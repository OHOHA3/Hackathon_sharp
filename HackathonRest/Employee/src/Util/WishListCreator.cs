using Employee.Rest;

namespace Employee.Util
{
    public static class WishListCreator
    {
        private static readonly Random Random = new(1);

        public static List<WishList> CreateWishList(
            List<Rest.Employee> employees,
            List<Rest.Employee> employeesToWishList)
        {
            return employees.Select(employee => new WishList(
                employee.Id,
                employeesToWishList
                    .Select(e => e.Id)
                    .OrderBy(_ => Random.Next())
                    .ToArray())).ToList();
        }
    }
}