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

namespace Crudapp.Controllers
{
    public class DepartmentController : Controller
    {

        database_access_layer.db dbop = new database_access_layer.db();

        private readonly IConfiguration _configuration;

        public IActionResult About()
        {
            DataSet ds = dbop.GetDepartment();
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list.Add(new SelectListItem { Text = dr["DName"].ToString(), Value = dr["DepId"].ToString() });
            }
            ViewBag.DepartmentList = list;
            return View();
        }

        public JsonResult GetCourse(int id)
        {
            DataSet ds = dbop.GetCourse(id);
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list.Add(new SelectListItem { Text = dr["CName"].ToString(), Value = dr["CId"].ToString() });
            }
          
            return Json(list);
        }


        public partial class Department
        {

            public string EditDepCodeIssue { get; set; }
        }


        /* [AcceptVerbs("GET", "POST")]
         * public bool CheckDepCodeExist(string DepCode)
          {
              DataSet dtbl = new DataSet();
              using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
              {
                  sqlConnection.Open();
                  SqlDataAdapter sqlDa = new SqlDataAdapter("CheckDepCode", sqlConnection);
                  sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
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


          }*/




        public DepartmentController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        // GET: Department
        public ActionResult Index()
        {
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("DepartmentViewAll", sqlConnection);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.Fill(dtbl);
            }
            return View(dtbl);
        }


        // Get: Department/FindCourses/5
        
        public ActionResult FindCourses(int id )
        {
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlCommand sqlCmd = new SqlCommand("FindCoursesByDepartment", sqlConnection);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("DepId", id);
                SqlDataAdapter sqlDa = new SqlDataAdapter(sqlCmd);

                sqlDa.Fill(dtbl);
            }
            return View(dtbl);

        }

        // Get: Department/FindStudents/5

        public ActionResult FindStudents(int id)
        {
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlCommand sqlCmd = new SqlCommand("FindStudentByCourse", sqlConnection);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("CId", id);
                SqlDataAdapter sqlDa = new SqlDataAdapter(sqlCmd);

                sqlDa.Fill(dtbl);
            }
            return View(dtbl);

        }





        // GET: Department/AddOrEdit/
        public ActionResult AddOrEdit(int id)
        {
            DepartmentView departmentView = new DepartmentView();
            if (id > 0)
                departmentView = FetchDepartmentByID(id);
            return View(departmentView);
        }

        // POST: Department/AddOrEdit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOrEdit(int id, [Bind("DepId,DepCode,DName")] DepartmentView departmentView)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
                {
                    sqlConnection.Open();
                    SqlCommand sqlCmd = new SqlCommand("DepartmentAddOrEdit", sqlConnection);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("DepId", departmentView.DepId);
                    sqlCmd.Parameters.AddWithValue("DepCode", departmentView.DepCode);
                    sqlCmd.Parameters.AddWithValue("DName", departmentView.DName);
                    sqlCmd.ExecuteNonQuery();

                }
                return RedirectToAction(nameof(Index));
            }
            return View(departmentView);

        }


        // GET: /Department/CAdd/
        public ActionResult CAdd(int id)
        {
            CourseView courseView = new CourseView();
            return View(courseView);
        }



        // POST: Department/CAdd/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CAdd(int id, [Bind("CId,DepId,CCode,CName")] CourseView courseView)
        {

            if (ModelState.IsValid)
            {
                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
                {
                    sqlConnection.Open();
                    SqlCommand sqlCmd = new SqlCommand("CAdd", sqlConnection);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("CId", courseView.CId);
                    sqlCmd.Parameters.AddWithValue("DepId", id);
                    sqlCmd.Parameters.AddWithValue("CCode", courseView.CCode);
                    sqlCmd.Parameters.AddWithValue("CName", courseView.CName);
                    sqlCmd.ExecuteNonQuery();

                }
                return RedirectToAction(nameof(Index));
            }


            return View(courseView);

        }


        // GET: /Department/SAdd/
        public ActionResult SAdd(int id)
        {
            StudentView studentView = new StudentView();
            return View(studentView);
        }


        // POST: Department/SAdd/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SAdd(int id, [Bind("StudentId,CId,SName,SRollno,Age")] StudentView studentView)
        {

            if (ModelState.IsValid)
            {
                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
                {
                    sqlConnection.Open();
                    SqlCommand sqlCmd = new SqlCommand("SAdd", sqlConnection);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("StudentId", studentView.StudentId);
                    sqlCmd.Parameters.AddWithValue("CId", id);
                    sqlCmd.Parameters.AddWithValue("SName", studentView.SName);
                    sqlCmd.Parameters.AddWithValue("SRollno", studentView.SRollno);
                    sqlCmd.Parameters.AddWithValue("Age", studentView.Age);
                    sqlCmd.ExecuteNonQuery();

                }
                return RedirectToAction(nameof(Index));
            }


            return View(studentView);

        }






        // GET: /Department/CEdit/
        public ActionResult CEdit(int id)
        {

            CourseView courseView = new CourseView();
            if (id > 0)
                courseView = FetchCourseByID(id);
            return View(courseView);
        }
        //Exclude = CCode ModelState.Remove("CCode"); [IgnoreRequiredValidations]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CEdit(int id, [Bind("CId,CCode,CName")] CourseView courseView)
        {
            
            if (ModelState.IsValid)
            {
                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
                {
                    sqlConnection.Open();
                    SqlCommand sqlCmd = new SqlCommand("CEdit", sqlConnection);
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


        // GET: /Course/SEdit/
        public ActionResult SEdit(int id)
        {

           StudentView studentView = new StudentView();
            if (id > 0)
                studentView = FetchStudentByID(id);
            return View(studentView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SEdit(int id, [Bind("StudentId,SRollno,SName,Age")] StudentView studentView)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
                {
                    sqlConnection.Open();
                    SqlCommand sqlCmd = new SqlCommand("SEdit", sqlConnection);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("StudentId", studentView.StudentId);
                    sqlCmd.Parameters.AddWithValue("SRollno", studentView.SRollno);
                    sqlCmd.Parameters.AddWithValue("SName", studentView.SName);
                    sqlCmd.Parameters.AddWithValue("Age", studentView.Age);
                    sqlCmd.ExecuteNonQuery();

                }
                return RedirectToAction(nameof(Index));
            }
            return View(studentView);

        }




        // GET: /Department/SAddOrEdit/
        public ActionResult SAddOrEdit(int id)
        {

            StudentView studentView = new StudentView();
            if (id > 0)
                studentView = FetchStudentByID(id);
            return View(studentView);
        }


        // POST: Department/SAddOrEdit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SAddOrEdit(int id, [Bind("StudentId,DepId,CId,SRollno,SName,Age")] StudentView studentView)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
                {
                    sqlConnection.Open();
                    SqlCommand sqlCmd = new SqlCommand("StudentAddOrEdit", sqlConnection);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("StudentId", studentView.StudentId);
                    sqlCmd.Parameters.AddWithValue("DepId", studentView.DepId);
                    sqlCmd.Parameters.AddWithValue("CId", studentView.CId);
                    sqlCmd.Parameters.AddWithValue("SRollno", studentView.SRollno);
                    sqlCmd.Parameters.AddWithValue("SName", studentView.SName);
                    sqlCmd.Parameters.AddWithValue("Age", studentView.Age);
                    sqlCmd.ExecuteNonQuery();

                }
                return RedirectToAction(nameof(Index));
            }
            return View(studentView);

        }


        // GET: Department/Delete/5
        public ActionResult Delete(int id)
        {
            DepartmentView departmentView = FetchDepartmentByID(id);
            return View(departmentView);

        }

        // POST: Department/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, IFormCollection collection)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlCommand sqlCmd = new SqlCommand("DepartmentDeleteById", sqlConnection);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("DepId", id);
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Department/CDelete/5
        public ActionResult CDelete(int id)
        {
            CourseView courseView = FetchCourseByID(id);
            return View(courseView);

        }

        // POST: Department/CDelete/5

        [HttpPost, ActionName("CDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult CDeleteConfirmed(int id, IFormCollection collection)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlCommand sqlCmd = new SqlCommand("CourseDeleteById", sqlConnection);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("CId", id);
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction(nameof(Index));
        }


        // GET: Department/SDelete/5
        public ActionResult SDelete(int id)
        {
           StudentView studentView = FetchStudentByID(id);
            return View(studentView);

        }


        // POST: Department/CDelete/5

        [HttpPost, ActionName("SDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult SDeleteConfirmed(int id, IFormCollection collection)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlCommand sqlCmd = new SqlCommand("StudentDeleteById", sqlConnection);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("StudentId", id);
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction(nameof(Index));
        }

        [NonAction]
        public DepartmentView FetchDepartmentByID(int? id)
        {
            DepartmentView departmentView = new DepartmentView();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                DataTable dtbl = new DataTable();
                sqlConnection.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("DepartmentViewByID", sqlConnection);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.SelectCommand.Parameters.AddWithValue("DepId", id);
                sqlDa.Fill(dtbl);
                if (dtbl.Rows.Count == 1)
                {
                    departmentView.DepId = Convert.ToInt32(dtbl.Rows[0]["DepId"].ToString());
                    departmentView.DepCode = dtbl.Rows[0]["DepCode"].ToString();
                    departmentView.DName = dtbl.Rows[0]["DName"].ToString();
                    
                }
                return departmentView;
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
                SqlDataAdapter sqlDa = new SqlDataAdapter("CourseViewByID", sqlConnection);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.SelectCommand.Parameters.AddWithValue("CId", id);
             
                sqlDa.Fill(dtbl);
                if (dtbl.Rows.Count == 1)
                {
                    //sqlDa.SelectCommand.Parameters.AddWithValue("DepId", id);
                    courseView.CId = Convert.ToInt32(dtbl.Rows[0]["CId"].ToString());
                    courseView.CCode = dtbl.Rows[0]["CCode"].ToString();
                    courseView.CName = dtbl.Rows[0]["CName"].ToString();
                    

                }
                return courseView;
            }
        }


        [NonAction]
        public StudentView FetchStudentByID(int? id)
        {
            StudentView studentView = new StudentView();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                DataTable dtbl = new DataTable();
                sqlConnection.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("StudentViewByID", sqlConnection);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.SelectCommand.Parameters.AddWithValue("StudentId", id);

                sqlDa.Fill(dtbl);
                if (dtbl.Rows.Count == 1)
                {
                    //sqlDa.SelectCommand.Parameters.AddWithValue("DepId", id);
                    // studentView.DepId = Convert.ToInt32(dtbl.Rows[0]["DepId"].ToString());
                    //studentView.CId = Convert.ToInt32(dtbl.Rows[0]["CId"].ToString());
                    studentView.StudentId = Convert.ToInt32(dtbl.Rows[0]["StudentId"].ToString());
                    studentView.SRollno = dtbl.Rows[0]["SROllnO"].ToString();
                    studentView.SName = dtbl.Rows[0]["SName"].ToString();
                    studentView.Age = Convert.ToInt32(dtbl.Rows[0]["Age"].ToString());

                }
                return studentView;
            }
        }
        
     
        [HttpPost]
        [AcceptVerbs("GET", "POST")]
        public bool CheckDepCodeAvailable(string EditDepCodeIssue, string DepCode)
        {

            
            DataSet dtbl = new DataSet();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
               
                sqlConnection.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("CheckDepCode", sqlConnection);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.SelectCommand.Parameters.AddWithValue("DepCode", DepCode);
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
   
}


