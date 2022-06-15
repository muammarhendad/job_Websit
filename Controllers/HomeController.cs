using Job_Offers_Website.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
	[RequireHttps]
	public class HomeController : Controller
	{
		private ApplicationDbContext db = new ApplicationDbContext();   
		public ActionResult Index()
		{
			return View(db.Categories.ToList());
		}

		public ActionResult Details(int JobId)
		{
			var job = db.Jobs.Find(JobId);
			if(job==null)
			{
				return HttpNotFound();
			}
			Session["JobId"] = JobId;
			return View(job);

		}
		[Authorize]
		public ActionResult Apply()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Apply(string Message, HttpPostedFileBase upload)
		{

			if (ModelState.IsValid)
			{
				string path = Path.Combine(Server.MapPath("~/Uploads"), upload.FileName);
				upload.SaveAs(path);

				var UserId = User.Identity.GetUserId();
				var JobId = (int)Session["JobId"];
				var check = db.ApplyForJobs.Where(a => a.JobId == JobId && a.UserId == UserId).ToList();
				if (check.Count < 1)
				{
					var job = new ApplyForJob();
					job.Cv = upload.FileName;
					job.JobId = JobId;
					job.UserId = UserId;
					job.Message = Message;
					job.ApplyDate = DateTime.Now;
					db.ApplyForJobs.Add(job);
					db.SaveChanges();
					ViewBag.Result = "Success, You Apply For This Job ";

				}
				else
				{
					ViewBag.Result = "Error, You are Aready Apply For This Job!!! ";
					return View();
				}

			}
			return RedirectToAction("GetjobsByUser");
			


		}


	  [Authorize]
	  [HttpGet]
		public ActionResult GetJobsByUser()
		{
			var UserId = User.Identity.GetUserId();
			var jobs = db.ApplyForJobs.Where(a => a.UserId == UserId);
			return View(jobs.ToList());
		}
		[Authorize]
		public ActionResult DetailsOfJob(int id)
		{
			var job = db.ApplyForJobs.Find(id);
			if (job == null)
			{
				return HttpNotFound();
			}
		
			return View(job);

		}

		public ActionResult Edit(int id)
		{
			var job = db.ApplyForJobs.Find(id);
			if (job == null)
			{
				return HttpNotFound();
			}

			return View(job);
		}

		// POST: GetjobByUser/Edit/5
		[HttpPost]
		public ActionResult Edit(ApplyForJob job, HttpPostedFileBase upload)
		{
			if (ModelState.IsValid)
			{
				string oldpath = System.IO.Path.Combine(Server.MapPath("~/Uploads"), job.Cv);
				if(upload!=null)
				{
					System.IO.File.Delete(oldpath);
					string path = Path.Combine(Server.MapPath("~/Uploads"), upload.FileName);
					upload.SaveAs(path);
					job.Cv = upload.FileName;

				}
				job.ApplyDate = DateTime.Now;
				db.Entry(job).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("GetJobsByUser");
			}
			return View(job);

		}

		// GET: GetjobByUser/Delete/5
		public ActionResult Delete(int id)
		{
			var job = db.ApplyForJobs.Find(id);
			if (job == null)
			{
				return HttpNotFound();
			}

			return View(job);
		}

		// POST: GetjobsByUser/Delete/5
		[HttpPost]
		public ActionResult Delete(ApplyForJob job)
		{

			// TODO: Add delete logic here
			var myjob = db.ApplyForJobs.Find(job.Id);
			db.ApplyForJobs.Remove(myjob);
			db.SaveChanges();
			return RedirectToAction("GetjobsByUser");


		}
		[Authorize]
		public ActionResult GetJobsByPublisher()
		{
			var UserID= User.Identity.GetUserId();
			var jobs = from app in db.ApplyForJobs //join ApplyForJobs with Jobs 
					   join job in db.Jobs
					   on app.JobId equals job.Id
					   where job.User.Id== UserID
					   select app;

			var grouped = from j in jobs
						  group j by j.job.JobTitle
						into gr
						  select new JobsViewModel // create this class to grouped all users who applyed this  job
						  {
							  JobTitle=gr.Key,
							  Items=gr
						  };


			return View(grouped.ToList());

		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}
		[Authorize]
		public ActionResult Contact()
		{	

			return View();
		}
		[Authorize]
		[HttpPost]
		public ActionResult Contact(ContactModel contact)
		{
			if (ModelState.IsValid)
			{
				var mail = new MailMessage();
				var LoginInfo = new NetworkCredential("muaamar.hendad1991@gmail.com", "Hendad1991");
				mail.From = new MailAddress(contact.Email);
				mail.To.Add(new MailAddress("Moamerhendad@gmail.com"));
				mail.Subject = contact.Subject;
				mail.IsBodyHtml = true;
				string body = "<strong>Sender Name :</strong>" + contact.Name + "<br>" +
							"<strong>Sender Mail :</strong>" + contact.Email + "<br>" +
							"<strong>Subject Message : </strong>" + contact.Subject + "<br>" +
							"<strong>Massege : </strong>" + contact.Message + "<br>";
				mail.Body = body;


				var smtpClient = new SmtpClient("smtp.gmail.com", 587);
				smtpClient.Credentials = LoginInfo;
				smtpClient.EnableSsl = true;
				smtpClient.Send(mail);
				return RedirectToAction("Index");
			}
			else
				return View();
			
		}
		public ActionResult Search()
		{
			return View();
		}
		[HttpPost]
		public ActionResult Search(string SearchName)
		{
			var result = db.Jobs.Where(a => a.JobTitle.Contains(SearchName)
			  || a.JobContent.Contains(SearchName)
			  || a.Category.CategoryName.Contains(SearchName)
			  || a.Category.CategoryDescription.Contains(SearchName)).ToList();
			return View(result);
		}

	}
}
