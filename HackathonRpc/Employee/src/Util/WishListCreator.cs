using Contracts;

namespace Employee.Util
{
    public static class WishListCreator
    {
        private static readonly Random Random = new(1);

        public static List<WishList> CreateWishList(
            List<Contracts.Employee> employees,
            List<Contracts.Employee> employeesToWishList)
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