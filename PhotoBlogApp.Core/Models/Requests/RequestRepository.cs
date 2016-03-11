using System;
using System.Data.Entity;
using System.Linq;

namespace PhotoBlogApp.Core.Models.Requests
{
    public class RequestRepository : IRequestRepository
    {
        public void Add(Request request)
        {
            using (var db = new DbModelContainer())
            {
                var exist = db.Requests.FirstOrDefault(r =>r.FirstName == request.FirstName) != null;
                if (!exist)
                {
                    try
                    {
                        db.Requests.Add(request);
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.Message);
                    }
                }
            }
        }

        public void Delete(int id)
        {
            var db = new DbModelContainer();
            var request = db.Requests.Find(id);
            if (request != null)
            {
                db.Entry(request).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public Request Find(int id)
        {
            using (var db = new DbModelContainer())
            {
                return db.Requests.FirstOrDefault(p => p.RequestId == id);
            }
        }

        public IQueryable<Request> GetRequests()
        {
            var db = new DbModelContainer();
            return db.Requests;
        }

        public void Save(Request request)
        {
            using (var db = new DbModelContainer())
            {
                db.Entry(request).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}