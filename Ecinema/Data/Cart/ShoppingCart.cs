using Ecinema.Data;
using Ecinema.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace Ecinema.Data.Cart
{
    public class ShoppingCart
    {
        public AppDbContext Context { get; set; }

        public string ShoppingCartId { get; set; }
        public List<ShoppingCartItem> ShoppingCartItems { get; set; }

        public ShoppingCart(AppDbContext context)
        {
            Context = context;
        }

        public static ShoppingCart GetShoppingCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            var context = services.GetService<AppDbContext>();

            // Get the current user's ID
            string userId = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Generate a unique cart ID if user is not authenticated
            string cartId = userId ?? session.GetString("CartId") ?? Guid.NewGuid().ToString();

            // If the user is authenticated, store the cart ID in session
            if (userId != null)
            {
            session.SetString("CartId", cartId);
            }

            return new ShoppingCart(context) { ShoppingCartId = cartId };
        }

        public void AddItemToCart(Movie movie)
        {
            var shoppingCartItem = Context.ShoppingCartItems.FirstOrDefault(n => n.Movie.Id == movie.Id && n.ShoppingCartId == ShoppingCartId);

            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem()
                {
                    ShoppingCartId = ShoppingCartId,
                    Movie = movie,
                    Amount = 1
                };

                Context.ShoppingCartItems.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Amount++;
            }
            Context.SaveChanges();
        }

        public void RemoveItemFromCart(Movie movie)
        {
            var shoppingCartItem = Context.ShoppingCartItems.FirstOrDefault(n => n.Movie.Id == movie.Id && n.ShoppingCartId == ShoppingCartId);

            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.Amount > 1)
                {
                    shoppingCartItem.Amount--;
                }
                else
                {
                    Context.ShoppingCartItems.Remove(shoppingCartItem);
                }
            }
            Context.SaveChanges();
        }

        public List<ShoppingCartItem> GetShoppingCartItems()
        {
            List<ShoppingCartItem>  ShoppingCartItems = Context.ShoppingCartItems.Where(n => n.ShoppingCartId == ShoppingCartId).Include(n => n.Movie).ToList();
            return ShoppingCartItems;
        }

        public double GetShoppingCartTotal() => Context.ShoppingCartItems.Where(n => n.ShoppingCartId == ShoppingCartId).Select(n => n.Movie.Price * n.Amount).Sum();

        public async Task ClearShoppingCartAsync()
        {
            var items = await Context.ShoppingCartItems.Where(n => n.ShoppingCartId == ShoppingCartId).ToListAsync();
            Context.ShoppingCartItems.RemoveRange(items);
            await Context.SaveChangesAsync();
        }
    }
}
