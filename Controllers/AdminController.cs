using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

using System.Web.Mvc;
using TruYum_APP.Models;
namespace TruYum_APP.Controllers

{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            string constr = ConfigurationManager.ConnectionStrings["TruYumconn"].ConnectionString;
            List<MenuItem> menuitems = new List<MenuItem>();

            using (SqlConnection con = new SqlConnection(constr))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType=CommandType.Text;
                cmd.Connection=  con;
                cmd.CommandText=$"select * from MenuItems";
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count>0)
                {

                    foreach (DataRow dr in dt.Rows)
                    {
                        MenuItem menu = new MenuItem()
                        {
                            Id = int.Parse(dr["MenuId"].ToString()),
                            Name = dr["name"].ToString(),
                            Category = dr["category"].ToString(),
                            Price = double.Parse(dr["price"].ToString()),
                            IsActive = bool.Parse(dr["IsActive"].ToString()),
                            FreeDelivery = bool.Parse(dr["FreeDelivery"].ToString()),
                            CreatedOn = DateTime.Parse(dr["CreatedOn"].ToString())
                        };
                        menuitems.Add(menu);
                    }
                }
            }
            return View(menuitems);
        }
        public ActionResult Delete(int id)
        {
            if (id !=0)
            {
                string constr = ConfigurationManager.ConnectionStrings["TruYumconn"].ConnectionString;

                using (SqlConnection con = new SqlConnection(constr))
                {
                    SqlCommand cmd = new SqlCommand();

                    cmd.Connection=  con;
                    cmd.CommandType=CommandType.Text;
                    cmd.CommandText=$"delete from MenuItems where Menuid={id}";
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return RedirectToAction("Index", "Admin");
        }
        public ActionResult Add()
        {
            ViewBag.categories = new List<SelectListItem>()
            {
                new SelectListItem { Text="Dessert", Value="Dessert" },
                new SelectListItem { Text="Main course", Value="Main course" },
                new SelectListItem { Text="Main Course", Value="Main Course" },
                new SelectListItem { Text="Starters", Value="Starters" },
            };
            return View();
        }
        [HttpPost]
        public ActionResult Add(MenuItem menu)
        {
            int isactive = menu.IsActive ? 1 : 0;
            int freedelivery = menu.FreeDelivery ? 1 : 0;

            string constr = ConfigurationManager.ConnectionStrings["truYumconn"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                SqlCommand cmd = new SqlCommand();

                cmd.Connection=  con;
                cmd.CommandType=CommandType.Text;
                cmd.CommandText=$"insert into MenuItems(name,category,price,isactive,freedelivery,createdon)"+$"values('{menu.Name}','{menu.Category}',{menu.Price},{isactive},{freedelivery},'{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}')";
                con.Open();
                if (cmd.ExecuteNonQuery() > 0)
                {
                    return RedirectToAction("Index", "Admin");
                }
                con.Close();
            }

            return View();
        }
        public ActionResult Edit(int id)
        {
            ViewBag.categories = new List<SelectListItem>()
            {
                new SelectListItem { Text="Dessert", Value="Dessert" },
                new SelectListItem { Text="Main course", Value="Main course" },
                new SelectListItem { Text="Beverages", Value="Beverages" },
                new SelectListItem { Text="Starters", Value="Starters" },
            };
            MenuItem menu = new MenuItem();
            if (id>0)
            {


                string constr = ConfigurationManager.ConnectionStrings["truYumconn"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    SqlCommand cmd = new SqlCommand();

                    cmd.Connection=  con;
                    cmd.CommandType=CommandType.Text;
                    cmd.CommandText=$"select * from MenuItems where MenuId={id}";
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count>0)
                    {
                        menu.Id = int.Parse(dt.Rows[0]["MenuId"].ToString());
                        menu.Name = dt.Rows[0]["Name"].ToString();
                        menu.Category = dt.Rows[0]["Category"].ToString();
                        menu.Price = double.Parse(dt.Rows[0]["Price"].ToString());
                        menu.IsActive = bool.Parse(dt.Rows[0]["IsActive"].ToString());
                        menu.FreeDelivery = bool.Parse(dt.Rows[0]["FreeDelivery"].ToString());
                    }

                }
            }
            return View(menu);
        }
        [HttpPost]
        public ActionResult Edit(MenuItem menu)
        {
            int isactive = menu.IsActive ? 1 : 0;
            int freedelivery = menu.FreeDelivery ? 1 : 0;

            string constr = ConfigurationManager.ConnectionStrings["truYumconn"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                SqlCommand cmd = new SqlCommand();

                cmd.Connection=  con;
                cmd.CommandType=CommandType.Text;
                cmd.CommandText=$"update MenuItems set name='{menu.Name}',category='{menu.Category}',price={menu.Price},IsActive={isactive},FreeDelivery={freedelivery},ModifyOn='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where MenuId={menu.Id}";
                con.Open();
                int rowcount = cmd.ExecuteNonQuery();

                con.Close();
                if (rowcount>0)
                {
                    return RedirectToAction("Index", "Admin");
                }
            }

            return View();
        }
    }
}