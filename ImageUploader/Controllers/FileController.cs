using System.Web;
using System.Web.Mvc;
using ImageUploader.Models;
using ImageUploader.DB;
using System.IO;

namespace ImageUploader.Controllers
{
    public class FileController : Controller
    {
        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(Person person, HttpPostedFileBase file,string saveLike)
        {
            if (file == null)
            {
                ModelState.AddModelError("FileError", "Pelase select your photo!");
                return View();
            }

            if (ModelState.IsValid)
            {
                SQLiteDA da = new SQLiteDA();
                switch (saveLike)
                {
                    case "Database":
                        if (file.ContentLength > (2 * 1024 * 1024))
                        {
                            ModelState.AddModelError("FileError", "File size must be less than 2 MB.");
                            return View();
                        }

                        if (file.ContentType != "image/jpeg" && file.ContentType != "image/gif")
                        {
                            ModelState.AddModelError("FileError", "File must be jpg or gif.");
                            return View();
                        }

                        person.ImageSize = file.ContentLength;

                        byte[] data = new byte[file.ContentLength];

                        file.InputStream.Read(data, 0, file.ContentLength);
                        person.ImageData = data;
                        da.InsertPerson(person);
                        break;

                    case "Disk":
                        if (file.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            var path = Path.Combine(Server.MapPath("~/Content/files"), fileName);
                            file.SaveAs(path);
                        }
                        if (file.ContentType != "image/jpeg" && file.ContentType != "image/gif")
                        {
                            ModelState.AddModelError("FileError", "File must be jpg or gif.");
                            return View();
                        }

                        person.ImageName = file.FileName;
                        person.ImageSize = file.ContentLength;
                        da.InsertPerson(person);
                        break;
                }
            }else return View();
                
            return RedirectToAction("Index");
        }

        public ActionResult Index() 
        {
            SQLiteDA da = new SQLiteDA();        
            return View(da.GetPersonList());
        }

        public ActionResult Edit(int itemID)
        {
            SQLiteDA da = new SQLiteDA();
            return View(da.GetSinglePerson(itemID));
        }

        [HttpPost]
        public ActionResult Edit(Person person, HttpPostedFileBase file)
        {
            SQLiteDA da = new SQLiteDA();
            if (ModelState.IsValid)
            {
                if (file == null) 
                {
                    da.UpdatePerson(person);
                    return RedirectToAction("Index");
                }

                if (person.ImageData != null) 
                {
                    if (file.ContentLength > (2 * 1024 * 1024))
                    {
                        ModelState.AddModelError("FileError", "File size must be less than 2 MB.");
                        return View();
                    }

                    if (file.ContentType != "image/jpeg" && file.ContentType != "image/gif")
                    {
                        ModelState.AddModelError("FileError", "File must be jpg or gif.");
                        return View();
                    }

                    person.ImageName = file.FileName;
                    person.ImageSize = file.ContentLength;

                    byte[] data = new byte[file.ContentLength];

                    file.InputStream.Read(data, 0, file.ContentLength);
                    person.ImageData = data;
                    da.UpdatePerson(person);
                }
                    else
                    {
                        if (file.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            var path = Path.Combine(Server.MapPath("~/Content/files"), fileName);
                            file.SaveAs(path);
                        }
                        if (file.ContentType != "image/jpeg" && file.ContentType != "image/gif")
                        {
                            ModelState.AddModelError("FileError", "File must be jpg or gif.");
                            return View();
                        }

                        person.ImageName = file.FileName;
                        person.ImageSize = file.ContentLength;
                        da.UpdatePerson(person);
                    }
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int itemID)
        {
            SQLiteDA da = new SQLiteDA();
            da.DeletePerson(itemID);
            return RedirectToAction("Index");
        }
	}
}