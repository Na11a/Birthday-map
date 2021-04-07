using System.Data;
using System.Collections.Generic;
using System;
using Npgsql;


namespace collections
{
    class Program
    {

        static void Main(string[] args)
        {   
            List<Employee> emps = getBirthdayMap();
            Dictionary<string,List<Employee>> employ = sortByMonthAndDay(emps);
            Info(employ,15);

            
        }

        struct Employee: IComparable<Employee>
        {
            public string name;
            public DateTime birthday;
            public int CompareTo(Employee other){
                return this.birthday.Day.CompareTo(other.birthday.Day); 
            }
            public Employee(string name, DateTime birthday){
                this.name = name;
                this.birthday = birthday;
            }

        }
        static Dictionary<string,List<Employee>> sortByMonthAndDay(List<Employee> employes){
            string[] monthes = new string[]{"January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"};
            Dictionary<string,List<Employee>> birthdayMap = new Dictionary<string, List<Employee>>();
            birthdayMap.Add("January", new List<Employee>());
            
            for(int i =0;i<12;i++){
                birthdayMap[monthes[i]] = new List<Employee>();
            }

            foreach(Employee emp in employes){
                int month = emp.birthday.Month;
                birthdayMap[monthes[month-1]].Add(emp);

            }

            for(int i =0;i<12;i++){
                List<Employee> sortEmployes = birthdayMap[monthes[i]];
                sortEmployes.Sort();
                }
            return birthdayMap;
        } 

        static void Info(Dictionary<string,List<Employee>> birthdayMap,int countOfMonth){
            string[] monthes = new string[]{"January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"};
            int month = DateTime.Now.Month - 1;
            int year = DateTime.Now.Year;
            int lastMonth = month + countOfMonth;
            int empAge;
            while(month<=lastMonth){
                Console.WriteLine($"{monthes[month]} {year}");
                if(birthdayMap[monthes[month]].Count == 0){
                    Console.WriteLine("No people");
                }
                foreach(Employee emp in birthdayMap[monthes[month]]){
                    if(TestCorrectAge(emp.birthday)){
                        empAge = year - emp.birthday.Year; 
                    }
                    else{
                        empAge = year - emp.birthday.Year - 1;
                    }
                    Console.WriteLine($"({emp.birthday.Day}) - {emp.name} ({empAge} years old)");
                }
                month +=1;
                if(month>11){
                    month = 0;
                    lastMonth -=12;
                    year +=1;
                }
            }
        }
        static bool TestCorrectAge(DateTime birthday){
            DateTime today = DateTime.Now;
            if(today.Month>birthday.Month){
                return true;
            }
            else if(today.Month == birthday.Month){
                if(today.Day>birthday.Day){
                    return true;
                }
                else return false;
            }
            else return false;
        }
        static List<Employee> getBirthdayMap()
        {
            List<Employee> dataList = new List<Employee>(); 
            var connString = "Host=127.0.0.1;Username=max;Password=secret;Database=todolist";

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand("SELECT * FROM birthday", conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read()){
                        dataList.Add(new Employee(reader.GetString(0),reader.GetDateTime(1)));
                    } 
                }
                return dataList;
            }     
        
    }
}


