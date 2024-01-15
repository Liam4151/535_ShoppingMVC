using Ganss.Xss;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SamsWarehouseWebApp.Models;
using SamsWarehouseWebApp.Models.Data;
using SamsWarehouseWebApp.Services;

namespace SamsWarehouseWebApp.Controllers
{
    public class ListController : Controller
    {
        private readonly ItemDBContext _dbcontext;
        private readonly SanitiserService _sanitiserService;


        public ListController(ItemDBContext dbcontext, SanitiserService sanitiserService)
        {
            _dbcontext = dbcontext;
            _sanitiserService = sanitiserService;
        }
        /// <summary>
        /// Index controller method that displays Shopping Lists page
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            //if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString("ID")))
            //{
            //    return RedirectToAction("Login", "Home");
            //}

            ViewBag.UserId = HttpContext.Session.GetString("ID");

            return View();
        }
        /// <summary>
        /// Creates and displays a drop-down list for all user lists. First the method checks the user's ID to authorise the user to use the
        /// drop-down list. The user's ID is then used to display all lists created by that user. If no lists have been made for that user, "No entries" 
        /// will be displayed in the drop-down list. When a list option is selected in the DDL, a partial view is returned to display the list's items. 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ShoppingListDDL()
        {
            if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString("ID")))
            {
                return Unauthorized();
            }

            string userId = HttpContext.Session.GetString("ID");

            var user = _dbcontext.Users.Where(c => c.UserId == int.Parse(userId))
                                    .Include(c => c.Lists).FirstOrDefault();

            if (user == null)
            {
                return BadRequest();
            }

            var selectList = user.Lists.Select(c => new SelectListItem
            {
                Text = c.ListName,
                Value = c.ListId.ToString()
            }).ToList();

            if (selectList.Count == 0)
            {
                selectList.Add(new SelectListItem
                {
                    Text = "No Entries",
                    Value = "0"
                });
            }

            ViewBag.SelectList = selectList;

            return PartialView("_ListDDL");
        }

        /// <summary>
        /// Add New List for user. Retrieves ID from session and checks if user is authorised to create a new list. 
        /// </summary>
        /// <param name="listName"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddNewList([FromBody] string listName)
        {
            //var content = _sanitiserService.Sanitiser.Sanitize(collection["Content"]);

            // retrieve ID from session
            string id = HttpContext?.Session?.GetString("ID");

            //var sanitizer = new HtmlSanitizer();

            //string userInput = "<p>This is <b>some</b> <script>alert('dangerous script!');</script> HTML input.</p>";
            //string sanitizedHtml = sanitizer.Sanitize(userInput);

            //HtmlSanitizer sanitizer = new HtmlSanitizer();
            //sanitizer.AllowedAttributes.Add("class");

            //var content = sanitizer.Sanitize(listName);

            //var content = collection["Content"];

            //string query = "SELECT * FROM Entries WHERE Type='Sanitised' AND Content LIKE '" + content + "'";

            //var enteredListName = _dbcontext.Lists.FromSqlRaw(query).ToList();

            if (!int.TryParse(id, out int userID))
            {
                return Unauthorized();
            }

            if (_dbcontext.Lists.Any(c => c.ListName == listName && c.UserId == userID))
            {
                return BadRequest();
            }

            List newList = new List()
            {
                ListName = listName,
                UserId = userID
            };
            _dbcontext.Lists.Add(newList);
            _dbcontext.SaveChanges();

            return Ok();
        }
        /// <summary>
        /// Retrieves all items for a user's selected list 
        /// </summary>
        /// <param name="listID"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetItemsForList([FromQuery] int listID)
        {
            var date = _dbcontext.Lists.Where (c => c.ListId == listID).FirstOrDefault().Created;
            ViewBag.CreatedDate = date;
            List<ListItems> items = _dbcontext.ListItems.Include(c => c.Item)
                                                                 .Where(c => c.ListId == listID).ToList();
            //.Select(c => c.Item)
            //.ToList();



            return PartialView("_ItemsForListPartial", items);
        }

        /// <summary>
        /// Removes an item from the selected user list
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> RemoveItemFromList([FromBody] ListItems item)
        {
            var listItem = _dbcontext.ListItems.Where(c => c.ListId == item.ListId && c.ItemId == item.ItemId)
                                                         .FirstOrDefault();
            try
            {
                if (listItem != null)
                {
                    _dbcontext.ListItems.Remove(listItem);
                    _dbcontext.SaveChanges();
                    return Ok();
                }
                return BadRequest();
            }

            catch (Exception e)
            {

                throw;
            }
        }


        /// <summary>
        /// Adds Item To a Shopping List
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> AddItemToList([FromBody] ListItems item)
        {
            try
            {
                if (_dbcontext.ListItems.Any(c => c.ListId == item.ListId && c.ItemId == item.ItemId && c.Quantity == item.Quantity))
                {
                    return BadRequest();
                }

                _dbcontext.ListItems.Add(item);
                _dbcontext.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                return Problem();
            }
        }
        /// <summary>
        /// List controller method used to delete a user's selected shopping list 
        /// </summary>
        /// <param name="listId"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult> DeleteShoppingList([FromBody] int listID)
        {
            if (listID != null)
            {
                var listToDelete = _dbcontext.Lists.Where(c => c.ListId == listID).FirstOrDefault();
                _dbcontext.Lists.Remove(listToDelete);
                _dbcontext.SaveChanges();

                return Ok();
            }
            return BadRequest();
        }
    }
}
