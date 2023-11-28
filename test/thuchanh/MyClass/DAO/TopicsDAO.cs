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

    public class TopicsDAO
    {
        private MyDBContext db = new MyDBContext();

        //index
        public List<Topics> getList()
        {
            return db.Topics.ToList();
        }
        //index dua vao status=1,2, con status=0 -> thùng rác
        public List<Topics> getList(string status = "ALL")
        {
            List<Topics> list = null;
            switch (status)
            {
                case "Index":
                    {
                        list = db.Topics
                            .Where(m => m.Status != 0)
                            .ToList();
                        break;
                    }
                case "Trash":
                    {
                        list = db.Topics
                            .Where(m => m.Status == 0)
                            .ToList();
                        break;
                    }
                default:
                    {
                        list = db.Topics.ToList();
                        break;

                    }
            }

            return list;
        }


        //details
        public Topics getRow(int? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Topics.Find(id);

            }

        }

        //create
        public int Insert(Topics row)
        {
            db.Topics.Add(row);

            return db.SaveChanges();
        }


        //edit
        public int Update(Topics row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }
        //delete
        public int Delete(Topics row)
        {
            db.Topics.Remove(row);
            return db.SaveChanges();
        }
    }


}