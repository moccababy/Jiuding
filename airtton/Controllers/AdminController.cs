﻿using airtton.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using airtton.ViewModel;

namespace airtton.Controllers
{
    public class AdminController : Controller
    {
        private NewsDBContext db = new NewsDBContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        #region // News

        public ActionResult News()
        {
            var news = db.News.ToList();

            List<NewsSummaryViewModel> news_sm = new List<NewsSummaryViewModel>();

            foreach (var item in news)
            {
                NewsSummaryViewModel _news = new NewsSummaryViewModel
                {
                    Title = item.Title,
                    Content = item.Content,
                    ImagePath = item.ImageUrl,
                    CreateDate = item.CreateDate.ToString(),
                    Id = item.ID
                };

                news_sm.Add(_news);
            }

            return View(news_sm);
        }
               
        public ActionResult NewsEdit(int id)
        {
            var news = db.News.SingleOrDefault(r => r.ID == id);

            var image = new UploadImageModel()
            {
                InstanceId = news.ID,
                ParentId = news.ID,
                IsPrimary = false,
                CurrentImage = null,
                Type = "news"
            };

            NewsEditViewModel _news = new NewsEditViewModel
            {
                Image = image,
                Title = news.Title,
                Content = news.Content,
                Id = news.ID,
                ImagePath = news.ImageUrl
            };

            return View(_news);

        }
               
        public ActionResult NewsSubmit(NewsEditViewModel submit_news)
        {
            var news = db.News.SingleOrDefault(r => r.ID == submit_news.Id);

            news.Title = submit_news.Title;
            news.Content = submit_news.Content;

            db.SaveChanges();

            ImageUpload _upload = new ImageUpload(Server);
            _upload.UploadImage(submit_news.Image);

            return RedirectToAction("News");
        }
                
        public ActionResult NewsDeleteConfirm(int id)
        {
            var news = db.News.Find(id);

            NewsEditViewModel _news = new NewsEditViewModel
            {
                Title = news.Title,
                Content = news.Content,
                Id = news.ID,
                ImagePath = news.ImageUrl
            };
            return View(_news);
        }
                
        public ActionResult NewsDelete(NewsEditViewModel delete_news)
        {
            var news = db.News.Find(delete_news.Id);

            db.News.Remove(news);
            db.SaveChanges();

            return RedirectToAction("News");
        }
                
        public ActionResult NewsCreate()
        {            
           return View();
        }
                
        public ActionResult CreateSubmit(NewsEditViewModel create_news)
        {
             
            News CreateNews = new News
            {
            Title = create_news.Title,
            Content = create_news.Content,
            CreateDate = DateTime.Now,
             };

            db.News.Add(CreateNews);
            db.SaveChanges();

            

            ImageUpload _upload = new ImageUpload(Server);

            create_news.Image.Type = "news";
            create_news.Image.InstanceId = CreateNews.ID;
            create_news.Image.ParentId = CreateNews.ID;

            _upload.UploadImage(create_news.Image);

            return RedirectToAction("News");
        }
        #endregion

        #region// Events

        public ActionResult Events()
        {
            var events = db.Events.ToList();

            List<EventsSummaryViewModel> event_sm = new List<EventsSummaryViewModel>();

            foreach (var item in events)
            {
                EventsSummaryViewModel _event = new EventsSummaryViewModel
                {
                    Id = item.ID,
                    Title = item.Title,
                    Content = item.Content,
                    CreateDate = item.CreateDate.ToString(),
                    ImagePath = item.ImageUrl
                };
                event_sm.Add(_event);
            }
            return View(event_sm);
        }
                
        public ActionResult EventsEdit(int id)
        {
            var events = db.Events.Find(id);

            var image = new UploadImageModel()
            {
                InstanceId = events.ID,
                ParentId = events.ID,
                IsPrimary = false,
                CurrentImage = null,
                Type = "events"
            };

            EventsEditViewModel _events = new EventsEditViewModel
            {
                Image = image,
                Title = events.Title,
                Content = events.Content,
                Id = events.ID,
                ImagePath = events.ImageUrl
            };

            return View(_events);
        }
                       
        public ActionResult EventsSubmit(EventsEditViewModel submit_events)
        {
            var events = db.Events.Find(submit_events.Id);
            
            events.Title = submit_events.Title;
            events.Content = submit_events.Content;

            db.SaveChanges();

            ImageUpload _upload = new ImageUpload(Server);
            _upload.UploadImage(submit_events.Image);

            return RedirectToAction("Events");
        }
                
        public ActionResult EventsDeleteConfirm(int id)
        {
            var events = db.Events.Find(id);

            EventsEditViewModel _events = new EventsEditViewModel
            {
                Id = events.ID,
                Title = events.Title,
                Content = events.Content,
                ImagePath = events.ImageUrl
            };
            return View(_events);
        }
              
        public ActionResult EventsDelete(EventsEditViewModel delete_events)
        {
            var events = db.Events.Find(delete_events.Id);
            
            db.Events.Remove(events);
            db.SaveChanges();

            return RedirectToAction("Events");
        }
               
        public ActionResult EventsCreate()
        {
            return View();
        }
              
        public ActionResult EventsCreateSubmit(EventsEditViewModel create_events)
        {
            Events events = new Events 
            {
                Title = create_events.Title,
                Content = create_events.Content,
                CreateDate = DateTime.Now
            };

            db.Events.Add(events);
            db.SaveChanges();

            ImageUpload _upload = new ImageUpload(Server);

            create_events.Image.Type = "events";
            create_events.Image.InstanceId = events.ID;
            create_events.Image.ParentId = events.ID;

            _upload.UploadImage(create_events.Image);

            return RedirectToAction("Events");
        }
        #endregion

        #region// GroupIntro        
        public ActionResult GroupIntro()
        {
            var GroupIntros = db.GroupIntro.First();

            GroupIntroSummaryViewModel _GroupIntro = new GroupIntroSummaryViewModel()
            {
                ID = GroupIntros.ID,
                Title = GroupIntros.Title,
                Description = GroupIntros.Description,
                ImagePath = GroupIntros.ImageUrl
            };

            return View(_GroupIntro);
        }

        public ActionResult EditGroupIntro(string title, string content, int id, int echo)
        {
            try
            {
                GroupIntroSummaryViewModel _GroupIntro = new GroupIntroSummaryViewModel()
                {
                    ID = id,
                    Title = title,
                    Description = content,
                    ImagePath = null
                };

                //prepare the partial view by passing the model, turn the view into a string in order to render the html at the front end in js
                var view_str = RazorViewToString.RenderRazorViewToString(this, "Partial/_GroupIntroEdit", _GroupIntro);

                //return json result
                return new JsonResult()
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new { result = "success", echo = echo, html = view_str }
                    
                };
            }

            catch (Exception ex)
            {
                return new JsonResult()
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new { result = "fail", message = ex.Message }
                };
            }
        }
        
        public ActionResult EditGroupIntroSubmit(string title, string content, int id, int echo)
        {

           var groupIntros = db.GroupIntro.Find(id);
            
                      
           groupIntros.Title = title;
           groupIntros.Description = content;
           //groupIntros.ID = id;                  

            
           db.SaveChanges();

           GroupIntroSummaryViewModel _GroupIntro = new GroupIntroSummaryViewModel()
           {
               ID = groupIntros.ID,
               Title = groupIntros.Title,
               Description = groupIntros.Description,
               ImagePath = null
           };

           var view_str = RazorViewToString.RenderRazorViewToString(this, "Partial/_GroupIntro", _GroupIntro);

           try
           {
               return new JsonResult()
               {
                   JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                   Data = new { result = "success", echo = echo, html = view_str }

               };
           }
           catch (Exception ex)
           {
               return new JsonResult()
               {
                   JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                   Data = new { result = "fail", message = ex.Message }
               };
           }

           //return RedirectToAction("GroupIntro");
        }
        #endregion

        #region// PresidentDetail
        public ActionResult PresidentDetail()
        {
            var PresidentDetails = db.PresidentDetail.First();

            PresidentDetailSummaryViewModel _PresidentDetail = new PresidentDetailSummaryViewModel()
            {
                ID = PresidentDetails.ID,
                Name = PresidentDetails.Name,
                Position = PresidentDetails.Position,
                Description = PresidentDetails.Description,
                Facebook = PresidentDetails.Facebook,
                Twitter = PresidentDetails.Twitter,
                Google = PresidentDetails.Google,
                LinkedIn = PresidentDetails.LinkedIn,
                Email = PresidentDetails.Email,
                Skype = PresidentDetails.Skype,
                ImagePath = PresidentDetails.ImageUrl
            };
            return View(_PresidentDetail);
        }

        public ActionResult PresidentDetailCreate()
        {
            return View();
        }

        public ActionResult PresidentDetailCreateSubmit(PresidentDetailEditViewModel create_PresidentDetail)
        {
            PresidentDetail presidentDetail = new PresidentDetail
            {
                Name = create_PresidentDetail.Name,
                Position = create_PresidentDetail.Position,
                Description = create_PresidentDetail.Description,
                Email = create_PresidentDetail.Email,
                Facebook = create_PresidentDetail.Facebook,
                Google = create_PresidentDetail.Google,
                LinkedIn = create_PresidentDetail.LinkedIn,
                Twitter = create_PresidentDetail.Twitter,
                Skype = create_PresidentDetail.Skype
            };

            db.PresidentDetail.Add(presidentDetail);
            db.SaveChanges();

            //ImageUpload _upload = new ImageUpload(Server);

            //create_PresidentDetail.Image.Type = "PresidentDetail";
            //create_PresidentDetail.Image.InstanceId = presidentDetail.ID;
            //create_PresidentDetail.Image.ParentId = presidentDetail.ID;

            //_upload.UploadImage(create_PresidentDetail.Image);

            return RedirectToAction("PresidentDetail");
        }

        public ActionResult PresidentDetailEdit(int id)
        {
            var PresidentDetails = db.PresidentDetail.Find(id);

            //var image = new UploadImageModel()
            //{
            //    InstanceId = PresidentDetails.ID,
            //    ParentId = PresidentDetails.ID,
            //    IsPrimary = false,
            //    CurrentImage = null,
            //    Type = "presidentDetail"
            //};

            PresidentDetailEditViewModel _presidentDetail = new PresidentDetailEditViewModel
            {
                ID = PresidentDetails.ID,
                Name = PresidentDetails.Name,
                Position = PresidentDetails.Position,
                Description = PresidentDetails.Description,
                //Image = image,
                Facebook = PresidentDetails.Facebook,
                Twitter = PresidentDetails.Twitter,
                Google = PresidentDetails.Google,
                LinkedIn = PresidentDetails.LinkedIn,
                Email = PresidentDetails.Email,
                Skype = PresidentDetails.Skype,
                ImagePath = PresidentDetails.ImageUrl
            };
            return View(_presidentDetail);
        }

        public ActionResult PresidentDetailSubmit(PresidentDetailEditViewModel submit_PresidentDetail)
        {
            var presidentDetail = db.PresidentDetail.Find(submit_PresidentDetail.ID);

            presidentDetail.Name = submit_PresidentDetail.Name;
            presidentDetail.Position = submit_PresidentDetail.Position;
            presidentDetail.Description = submit_PresidentDetail.Description;
            presidentDetail.Facebook = submit_PresidentDetail.Facebook;
            presidentDetail.Twitter = submit_PresidentDetail.Twitter;
            presidentDetail.Google = submit_PresidentDetail.Google;
            presidentDetail.LinkedIn = submit_PresidentDetail.LinkedIn;
            presidentDetail.Email = submit_PresidentDetail.Email;
            presidentDetail.Skype = submit_PresidentDetail.Skype;
            //presidentDetail.ImageUrl = submit_PresidentDetail.ImagePath;

            db.SaveChanges();

            return RedirectToAction("PresidentDetail");
        }

        //public ActionResult PresidentDetailDeleteConfirm(int id)
        //{
        //    PresidentDetail presidentDetail = db.PresidentDetail.Find(id);

        //    PresidentDetailEditViewModel _presidentDetail = new PresidentDetailEditViewModel
        //    {
        //        Name = presidentDetail.Name,
        //        Position = presidentDetail.Position,
        //        Description = presidentDetail.Description,
        //        //Image = image,
        //        Facebook = presidentDetail.Facebook,
        //        Twitter = presidentDetail.Twitter,
        //        Google = presidentDetail.Google,
        //        LinkedIn = presidentDetail.LinkedIn,
        //        Email = presidentDetail.Email,
        //        Skype = presidentDetail.Skype,
        //        ImagePath = presidentDetail.ImageUrl
        //    };
        //    return View(_presidentDetail);
        //}

        //public ActionResult PresidentDetailDelete(PresidentDetailEditViewModel delete_PresidentDetail)
        //{
        //    var presidentDetail = db.PresidentDetail.Find(delete_PresidentDetail.ID);

        //    db.PresidentDetail.Remove(presidentDetail);
        //    db.SaveChanges();

        //    return RedirectToAction("PresidentDetail");
        //}


        //Honor

        // Organization
        #endregion

        #region// Career
        public ActionResult Career()
        {
            var career = db.Career.ToList();

            List<CareerSummaryViewModel> career_sm = new List<CareerSummaryViewModel>();

            foreach (var item in career)
            {
                CareerSummaryViewModel _career = new CareerSummaryViewModel
                {
                    ID = item.ID,
                    JobTitle = item.JobTitle,
                    //CategoryName = item.CategoryName,
                    Location = item.Location,
                    Experience = item.Experience,
                    Education = item.Education,
                    WorkType = item.WorkType,
                    VacancyNubmer = item.VacancyNubmer,
                    CreateDate = item.CreateDate.ToString(),
                    Description = item.Description,
                    Qualification = item.Qualification
                };
                career_sm.Add(_career);
            }
            return View(career_sm);
        }

        public ActionResult CareerEdit(int id)
        {
            var careers = db.Career.SingleOrDefault(r => r.ID == id);

            CareerEditViewModel _careers = new CareerEditViewModel
            {
                JobTitle = careers.JobTitle,
                //CategoryName = careers.CategoryName,
                Location = careers.Location,
                Experience = careers.Experience,
                Education = careers.Education,
                WorkType = careers.WorkType,
                VacancyNubmer = careers.VacancyNubmer,
                //CreateDate = careers.CreateDate,
                Description = careers.Description,
                Qualification = careers.Qualification,
            };
            return View(_careers);
        }

        public ActionResult CareerSubmit(CareerEditViewModel submit_career)
        {
            var careers = db.Career.SingleOrDefault(r => r.ID == submit_career.ID);

            careers.JobTitle = submit_career.JobTitle;
            //careers.CategoryName = submit_career.CategoryName;
            careers.Location = submit_career.Location;
            careers.Experience = submit_career.Experience;
            careers.Education = submit_career.Education;
            careers.WorkType = submit_career.WorkType;
            careers.VacancyNubmer = submit_career.VacancyNubmer;
            careers.CreateDate = DateTime.Now;
            careers.Description = submit_career.Description;
            careers.Qualification = submit_career.Qualification;

            db.SaveChanges();

            return RedirectToAction("Career");
        }

        public ActionResult CareerCreate()
        {
            return View();
        }

        public ActionResult CareerCreateSubmit(CareerEditViewModel create_career)
        {
            Career careers = new Career
            {
                JobTitle = create_career.JobTitle,
                //CategoryName = create_career.CategoryName,
                Location = create_career.Location,
                Experience = create_career.Experience,
                Education = create_career.Education,
                WorkType = create_career.WorkType,
                VacancyNubmer = create_career.VacancyNubmer,
                CreateDate = DateTime.Now,
                Description = create_career.Description,
                Qualification = create_career.Qualification,
            };

            db.Career.Add(careers);
            db.SaveChanges();

            return RedirectToAction("Career");
        }

        public ActionResult CareerDeleteConfirm(int id)
        {
            var careers = db.Career.SingleOrDefault(r => r.ID == id);

            CareerEditViewModel _career = new CareerEditViewModel
            {
                JobTitle = careers.JobTitle,
                //CategoryName = careers.CategoryName,
                Location = careers.Location,
                Experience = careers.Experience,
                Education = careers.Education,
                WorkType = careers.WorkType,
                VacancyNubmer = careers.VacancyNubmer,
                //CreateDate = careers.CreateDate,
                Description = careers.Description,
                Qualification = careers.Qualification,
            };
            return View(_career);
        }

        public ActionResult CareerDelete(CareerEditViewModel delete_career)
        {
            var careers = db.Career.SingleOrDefault(r => r.ID == delete_career.ID);

            db.Career.Remove(careers);
            db.SaveChanges();

            return RedirectToAction("Career");
        }
        #endregion

        #region// Contact
        public ActionResult Contact()
        {
            var Contacts = db.Contact.First();

            ContactSummaryViewModel _Contact = new ContactSummaryViewModel()
            {
                ID = Contacts.ID,
                Address = Contacts.Address,
                Phone = Contacts.Phone,
                Email = Contacts.Email,
                Fax = Contacts.Fax,
                Link = Contacts.Link
            };

            return View(_Contact);
        }

        public ActionResult EditContact(int id, string address, string phone, string fax, string email, string link, int echo)
        {

            try
            {
                ContactSummaryViewModel _Contact = new ContactSummaryViewModel()
                {
                    ID = id,
                    Address = address,
                    Phone = phone,
                    Fax = fax,
                    Email = email,
                    Link = link
                };

                var view_str = RazorViewToString.RenderRazorViewToString(this, "Partial/_ContactEdit", _Contact);


                return new JsonResult()
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new { result = "success", echo = echo, html = view_str }
                };
            }
            catch (Exception ex)
            {
                return new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new { result = "fail", message = ex.Message }
                };
            }

        }

        public ActionResult EditContactSubmit(int id, string address, string phone, string fax, string email, string link, int echo)
        {
            var Contacts = db.Contact.Find(id);

            Contacts.Address = address;
            Contacts.Phone = phone;
            Contacts.Fax = fax;
            Contacts.Email = email;
            Contacts.Link = link;

            db.SaveChanges();

            ContactSummaryViewModel _Contact = new ContactSummaryViewModel()
            {
                Address = Contacts.Address,
                Phone = Contacts.Phone,
                Fax = Contacts.Fax,
                Email = Contacts.Email,
                Link = Contacts.Link
            };

            var view_str = RazorViewToString.RenderRazorViewToString(this, "Partial/_Contact", _Contact);

            try
            {
                return new JsonResult()
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new { result = "success", echo = echo, html = view_str }
                };
            }
            catch (Exception ex)
            {
                return new JsonResult()
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new { result = "fail", message = ex.Message }
                };
            }

        }
        #endregion

        #region// Message
        public ActionResult MessageInfo()
        {
            var messageInfo = db.MessageInfo.ToList();

            List<MessageInfoSummaryViewModel> message_sm = new List<MessageInfoSummaryViewModel>();

            foreach (var item in messageInfo)
            {
                MessageInfoSummaryViewModel _messageInfo = new MessageInfoSummaryViewModel
                {
                    ID = item.ID,
                    Name = item.Name,
                    Email = item.Email,
                    Content = item.Content,
                    CreateDate = item.CreateDate.ToString()
                };
                message_sm.Add(_messageInfo);
            }
            return View(message_sm);
        }

        public ActionResult MessageDeleteConfirm(int id)
        {
            var MessageInfo = db.MessageInfo.Find(id);

            MessageInfoSummaryViewModel messageInfo = new MessageInfoSummaryViewModel
            {
                Name = MessageInfo.Name,
                Email = MessageInfo.Email,
                Content = MessageInfo.Content,
                CreateDate = MessageInfo.CreateDate.ToString()
            };

            return View(messageInfo);
        }

        public ActionResult MessageDelete(MessageInfoSummaryViewModel messageInfo)
        {
            var MessageInfo = db.MessageInfo.Find(messageInfo.ID);

            db.MessageInfo.Remove(MessageInfo);
            db.SaveChanges();

            return RedirectToAction("MessageInfo");
        }
        #endregion

        #region // Base

        // Base AssemblyPlant 组装车间

        public ActionResult AssemblyPlant()
        {
            var assemblyPlant = db.AssemblyPlant.First();

            BaseAssemblyPlantSummaryViewModel assemblyPlant_sm = new BaseAssemblyPlantSummaryViewModel
            {
                Id = assemblyPlant.ID,
                Title = assemblyPlant.Title,
                Content = assemblyPlant.Content
            };

            return View(assemblyPlant_sm);
        }

        //public ActionResult AssemblyPlantCreate()
        //{
        //    return View();
        //}

        //public ActionResult AssemblyPlantCreateSubmit(BaseAssemblyPlantEditViewModel create_AssemblyPlant)
        //{
        //    AssemblyPlant assemblyPlant = new AssemblyPlant
        //    {
        //        Title = create_AssemblyPlant.Title,
        //        Content = create_AssemblyPlant.Content
        //    };

        //    db.AssemblyPlant.Add(assemblyPlant);
        //    db.SaveChanges();

        //    return RedirectToAction("AssemblyPlant");
        //}

        public ActionResult AssemblyPlantEdit(int id)
        {
            var assemblyPlant = db.AssemblyPlant.Find(id);

            BaseAssemblyPlantEditViewModel _assemblyPlant = new BaseAssemblyPlantEditViewModel
            {
                Title = assemblyPlant.Title,
                Content = assemblyPlant.Content
            };

            return View(_assemblyPlant);
        }

        public ActionResult AssemblyPlantEditSubmit(BaseAssemblyPlantEditViewModel edit_AssemblyPlant)
        {
            var assemblyPlant = db.AssemblyPlant.Find(edit_AssemblyPlant.Id);

            assemblyPlant.Title = edit_AssemblyPlant.Title;
            assemblyPlant.Content = edit_AssemblyPlant.Content;

            db.SaveChanges();

            return RedirectToAction("AssemblyPlant");
        }


        // Base ChemicalProducts 研发大楼

        public ActionResult ChemicalProducts()
        {
            var chemicalProducts = db.ChemicalProducts.First();

            BaseChemicalProductsSummaryViewModel chemicalProducts_sm = new BaseChemicalProductsSummaryViewModel
            {
                Id = chemicalProducts.ID,
                Title = chemicalProducts.Title,
                Content = chemicalProducts.Content
            };

            return View(chemicalProducts_sm);
        }

        public ActionResult ChemicalProductsEdit(int id)
        {
            var chemicalProducts = db.ChemicalProducts.Find(id);

            BaseChemicalProductsEditViewModel _chemicalProducts = new BaseChemicalProductsEditViewModel
            {
                Title = chemicalProducts.Title,
                Content = chemicalProducts.Content
            };

            return View(_chemicalProducts);
        }

        public ActionResult ChemicalProductsEditSubmit(BaseAssemblyPlantEditViewModel edit_ChemicalProducts)
        {
            var chemicalProducts = db.ChemicalProducts.Find(edit_ChemicalProducts.Id);

            chemicalProducts.Title = edit_ChemicalProducts.Title;
            chemicalProducts.Content = edit_ChemicalProducts.Content;

            db.SaveChanges();

            return RedirectToAction("ChemicalProducts");
        }


        // Base MetalProducts 金属制品

        public ActionResult MetalProducts()
        {
            var metalProducts = db.MetalProducts.First();

            BaseMetalProductsSummaryViewModel metalProducts_sm = new BaseMetalProductsSummaryViewModel
            {
                Id = metalProducts.ID,
                Title = metalProducts.Title,
                Content = metalProducts.Content
            };

            return View(metalProducts_sm);
        }

        public ActionResult MetalProductsEdit(int id)
        {
            var metalProducts = db.MetalProducts.Find(id);

            BaseMetalProductsEditViewModel _metalProducts = new BaseMetalProductsEditViewModel
            {
                Title = metalProducts.Title,
                Content = metalProducts.Content
            };

            return View(_metalProducts);
        }

        public ActionResult MetalProductsEditSubmit(BaseMetalProductsEditViewModel edit_MetalProducts)
        {
            var metalProducts = db.MetalProducts.Find(edit_MetalProducts.Id);

            metalProducts.Title = edit_MetalProducts.Title;
            metalProducts.Content = edit_MetalProducts.Content;

            db.SaveChanges();

            return RedirectToAction("MetalProducts");
        }



        // Base PrecisionMachinery 精密机械加工

        public ActionResult PrecisionMachinery()
        {
            var precisionMachinery = db.PrecisionMachinery.First();

            BasePrecisionMachinerySummaryViewModel precisionMachinery_sm = new BasePrecisionMachinerySummaryViewModel
            {
                Id = precisionMachinery.ID,
                Title = precisionMachinery.Title,
                Content = precisionMachinery.Content
            };

            return View(precisionMachinery_sm);
        }

        public ActionResult PrecisionMachineryEdit(int id)
        {
            var precisionMachinery = db.PrecisionMachinery.Find(id);

            BasePrecisionMachineryEditViewModel _precisionMachinery = new BasePrecisionMachineryEditViewModel
            {
                Title = precisionMachinery.Title,
                Content = precisionMachinery.Content
            };

            return View(_precisionMachinery);
        }

        public ActionResult PrecisionMachineryEditSubmit(BasePrecisionMachineryEditViewModel edit_PrecisionMachinery)
        {
            var precisionMachinery = db.PrecisionMachinery.Find(edit_PrecisionMachinery.Id);

            precisionMachinery.Title = edit_PrecisionMachinery.Title;
            precisionMachinery.Content = edit_PrecisionMachinery.Content;

            db.SaveChanges();

            return RedirectToAction("PrecisionMachinery");
        }


        // Base PrecisionStamping 冲压车间

        public ActionResult PrecisionStamping()
        {
            var precisionStamping = db.PrecisionStamping.First();

            BasePrecisionStampingSummaryViewModel precisionStamping_sm = new BasePrecisionStampingSummaryViewModel
            {
                Id = precisionStamping.ID,
                Title = precisionStamping.Title,
                Content = precisionStamping.Content
            };

            return View(precisionStamping_sm);
        }

        public ActionResult PrecisionStampingEdit(int id)
        {
            var precisionStamping = db.PrecisionStamping.Find(id);

            BasePrecisionStampingEditViewModel _precisionStamping = new BasePrecisionStampingEditViewModel
            {
                Title = precisionStamping.Title,
                Content = precisionStamping.Content
            };

            return View(_precisionStamping);
        }

        public ActionResult PrecisionStampingEditSubmit(BasePrecisionStampingEditViewModel edit_PrecisionStamping)
        {
            var precisionStamping = db.PrecisionStamping.Find(edit_PrecisionStamping.Id);

            precisionStamping.Title = edit_PrecisionStamping.Title;
            precisionStamping.Content = edit_PrecisionStamping.Content;

            db.SaveChanges();

            return RedirectToAction("PrecisionStamping");
        }

        // Base SheetMetal 钣金加工

        public ActionResult SheetMetal()
        {
            var sheetMetal = db.SheetMetal.First();

            BaseSheetMetalSummaryViewModel sheetMetal_sm = new BaseSheetMetalSummaryViewModel
            {
                Id = sheetMetal.ID,
                Title = sheetMetal.Title,
                Content = sheetMetal.Content
            };

            return View(sheetMetal_sm);
        }

        public ActionResult SheetMetalEdit(int id)
        {
            var sheetMetal = db.SheetMetal.Find(id);

            BaseSheetMetalEditViewModel _sheetMetal = new BaseSheetMetalEditViewModel
            {
                Title = sheetMetal.Title,
                Content = sheetMetal.Content
            };

            return View(_sheetMetal);
        }

        public ActionResult SheetMetalEditSubmit(BaseSheetMetalEditViewModel edit_SheetMetal)
        {
            var sheetMetal = db.SheetMetal.Find(edit_SheetMetal.Id);

            sheetMetal.Title = edit_SheetMetal.Title;
            sheetMetal.Content = edit_SheetMetal.Content;

            db.SaveChanges();

            return RedirectToAction("SheetMetal");
        }

        #endregion
    }
}