using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Crudapp.Data;
using Crudapp.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace Crudapp.Controllers
{
    public class CourseController : Controller
    {
        private readonly IConfiguration _configuration;
        database_access_layer.db dbop = new database_access_layer.db();

        public CourseController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        // GET: CourseController
        public ActionResult Index()
        {
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("CourseViewAll", sqlConnection);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.Fill(dtbl);
            }
            return View(dtbl);
        }

        public IActionResult AddCourse(int id)
        {

            CourseView courseView = new CourseView();
            DataSet ds = dbop.GetDepartment();
            List<DepartmentView> departmentList = new List<DepartmentView>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DepartmentView dept = new DepartmentView();
                dept.DepId = Convert.ToInt32(ds.Tables[0].Rows[i]["DepId"].ToString());
                dept.DName = ds.Tables[0].Rows[i]["DName"].ToString();

                departmentList.Add(dept);
            }

            ViewBag.departmentList = new SelectList(departmentList, "DepId", "DName");
        
            return View(courseView);

            /*
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list.Add(new SelectListItem { Text = dr["DName"].ToString(), Value = dr["DepId"].ToString() });
            }
            ViewBag.Departmentlist = list;
            return View(courseView);
            */
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddCourse(int id, [Bind("CId,DepId,CCode,CName")] CourseView courseView)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
               
                sqlConnection.Open();
                SqlCommand sqlCmd = new SqlCommand("CourseAdd", sqlConnection);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("CId", courseView.CId);
                sqlCmd.Parameters.AddWithValue("DepId", courseView.DepId);
                sqlCmd.Parameters.AddWithValue("CCode", courseView.CCode);
                sqlCmd.Parameters.AddWithValue("CName", courseView.CName);
                sqlCmd.ExecuteNonQuery();

            }
                return RedirectToAction(nameof(Index));
            }

            return View(courseView);
        }


        // GET: Course/AddOrEdit/
        public ActionResult AddOrEdit(int id)
        {
            CourseView courseView = new CourseView();
            if (id > 0)
                courseView = FetchCourseByID(id);
            return View(courseView);
        }

        // POST: Department/AddOrEdit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOrEdit(int id, [Bind("CId,CCode,CName")] CourseView courseView)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
                {
                    sqlConnection.Open();
                    SqlCommand sqlCmd = new SqlCommand("CourseAddOrEdit", sqlConnection);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("CId", courseView.CId);
                    sqlCmd.Parameters.AddWithValue("CCode", courseView.CCode);
                    sqlCmd.Parameters.AddWithValue("CName", courseView.CName);
                   
                    sqlCmd.ExecuteNonQuery();

                }
                return RedirectToAction(nameof(Index));
            }
            return View(courseView);

        }


        

        // GET: CourseController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CourseController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        [NonAction]
        public CourseView FetchCourseByID(int? id)
        {
            CourseView courseView = new CourseView();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                DataTable dtbl = new DataTable();
                sqlConnection.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("CourseViewById", sqlConnection);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.SelectCommand.Parameters.AddWithValue("CId", id);
                sqlDa.Fill(dtbl);
                if (dtbl.Rows.Count == 1)
                {
                    courseView.CId = Convert.ToInt32(dtbl.Rows[0]["CId"].ToString());
                    courseView.CCode = dtbl.Rows[0]["CCode"].ToString();
                    courseView.CName = dtbl.Rows[0]["CName"].ToString();
                    

                }
                return courseView;
            }
        }
        /*
        [HttpPost]
        [AcceptVerbs("GET", "POST")]
        public JsonResult CheckCCode(string CCode , string initialCCode)
        {

            if (CCode == initialCCode)
            {
                return Json(true);
            }
            else
            {
                var Ccode = CheckCCodeAvailable(CCode, initialCCode);
                return Json(false, Ccode);
            }
        }*/

       /* [HttpPost]
        public JsonResult DoesCCodeExist(string CCode, string Code)
        {
            if (CCode == Code)
            {
                return Json(true);
            }
            else
            {
                return Json(CheckCCodeAvailable(CCode, Code));
                

            }
        }
       */


            [HttpPost]
            [AcceptVerbs("GET", "POST")]
            public bool CheckCCodeAvailable(string CCode)
            {
                /*if (CCode == initialCCode)
                {
                    return true;
                }
                else
                {*/


                DataSet dtbl = new DataSet();
                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
                {

                    sqlConnection.Open();
                    SqlDataAdapter sqlDa = new SqlDataAdapter("CheckCCode", sqlConnection);
                    sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                    sqlDa.SelectCommand.Parameters.AddWithValue("CCode", CCode);
                    sqlDa.Fill(dtbl);

                }
                bool status;
                if (dtbl.Tables[0].Rows.Count == 0)
                {
                    status = true;
                }
                else
                {
                    status = false;
                }
                return status;
            }

        }
        /*
        public JsonResult CheckCCode(string CCode)
        {

        }*/
    }

