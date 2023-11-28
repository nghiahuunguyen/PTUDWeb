using MyClass.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Net;

namespace MyClass.DAO
{

    public class PostDAO
    {
        private MyDBContext db = new MyDBContext();

        //index
        public List<Posts> getList()
        {
            return db.Posts.ToList();
        }
        //index dua vao status=1,2, con status=0 -> thùng rác
        public List<Posts> getList(string status = "ALL")
        {
            List<Posts> list = null;
            switch (status)
            {
                case "Index":
                    {
                        list = db.Posts
                            .Where(m => m.Status != 0)
                            .ToList();
                        break;
                    }
                case "Trash":
                    {
                        list = db.Posts
                            .Where(m => m.Status == 0)
                            .ToList();
                        break;
                    }
                default:
                    {
                        list = db.Posts.ToList();
                        break;

                    }
            }

            return list;
        }


        //details
        public Posts getRow(int? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Posts.Find(id);

            }

        }

        //create
        public int Insert(Posts row)
        {
            db.Posts.Add(row);

            return db.SaveChanges();
        }


        //edit
        public int Update(Posts row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }
        //delete
        public int Delete(Posts row)
        {
            db.Posts.Remove(row);
            return db.SaveChanges();
        }
    }


}