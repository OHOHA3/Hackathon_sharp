using Hackathon.Employees;

namespace Hackathon.Util
{
    public static class WishListCreator
    {
        private static readonly Random Random = new(1);

        public static List<Wishlist> CreateWishList(
            List<Employee> employees, 
            List<Employee> employeesToWishList)
        {
            return employees.Select(employee => new Wishlist(
                employee.Id,
                employeesToWishList
                    .Select(e => e.Id)
                    .OrderBy(_ => Random.Next())
                    .ToArray())).ToList();
        }
    }
}