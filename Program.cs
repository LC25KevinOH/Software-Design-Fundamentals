namespace Software_Design_Fundamentals
{
    using System;
    using System.IO;

    // Department Class.
    class Department
    {
        // Reference variables for the Department.
        public string Name;
        public int Index;

        // Number variables for the Department.
        public int EmployeeAmount;
        public double OverallHours;
        public double AverageHours;

        // Object variables for the Department.
        public Employee BestEmployee;
        public List<Employee> AllEmployees;

        // Department Constructor. Only an Index is needed. New List for Employee Objects is created.
        public Department(int departmentIndex)
        {
            this.Index = departmentIndex;
            this.AllEmployees = new List<Employee>();
        }

        // Method with a Switch Case to find/set the Departments 'Name' based on it's 'Index'.
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

        // Main method of Department class. Called with each new Employee object created. Does all main Department calculations.
        public void RecieveEmployee(Employee newEmployee)
        {
            // Technically the same outcome if 'AllEmployees.Count' was used.
            this.EmployeeAmount++;

            // Add the new Employee object to this Departments List of Employees.
            this.AllEmployees.Add(newEmployee);

            // Add the new Employees total hours for the week to this Departments total hours for the week.
            this.OverallHours = this.OverallHours += newEmployee.HoursWeek;

            // If this Employee is the first of its Department, there will be no 'BestEmployee' set yet.
            if(this.BestEmployee == null)
            {
                this.BestEmployee = newEmployee;
            }
            else
            {
                // If there is already something set for 'BestEmployee'.
                if(newEmployee.HoursWeek > this.BestEmployee.HoursWeek)
                {
                    this.BestEmployee = newEmployee;
                }
            }
        }

        // Simple method to find the average hours worked for this Department based on the information gathered in 'RecieveEmployee()'.
        public void SetAverageHours()
        {
            this.AverageHours = this.OverallHours / this.EmployeeAmount;
        }
    }

    // Employee Class.
    class Employee
    {
        // Basic infomation variables for the Employee.
        public string Name;
        public string Department;
        public int DepartmentIndex;

        // Variables for the Employees 'Hours' during the week.
        public int HoursMonday;
        public int HoursTuesday;
        public int HoursWednesday;
        public int HoursThursday;
        public int HoursFriday;
        public int HoursWeek;

        // Employee Constructor. The information needed is referencing which columns the CSV input file has.
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

        // Similar to the method in the Department class but flipped. A Switch Case to find/set the Employees Department 'Index' based on it's 'Name'.
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

        // Simple calculation to combine the Employees hours each day into a total for the week.
        public void SetHoursWeek()
        {
            this.HoursWeek = this.HoursMonday + this.HoursTuesday + this.HoursWednesday + this.HoursThursday + this.HoursFriday;
        }
    }

    // Main Program Class
    internal class Program
    {
        // Static strings for both the Input and Output filepaths. 
        public static string inputFilePath = "C:/Users/25KevinOHalloran/source/repos/Software-Design-Fundamentals/SD-TA-001-A_OrganisationWeeklyTimesheet.csv";
        public static string outputFilePath = "C:/Users/25KevinOHalloran/source/repos/Software-Design-Fundamentals/EmployeeReport.txt";
        
        // Set string array to filepath of 'Input File'
        public static string[] csvLines = File.ReadAllLines(inputFilePath);
        public static int csvLength = csvLines.Length;

        // Hard-coded the amount of Departments the Company has. A Department array is created called 'theCompany' which will contain the 5 Department Objects.
        public static int departmentAmount = 5;
        public Department[] theCompany = new Department[departmentAmount];


        // The 5 Department Objects are created and their 'Index' set based on 'i'.
        public void CreateDepartments()
        {
            for (int i = 0; i < departmentAmount; i++)
            {
                theCompany[i] = new Department(i);
                // Once 'SetName()' is called each Department will have both the String and Index reference available.
                theCompany[i].SetName();
            }
        }

        // Main function of the Program.
        public void ReadFile()
        {
            for (int i = 1; i < csvLength; i++)
            {
                // Will split each line of the Input file into parts.
                string[] csvParts = csvLines[i].Split(',');

                // The parts are used to populate each newly created Employee Object.
                Employee employeeTemporary = new Employee(csvParts[0], csvParts[1], int.Parse(csvParts[2]), int.Parse(csvParts[3]), int.Parse(csvParts[4]), int.Parse(csvParts[5]), int.Parse(csvParts[6]));

                // Calling both Employee methods to set remaining variables.
                employeeTemporary.SetDepartment();
                employeeTemporary.SetHoursWeek();

                // Using the 'DepartmentIndex' of the current Employee being created to have that Department call 'RecieveEmployee'.
                theCompany[employeeTemporary.DepartmentIndex].RecieveEmployee(employeeTemporary);
                theCompany[employeeTemporary.DepartmentIndex].SetAverageHours();
            }
        }

        // Simple Function to take a given String and Path, and create an Output File.
        public void PrintFile(string outputString, string outputPath)
        {
            File.WriteAllText(outputPath, outputString);

            Console.WriteLine($"Output file is a success! \nDestination of Output: {outputFilePath}");
        }

        // Main Function.
        static void Main(string[] args)
        {
            Program reportSoftware = new Program();

            // This String will be used to hold all text that will be sent to the Output file.
            string temporaryOutput = "";

            // Create the 5 Departments.
            reportSoftware.CreateDepartments();

            // Create and Populate Employee Objects based on the CSV Input file.
            // Each Employee gets sent to a List with the relevant Department Object.
            // All relevant Department calculations are done with each new Employee.
            reportSoftware.ReadFile();

            // Initial seperation line.
            temporaryOutput = temporaryOutput + $"---\n";

            foreach(Department department in reportSoftware.theCompany)
            {
                // Departments Name.
                temporaryOutput = temporaryOutput + ($"{department.Name}\n");

                // Departments Overall Hours Worked.
                temporaryOutput = temporaryOutput + $"Total Hours Worked: {department.OverallHours}\n";

                // The Amount of Employees in this Department.
                temporaryOutput = temporaryOutput + $"Amount of Employees: {department.EmployeeAmount}\n";

                // The Average Hours worked in this Department by the Amount of Employees it has. 
                temporaryOutput = temporaryOutput + $"Average Hours Worked: {department.AverageHours}\n";

                // Which Employee is the best. The Name and Hours Worked by this Employee is given.
                temporaryOutput = temporaryOutput + $"Employee with Most Hours: {department.BestEmployee.Name} (Hours Worked: {department.BestEmployee.HoursWeek})\n";

                // Additional seperation line after each Departments section.
                temporaryOutput = temporaryOutput + $"---\n";
            }

            // Create a report using the temporary string as the content for the report, and the programs given filepath as the destination.
            reportSoftware.PrintFile(temporaryOutput, outputFilePath);
        }
    }
}
