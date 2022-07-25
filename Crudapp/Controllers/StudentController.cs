using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
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
    public class StudentController : Controller
    {
       
        private readonly IConfiguration _configuration;
        database_access_layer.db dbop = new database_access_layer.db();



        
        public StudentController(IConfiguration configuration )
        {
            this._configuration = configuration;
        }

        // GET: Student
        public IActionResult Index()
        {
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("StudentViewAll", sqlConnection);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.Fill(dtbl);
            }
            return View(dtbl);
        }




    

        // GET: Student/AddStudent/
        public IActionResult AddStudent(int? id)
        {

            StudentView studentView = new StudentView();
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

            return View(studentView);
            /*
            StudentView studentView = new StudentView();

            DataSet ds = dbop.GetDepartment();
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list.Add(new SelectListItem { Text = dr["DName"].ToString(), Value = dr["DepId"].ToString() });
            }
            ViewBag.Departmentlist = list;
            
            return View(studentView);
            */
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
        /*
                public JsonResult GetCourse(int id)
                {
                    DataSet ds = dbop.GetCourse(id);

                    List<CourseView> courseList = new List<CourseView>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        CourseView course = new CourseView();
                        course.CId = Convert.ToInt32(ds.Tables[0].Rows[i]["CId"].ToString());
                        course.CName = ds.Tables[0].Rows[i]["CName"].ToString();


                    }
                    return Json(courseList);

                    List<SelectListItem> list = new List<SelectListItem>();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(new SelectListItem { Text = dr["CName"].ToString(), Value = dr["CId"].ToString() });
                    }



                }

        */
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddStudent(int id, [Bind("StudentId,DepId,CId,SRollno,SName,Age")] StudentView studentView)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
                {
                    sqlConnection.Open();
                    SqlCommand sqlCmd = new SqlCommand("StudentAdd", sqlConnection);
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

        // GET: Student/AddOrEdit/
        public IActionResult AddOrEdit(int? id)
        {
         StudentView studentView = new StudentView();    
         if (id > 0)
            studentView = FetchStudentByID(id);
            return View(studentView);
        }

        

        // POST: Student/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrEdit(int id, [Bind("StudentId,SRollno,SName,Age")] StudentView studentView)
        {
           if (ModelState.IsValid)
            {
                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
                {
                    sqlConnection.Open();
                    SqlCommand sqlCmd = new SqlCommand("StudentAddOrEdit", sqlConnection);
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

        // GET: Student/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            StudentView studentView = FetchStudentByID(id);
            return View(studentView);
            
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
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
                    studentView.StudentId = Convert.ToInt32(dtbl.Rows[0]["StudentId"].ToString());
                    studentView.SRollno = dtbl.Rows[0]["SRollno"].ToString();
                    studentView.SName = dtbl.Rows[0]["SName"].ToString();
                    studentView.Age = Convert.ToInt32(dtbl.Rows[0]["Age"].ToString());
                }
                return studentView;
            }
        }


        [HttpPost]
        [AcceptVerbs("GET", "POST")]
        public bool CheckSRollnoAvailable(string SRollno)
        {
            DataSet dtbl = new DataSet();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {

                sqlConnection.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("CheckSRollno", sqlConnection);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.SelectCommand.Parameters.AddWithValue("SRollno", SRollno);
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
