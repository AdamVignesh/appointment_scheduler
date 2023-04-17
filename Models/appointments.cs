namespace AppointmentScheduler.Models
{
    public class appointments
    {
        public int appointment_id { get; set; }
        public int my_emp_id { get; set; }
        public int emp_id { get; set; }
        
        public DateTime appointment_date { get; set; }
        public TimeSpan  appointment_time { get; set; }

        public string topic { get; set; }



    }
}
