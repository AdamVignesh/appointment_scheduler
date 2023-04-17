using AppointmentScheduler.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Session;

namespace AppointmentScheduler.Controllers
{

    public class employeeController : Controller
    {

        IConfiguration configuration;
        public employeeController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // GET: employeeController
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(IFormCollection collection)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("appointment-scheduler");
                SqlConnection connection = new(connectionString);


                connection.Open();


                string query = $"INSERT INTO employees VALUES({collection.ElementAt(0).Value},'{collection.ElementAt(1).Value}','{collection.ElementAt(2).Value}',{collection.ElementAt(3).Value})";
                SqlCommand command = new SqlCommand(query, connection);

                command.ExecuteNonQuery();


                string query1 = $"INSERT INTO LoginTable VALUES({collection.ElementAt(0).Value},'{collection.ElementAt(4).Value}')";
                SqlCommand command1 = new SqlCommand(query1, connection);
                command1.ExecuteNonQuery();

                /* ViewBag.ResultMessage = "Job Added Successfully";
                 ViewBag.AlertCode = 1;*/

                // return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                Console.WriteLine("in Index's post catch :" + ex.Message);
                ViewBag.ResultMessage = "Job Failed To Add";
                ViewBag.AlertCode = 0;


            }
            // return RedirectToAction("Login","Employee");
            return View("Login", "Employee");
        }
        public int getLoggedInUserIdForProfile()
        {
            int? LoggedUserId = HttpContext.Session.GetInt32("emp_id");
            Console.WriteLine(LoggedUserId);
            return LoggedUserId.GetValueOrDefault();
        }
        public List<appointments> appointmentsList = new List<appointments>();
        public ActionResult appointments()
        {
            //Console.WriteLine(getLoggedInUserIdForProfile());
            try
            {
                string connectionString = configuration.GetConnectionString("appointment-scheduler");
                SqlConnection connection = new(connectionString);


                connection.Open();


                string query = $"SELECT * FROM appointments WHERE my_emp_id = {getLoggedInUserIdForProfile()}";
                SqlCommand command = new SqlCommand(query, connection);

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    appointments appointment = new appointments();
                    appointment.appointment_id = (int)reader["appointment_id"];
                    appointment.my_emp_id = (int)reader["my_emp_id"];
                    DateTime dateTimeValue = (DateTime)reader["appointment_date"];
                    appointment.appointment_date = dateTimeValue.Date;
                    Console.WriteLine(dateTimeValue.Date);
                    DateTime timeValue = (DateTime)reader["appointment_time"];
                    appointment.appointment_time = timeValue.TimeOfDay;
                    appointment.emp_id = (int)reader["emp_id"];
                    appointment.topic = (string)reader["topic"];
                    appointmentsList.Add(appointment);
                }

                //Console.WriteLine(appointment);
            }
            catch (Exception ex)
            {
                Console.WriteLine("in appointments get catch :" + ex.Message);
                //ViewBag.ResultMessage = "Job Failed To Add";
                //ViewBag.AlertCode = 0;


            }
            ViewBag.appointmentsList = appointmentsList;
            // return RedirectToAction("Login","Employee");
            return View();
        }

        // GET: employeeController
        public ActionResult AddAppointment()
        {
            //Console.WriteLine("In add appointment");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAppointment(IFormCollection collection)
        {

            //Console.WriteLine(getLoggedInUserIdForProfile());
            try
            {
                string connectionString = configuration.GetConnectionString("appointment-scheduler");

                SqlConnection connection = new(connectionString);

                connection.Open();
                string query = $"Insert into appointments values ({getLoggedInUserIdForProfile()},{collection.ElementAt(0).Value},'{collection.ElementAt(3).Value}', '{collection.ElementAt(1).Value}','{collection.ElementAt(2).Value}')";
                SqlCommand command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Console.WriteLine("in add appointments post catch :" + ex.Message);
            }
            return RedirectToAction("appointments");

        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(IFormCollection collection)
        {
            Console.WriteLine("In login post");
            try
            {
                string connectionString = configuration.GetConnectionString("appointment-scheduler");
                SqlConnection connection = new(connectionString);


                connection.Open();


                string query = $"SELECT * FROM LoginTable WHERE emp_id = {collection.ElementAt(0).Value}";
                SqlCommand command = new SqlCommand(query, connection);

                var reader = command.ExecuteReader();
                login log = new login();
                while (reader.Read())
                {
                    log.emp_id = (int)reader["emp_id"];
                    log.pass = (string)reader["pass"];

                }

                if (log.pass == collection.ElementAt(1).Value)
                {
                    //Console.WriteLine("EMP_ID " + log.emp_id);
                    HttpContext.Session.SetInt32("emp_id", log.emp_id);

                    return RedirectToAction("appointments", "Employee");

                }
                else
                {
                    Console.WriteLine("Fail");
                    return View("Login", "Employee");
                }
                return View("appointments", "Employee");



            }

            catch (Exception ex)
            {
                Console.WriteLine("in Login's Post catch :" + ex.Message);
                Console.WriteLine("Duplicating Primary Key");
                ViewBag.ResultMessage = "Job Failed To Add";
                ViewBag.AlertCode = 0;


            }

            return View();
        }

        // GET: employeeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: employeeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: employeeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: employeeController/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("appointment-scheduler");
                SqlConnection connection = new(connectionString);


                connection.Open();


                string query = $"SELECT * FROM appointments WHERE appointment_id = {id}";

                SqlCommand command = new SqlCommand(query, connection);

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    appointments appointment = new appointments();
                    //appointment.appointment_id = (int)reader["appointment_id"];
                    appointment.my_emp_id = (int)reader["my_emp_id"];
                    DateTime dateTimeValue = (DateTime)reader["appointment_date"];
                    appointment.appointment_date = dateTimeValue.Date;
                    Console.WriteLine(dateTimeValue.Date);
                    DateTime timeValue = (DateTime)reader["appointment_time"];
                    appointment.appointment_time = timeValue.TimeOfDay;
                    appointment.emp_id = (int)reader["emp_id"];
                    appointment.topic = (string)reader["topic"];
                    appointmentsList.Add(appointment);
                }
                ViewBag.appointmentsList = appointmentsList;
            }
            catch (Exception ex)
            {
                Console.WriteLine("in appointments get catch :" + ex.Message);


            }
            ViewBag.appointmentsList = appointmentsList;

            return View();

        }

        // POST: employeeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                Console.WriteLine("In edit's post");
                string connectionString = configuration.GetConnectionString("appointment-scheduler");

                SqlConnection connection = new(connectionString);

                connection.Open();
                string query = $"UPDATE appointments SET emp_id = {collection.ElementAt(0).Value},appointment_date = '{collection.ElementAt(1).Value}', appointment_time = '{collection.ElementAt(2).Value}',topic = '{collection.ElementAt(3).Value}' WHERE appointment_id = {id}";
                SqlCommand command = new SqlCommand(query, connection);
                Console.WriteLine(query);
                command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Console.WriteLine("in Edit's post catch :" + ex.Message);
            }
            return RedirectToAction("appointments");
        }

        // GET: employeeController/Delete/5
        public ActionResult Delete(int id)
        {

            try
            {
                Console.WriteLine("In delete's get");
                string connectionString = configuration.GetConnectionString("appointment-scheduler");

                SqlConnection connection = new(connectionString);

                connection.Open();
                string query = $"DELETE FROM appointments WHERE appointment_id = {id}";
                SqlCommand command = new SqlCommand(query, connection);
                Console.WriteLine(query);
                command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Console.WriteLine("in Edit's post catch :" + ex.Message);
            }
            return RedirectToAction("appointments");
        }

        // POST: employeeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {

            return View();
        }
    }
}