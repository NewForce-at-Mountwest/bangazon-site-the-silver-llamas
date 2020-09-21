using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bangazon.Data;
using Bangazon.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Bangazon.Models.ProductViewModels;

namespace Bangazon.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;
        public ProductsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        } 
      
        //
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: Products
        public async Task<IActionResult> Index()
        {
            //added user information, so it would only show the user's products for sale
            ApplicationUser loggedInUser = await GetCurrentUserAsync();
           
           

                var product = await _context.Product
                    .Include(p => p.ProductType)
                    .Include(p => p.User)
                    .Where(product => product.UserId == loggedInUser.Id)
                    .ToListAsync();
                return View(product);
          
            //return View(product);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.ProductType)
                .Include(p => p.User)
                .Include(p => p.OrderProducts)
                .FirstOrDefaultAsync(m => m.ProductId == id);

            //Take the quantity and subtract the count for OrderProducts to get the current inventory count for the product.
            product.Quantity -= product.OrderProducts.Count();
            
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            //create an instance of the ProductCreateViewModel to get a list of ProductTypes for the dropdown
            ProductCreateViewModel ViewModel = new ProductCreateViewModel();

            //then use the view model rather than view data for more flexibility
            ViewModel.productTypes = _context.ProductType.Select(c => new SelectListItem
            {
                Text = c.Label,
                Value = c.ProductTypeId.ToString()
            }
            ).ToList();

            //forces user to choose a product category before continuing.  Error message displays due to 
            //data annotation on ProductTypeId requiring the foreign key to be greater than 0 or display
            //an error message.  Otherwise, this will give the productType an id of 0 and try to send to database
            ViewModel.productTypes.Insert(0, new SelectListItem() { Value = "0", Text = "--Select Product Category--" });

            //ViewData["ProductTypeId"] = new SelectList(_context.ProductType, "ProductTypeId", "Label");
            //ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id");
            return View(ViewModel);
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,DateCreated,Description,Title,Price,Quantity,City,ImagePath,Active,ProductTypeId")] Product product)
        {
            //We weren't getting a user, we were only getting a userId, so we needed to remove User from the ModelState so the conditional would return true.
            //We may need to change it to product.User later if we use a view model.
            ModelState.Remove("product.User");
            ModelState.Remove("product.UserId");
            
            ProductCreateViewModel ViewModel = new ProductCreateViewModel();
            if (ModelState.IsValid)
            {
                //We added the next two lines to get the user to check the ID.
                var user = await GetCurrentUserAsync();
                product.UserId = user.Id;
                _context.Add(product);
                await _context.SaveChangesAsync();
                //redirects to the new product details by selecting the details view along with the new parameter
                return RedirectToAction("Details", new
                {
                   id = product.ProductId
                });
            }
            //you can put this in an Else, but it basically is an else statement because of the return above
            ViewModel.productTypes = _context.ProductType.Select(c => new SelectListItem
            {
                Text = c.Label,
                Value = c.ProductTypeId.ToString()
            }).ToList();

            

            //ViewData["ProductTypeId"] = new SelectList(_context.ProductType, "ProductTypeId", "Label", product.ProductTypeId);
            //ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", product.UserId);
            return View(ViewModel);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["ProductTypeId"] = new SelectList(_context.ProductType, "ProductTypeId", "Label", product.ProductTypeId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", product.UserId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,DateCreated,Description,Title,Price,Quantity,UserId,City,ImagePath,Active,ProductTypeId")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            ModelState.Remove("User");

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await GetCurrentUserAsync();
                    product.UserId = user.Id;
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductTypeId"] = new SelectList(_context.ProductType, "ProductTypeId", "Label", product.ProductTypeId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", product.UserId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.ProductType)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Product product = await _context.Product.FindAsync(id);
            try
            {
                _context.Product.Remove(product);
                await _context.SaveChangesAsync();
            
            }
            catch (Exception) when (product.Active == true)
            {
                product.Active = false;
                _context.Update(product);
                await _context.SaveChangesAsync();
            }
            //var product = await _context.Product.FindAsync(id);
           // _context.Product.Remove(product);
           // await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }
    }
}
