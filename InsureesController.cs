using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CarInsurance.Models;

namespace CarInsurance.Controllers
{
public class InsureesController : Controller
{
private CarInsuranceEntities db = new CarInsuranceEntities();

```
    // GET: Insurees
    public ActionResult Index()
    {
        return View(db.Insurees.ToList());
    }

    // GET: Insurees/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: Insurees/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create([Bind(Include =
        "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType")]
        Insuree insuree)
    {
        if (ModelState.IsValid)
        {
            // =========================
            // BASE QUOTE
            // =========================
            decimal quote = 50m;

            // =========================
            // AGE CALCULATION
            // =========================
            int age = DateTime.Now.Year - insuree.DateOfBirth.Year;
            if (insuree.DateOfBirth > DateTime.Now.AddYears(-age))
            {
                age--;
            }

            if (age <= 18)
            {
                quote += 100;
            }
            else if (age >= 19 && age <= 25)
            {
                quote += 50;
            }
            else if (age >= 26)
            {
                quote += 25;
            }

            // =========================
            // CAR YEAR RULES
            // =========================
            if (insuree.CarYear < 2000)
            {
                quote += 25;
            }

            if (insuree.CarYear > 2015)
            {
                quote += 25;
            }

            // =========================
            // CAR MAKE / MODEL RULES
            // =========================
            if (!string.IsNullOrEmpty(insuree.CarMake) &&
                insuree.CarMake.ToLower() == "porsche")
            {
                quote += 25;

                if (!string.IsNullOrEmpty(insuree.CarModel) &&
                    insuree.CarModel.ToLower() == "911 carrera")
                {
                    quote += 25;
                }
            }

            // =========================
            // SPEEDING TICKETS
            // =========================
            quote += insuree.SpeedingTickets * 10;

            // =========================
            // DUI RULE
            // =========================
            if (insuree.DUI)
            {
                quote *= 1.25m;
            }

            // =========================
            // FULL COVERAGE RULE
            // =========================
            if (insuree.CoverageType)
            {
                quote *= 1.50m;
            }

            // SAVE QUOTE
            insuree.Quote = quote;

            db.Insurees.Add(insuree);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        return View(insuree);
    }

    // GET: Admin View (ALL QUOTES)
    public ActionResult Admin()
    {
        var insurees = db.Insurees.ToList();
        return View(insurees);
    }

    // OPTIONAL: Details
    public ActionResult Details(int? id)
    {
        if (id == null)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        Insuree insuree = db.Insurees.Find(id);

        if (insuree == null)
        {
            return HttpNotFound();
        }

        return View(insuree);
    }

    // GET: Delete
    public ActionResult Delete(int? id)
    {
        if (id == null)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        Insuree insuree = db.Insurees.Find(id);

        if (insuree == null)
        {
            return HttpNotFound();
        }

        return View(insuree);
    }

    // POST: Delete
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
        Insuree insuree = db.Insurees.Find(id);
        db.Insurees.Remove(insuree);
        db.SaveChanges();

        return RedirectToAction("Index");
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            db.Dispose();
        }

        base.Dispose(disposing);
    }
}
```

}
