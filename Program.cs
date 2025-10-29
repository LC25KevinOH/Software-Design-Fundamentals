namespace Software_Design_Fundamentals
{
    using System;
    using System.IO;

    class Department
    {
        public string Name;
        public int Index;

        public int EmployeeAmount;
        public double OverallHours;
        public double AverageHours;

        public Employee BestEmployee;
        public List<Employee> AllEmployees;

        public Department(int departmentIndex)
        {
            this.Index = departmentIndex;
            this.AllEmployees = new List<Employee>();
        }

        public void SetName()
        {
            switch (this.Index)
            {
                case 0:
                    this.Name = "Finance";
                    break;

                case 1:
                    this.Name = "Marketing";
                    break;

                case 2:
                    this.Name = "Human Resources";
                    break;

                case 3:
                    this.Name = "Engineering";
                    break;

                case 4:
                    this.Name = "Management";
                    break;
            }
        }

        public void RecieveEmployee(Employee newEmployee)
        {
            this.EmployeeAmount++;

            this.AllEmployees.Add(newEmployee);

            this.OverallHours = this.OverallHours += newEmployee.HoursWeek;

            if(this.BestEmployee == null)
            {
                this.BestEmployee = newEmployee;
            }
            else
            {
                if(newEmployee.HoursWeek > this.BestEmployee.HoursWeek)
                {
                    this.BestEmployee = newEmployee;
                }
            }
        }

        public void SetAverageHours()
        {
            this.AverageHours = this.OverallHours / this.EmployeeAmount;
        }
    }
    class Employee
    {
        public string Name;
        public string Department;
        public int DepartmentIndex;

        public int HoursMonday;
        public int HoursTuesday;
        public int HoursWednesday;
        public int HoursThursday;
        public int HoursFriday;
        public int HoursWeek;

        public Employee(string employeeName, string department, int hoursMonday, int hoursTuesday, int hoursWednesday, int hoursThursday, int hoursFriday)
        {
            this.Name = employeeName;
            this.Department = department;
            
            this.HoursMonday = hoursMonday;
            this.HoursTuesday = hoursTuesday;
            this.HoursWednesday = hoursWednesday;
            this.HoursThursday = hoursThursday;
            this.HoursFriday = hoursFriday;
        }

        public void SetDepartment()
        {
            switch (this.Department)
            {
                case "Finance":
                    this.DepartmentIndex = 0;
                    break;

                case "Marketing":
                    this.DepartmentIndex = 1;
                    break;

                case "Human Resources":
                    this.DepartmentIndex = 2;
                    break;

                case "Engineering":
                    this.DepartmentIndex = 3;
                    break;

                case "Management":
                    this.DepartmentIndex = 4;
                    break;
            }
        }

        public void SetHoursWeek()
        {
            this.HoursWeek = this.HoursMonday + this.HoursTuesday + this.HoursWednesday + this.HoursThursday + this.HoursFriday;
        }
    }
    internal class Program
    {
        public static string inputFilePath = "C:/Users/25KevinOHalloran/source/repos/Software-Design-Fundamentals/SD-TA-001-A_OrganisationWeeklyTimesheet.csv";
        public static string outputFilePath = "C:/Users/25KevinOHalloran/source/repos/Software-Design-Fundamentals/EmployeeReport.txt";
        
        // Set string array to filepath of 'Input File'
        public static string[] csvLines = File.ReadAllLines(inputFilePath);
        public static int csvLength = csvLines.Length;

        public static int departmentAmount = 5;
        public Department[] theCompany = new Department[departmentAmount];


        // Department Creation
        public void CreateDepartments()
        {
            for (int i = 0; i < departmentAmount; i++)
            {
                theCompany[i] = new Department(i);
                theCompany[i].SetName();
            }
        }

        public void ReadFile()
        {
            for (int i = 1; i < csvLength; i++)
            {
                string[] csvParts = csvLines[i].Split(',');

                Employee employeeTemporary = new Employee(csvParts[0], csvParts[1], int.Parse(csvParts[2]), int.Parse(csvParts[3]), int.Parse(csvParts[4]), int.Parse(csvParts[5]), int.Parse(csvParts[6]));

                employeeTemporary.SetDepartment();
                employeeTemporary.SetHoursWeek();

                theCompany[employeeTemporary.DepartmentIndex].RecieveEmployee(employeeTemporary);
                theCompany[employeeTemporary.DepartmentIndex].SetAverageHours();
            }
        }

        // Output file function
        public void PrintFile(string outputString, string outputPath)
        {
            File.WriteAllText(outputPath, outputString);

            Console.WriteLine($"Output file is a success! \nDestination of Output: {outputFilePath}");
        }

        // Main program function
        static void Main(string[] args)
        {
            Program reportSoftware = new Program();

            string temporaryOutput = "";

            reportSoftware.CreateDepartments();

            reportSoftware.ReadFile();

            temporaryOutput = temporaryOutput + $"---\n";

            foreach(Department department in reportSoftware.theCompany)
            {
                temporaryOutput = temporaryOutput + ($"{department.Name}\n");

                temporaryOutput = temporaryOutput + $"Total Hours Worked: {department.OverallHours}\n";

                temporaryOutput = temporaryOutput + $"Amount of Employees: {department.EmployeeAmount}\n";

                temporaryOutput = temporaryOutput + $"Average Hours Worked: {department.AverageHours}\n";

                temporaryOutput = temporaryOutput + $"Employee with Most Hours: {department.BestEmployee.Name} (Hours Worked: {department.BestEmployee.HoursWeek})\n";

                temporaryOutput = temporaryOutput + $"---\n";
            }

            reportSoftware.PrintFile(temporaryOutput, outputFilePath);
        }
    }
}
