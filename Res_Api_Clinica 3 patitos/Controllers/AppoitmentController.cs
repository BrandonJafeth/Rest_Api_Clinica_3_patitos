using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Res_Api_Clinica_3_patitos.Controllers
{
    public class AppoitmentController : Controller
    {
        // GET: AppoitmentController
        public ActionResult Index()
        {
            return View();
        }

        // GET: AppoitmentController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AppoitmentController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AppoitmentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AppoitmentController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AppoitmentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AppoitmentController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AppoitmentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
