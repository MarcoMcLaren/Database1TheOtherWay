using INF272DB1StudentFiles2022.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace INF272DB1StudentFiles2022.Controllers
{
    public class HomeController : Controller
    {
        private DataService dataService = DataService.getDataService();

        //complete the missing code and adjust given code where necessary
        public ActionResult Index()
        { //to display a list of records currently in the database in the Index view

            List<DestinationModel> databaseUsers = dataService.getDest();
            return View(databaseUsers);  
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            //To display the chosen record in the update view
            DestinationModel foundModel = dataService.getDestById(id);
            if (foundModel == null)
            {
                return RedirectToAction("Error");
            }
            return View(foundModel);
          
        }

        [HttpPost]
        public ActionResult Update(DestinationModel someDest)
        {//to return the updated record to the Index view and update it in the database

            dataService.updateDest(someDest);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Add()
        {//to open the Add view
            return View();
        }

        [HttpPost]
        public ActionResult Add(DestinationModel someDest)
        {//to add a new record to the database and display it on the Index view
            if (someDest.ID == 0)
            {
                someDest.ID = -1;
            }

            dataService.createDest(someDest);
            return RedirectToAction("Index");

        }

        public ActionResult Delete(int id)
        {//to delete the chosen record from the database and show it on the Index view
            dataService.deleteDest(id);
            return RedirectToAction("Index");

        }

    }
}