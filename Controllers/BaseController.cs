using ElectionWeb.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectionWeb.Controllers
{

	[Authorize]
	public class BaseController : Controller
    {
        public void DisplayMessage(string message)
        {
            TempData[SD.Success] = message; 
        }

        public void DisplayError(string error)
        {
            TempData[SD.Error] = error;
        }
    }
}
