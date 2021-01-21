using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Wriststone.Models;
using Wriststone.Models.Table;
using Wriststone.Models.ViewModel;

namespace Wriststone.Controllers
{
    public class ForumController : Controller
    {
        readonly WriststoneContext db = new WriststoneContext();
        public ActionResult Forum()
        {
            return View(db.ForumCategories.Where(e => e.Category == null).ToList());
        }

        [HttpGet]
        public ActionResult Category(long? id)
        {
            if (id == null) return View("NotFound");
            CategoryThreadsModel categoryThreads = new CategoryThreadsModel
            {
                Category = db.ForumCategories.Where(e => e.Id == id).Single(),
                Categories = db.ForumCategories.Where(e => e.Category == id).ToList().AsEnumerable(),
                Threads = db.Threads.Where(e => e.Category == id).ToList().AsEnumerable().OrderBy(e => e.Created)
            };
            return View(categoryThreads);
        }

        [HttpGet]
        public ActionResult Create(long? id)
        {
            if (id == null) return View("NotFound");
            return View(db.ForumCategories.Where(e => e.Id == id).Single());
        }

        [HttpPost]
        public ActionResult Thread(long category, string subject, string context)
        {
            var account = Convert.ToInt64(Session["id"]);
            db.Threads.Add(new Thread { Subject = subject, Category = category, Created = DateTime.Now, Account = account});
            db.SaveChanges();
            var thread = db.Threads.Where(e => e.Subject.Equals(subject)).Single().Id;
            db.Posts.Add(new Post { Context = context, Account = account, Created = DateTime.Now, Thread = thread });
            db.SaveChanges();
            return RedirectToAction("Thread", new { id = thread });
        }

        [HttpGet]
        public ActionResult Thread(long? id)
        {
            if (id == null) return View("NotFound");
            ThreadPostsModel posts = new ThreadPostsModel
            {
                Thread = db.Threads.Where(e => e.Id == id).Single()
            };
            posts.Creator = db.Accounts.Where(e => e.Id == posts.Thread.Account).Single();
            posts.Post = db.Posts.Where(e => e.Account == posts.Creator.Id && e.Thread == posts.Thread.Id).Min(e => e.Id);
            var result = from post in db.Posts
                         join account in db.Accounts on post.Account equals account.Id
                         orderby post.Id
                         where post.Thread == id
                         select new AccountPostModel { Account = account, Post = post};
            posts.Posts = result.ToList().AsEnumerable();
            return View(posts);
        }

        [HttpPost]
        public ActionResult Post(long thread, string context)
        {
            var account = Convert.ToInt64(Session["id"]);
            db.Posts.Add(new Post { Context = context, Account = account, Created = DateTime.Now, Thread = thread });
            db.SaveChanges();
            return RedirectToAction("Thread", new { id = thread });
        }

        [HttpGet]
        public ActionResult Edit(long thread, long post)
        {
            CreatorThreadModel creatorThread = new CreatorThreadModel
            {
                Thread = db.Threads.Where(e => e.Id == thread).Single(),
                Post = db.Posts.Where(e => e.Id == post).Single()
            };
            return View(creatorThread);
        }

        [HttpPost]
        public ActionResult Edit(long thread, long post, string subject, string context)
        {
            db.Threads.Where(e => e.Id == thread).Single().Subject = subject;
            db.Posts.Where(e => e.Id == post).Single().Context = context;
            db.SaveChanges();
            return RedirectToAction("Thread", new { id = thread });
        }

    }
}
