using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Crudapp.database_access_layer
{
    public class db
    {
        
        SqlConnection con = new SqlConnection("Server=DESKTOP-260O2KU;Database=Crudapp;Trusted_Connection=True;MultipleActiveResultSets=True;");

        //Get Department List
        public DataSet GetDepartment()
        {
            SqlCommand com = new SqlCommand("DepartmentList", con);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;


        }
        public DataSet GetCourse(int id)
        {
            SqlCommand com = new SqlCommand("CourseList", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@DepId", id);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        public DataSet GetDepId()
        {
            SqlCommand com = new SqlCommand("DepIdList", con);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;


        }

        public DataSet GetCourseId()
        {
            SqlCommand com = new SqlCommand("CIdList", con);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;


        }
    }
}
