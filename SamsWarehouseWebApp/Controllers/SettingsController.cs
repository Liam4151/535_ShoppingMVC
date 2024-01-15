using Microsoft.AspNetCore.Mvc;

namespace SamsWarehouseWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : Controller
    {
        /// <summary>
        /// Theme method that sends a HTTP Post request that is used to switch themes. 
        /// Sets up "theme" string to be used in switchTheme script function. 
        /// </summary>
        /// <param name="themeSwitch"></param>
        /// <returns></returns>
        [HttpPost("SetTheme")]
        public async Task<IActionResult> SetTheme([FromBody] ThemeSetting themeSwitch)
        {
            HttpContext.Session.SetString("theme", themeSwitch.Theme);
            return Ok();
        }
        /// <summary>
        /// Theme Setting class that creates the theme string to be used in controller method. 
        /// </summary>
        public class ThemeSetting
        {
            public string Theme { get; set; }
        }
    }
}
