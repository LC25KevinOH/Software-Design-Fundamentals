namespace Software_Design_Fundamentals
{
    using System;
    using System.IO;

    // First pass of program aspect of assignment
    internal class Program
    {
        // Set string array to filepath of 'Input File'
        public static string[] csvLines = File.ReadAllLines("C:/Users/25KevinOHalloran/source/repos/Software-Design-Fundamentals/SD-TA-001-A_OrganisationWeeklyTimesheet.csv");

        // Arrays to store details of each employee
        // csvLen -1 to exclude the header line being counter
        static int csvLen = csvLines.Length;

        // string variables
        string[] empName = new string[csvLen - 1];
        string[] empDepStr = new string[csvLen - 1]; //--> string to int
        int[] empDepInt = new int[csvLen - 1];

        // unused array. remove ?
        int[][] empHours = new int[5][];

        // int array for employee hours, 1 per day of week
        int[] empMon = new int[csvLen - 1];
        int[] empTue = new int[csvLen - 1];
        int[] empWed = new int[csvLen - 1];
        int[] empThu = new int[csvLen - 1];
        int[] empFri = new int[csvLen - 1];

        // array for storing all total hours of week (all day hours combined)
        int[] empTotal = new int[csvLen - 1];

        // Department
        /*
         * showing average hours worked, total hours worked, and employee with most hours.
         * 0. Finance 1. Marketing 2. Human Resources 3. Engineer 4. Management
         */
        // Department arrays. Only 5 unique types of departments. Above number will be used as reference numbers

        double[] depTotal = new double[5]; // All empTotal of the same department will be added together in this array, combined on the same index number
        int[] depAmountEmp = new int[5]; // How many employees there in each department. Will be used with above variable to find average hours of departments
        string[] depEmpMost = new string[5]; // to store a reference of the name of which employee worked the most for each department


        // Simple function to create a long line of "---" to seperate sections in the output console
        public void CreateLine()
        {
            int lineAmount = new int();

            lineAmount = 30;

            for (int i = 0; i < lineAmount; i++)
            {
                Console.Write("--");
            }

            Console.Write("\n");
        }

        // Initial main function to take each line of 'Input File' and break it into smaller parts.

        // CSV file has an amount of lines where we care about 1 less (first header line) than the total
        // We create our holder arrays with a size 1 less than CSV lines amount

        // Two options with below function:
        // i starts as 1, reference to CSV will be correct (skips header), setting emp Arrays will be off as first element won't be set.. index out of bounds..i-1 for employee arrays
        // i starts as 0, reference to CSV will be wrong..set i+1, any emp Array can be set to just i
        public void ReadFile()
        {
            for (int i = 1; i < csvLen; i++)
            {
                string[] csvParts = csvLines[i].Split(',');

                empName[i - 1] = csvParts[0];

                empDepStr[i - 1] = csvParts[1];

                switch (csvParts[1])
                {
                    case "Finance":
                        empDepInt[i - 1] = 0;
                    break;

                    case "Marketing":
                        empDepInt[i - 1] = 1;
                    break;

                    case "Human Resources":
                        empDepInt[i - 1] = 2;
                    break;

                    case "Engineering":
                        empDepInt[i - 1] = 3;
                    break;

                    case "Management":
                        empDepInt[i - 1] = 4;
                    break;
                }

                empMon[i - 1] = int.Parse(csvParts[2]);
                empTue[i - 1] = int.Parse(csvParts[3]);
                empWed[i - 1] = int.Parse(csvParts[4]);
                empThu[i - 1] = int.Parse(csvParts[5]);
                empFri[i - 1] = int.Parse(csvParts[6]);

                empTotal[i - 1] = empMon[i - 1] + empTue[i - 1] + empWed[i - 1] + empThu[i - 1] + empFri[i - 1]; // Total hours of i employee in a week
            }
        }

        // Function to calculate necessary values for department Arrays. Uses what the employee department value is as reference to which element of department arrays to be adjusted
        public void DepartmentSplit()
        {
            // Temporary int array to hold reference for which employee has worked the most per department
            int[] temp = new int[5];

            // Will pass through this loop as many employee names are saved
            // The reference to which department is associated with which index is from an above comment
            for (int i = 0; i < empName.Length; i++)
            {
                depTotal[empDepInt[i]] += empTotal[i];
                depAmountEmp[empDepInt[i]] += 1;

                // Check the current employees total hours worked to see if it is more than their departments current (using temp value) highest amount
                // Update that both temp and depEmpMost to reference current employee
                //Now uses newly improved empDep[i]
                if (empTotal[i] > temp[empDepInt[i]])
                {
                    temp[empDepInt[i]] = empTotal[i];
                    depEmpMost[empDepInt[i]] = empName[i];
                }
            }
        }

        // Output file function
        public void PrintFile(string outString)
        {
            string outputPath = "C:/Users/25KevinOHalloran/source/repos/Software-Design-Fundamentals/EmployeeReport.txt";

            File.WriteAllText(outputPath, outString);

            Console.WriteLine("Output file is a success!");
        }

        // Main program function
        static void Main(string[] args)
        {
            Program p = new Program();

            string tempOut = "";

            p.ReadFile();

            p.DepartmentSplit();

            p.CreateLine();
            tempOut = tempOut + $"---\n";


            // For the amount of departments there are, print the departments name and all relevant information
            for (int i = 0; i < p.depTotal.Length; i++)
            {
                // Simple method to specify a different heading per loop
                if (i == 0)
                {
                    Console.WriteLine("Finance");
                    tempOut = tempOut + $"Finance\n";
                }
                else if (i == 1)
                {
                    Console.WriteLine("Marketing");
                    tempOut = tempOut + $"Marketing\n";
                }
                else if (i == 2)
                {
                    Console.WriteLine("Human Resources");
                    tempOut = tempOut + $"Human Resources\n";
                }
                else if (i == 3)
                {
                    Console.WriteLine("Engineering");
                    tempOut = tempOut + $"Engineering\n";
                }
                else
                {
                    Console.WriteLine("Management");
                    tempOut = tempOut + $"Management\n";
                }

                Console.WriteLine($"Amount of employees: {p.depAmountEmp[i]}");
                tempOut = tempOut + $"Amount of employees: {p.depAmountEmp[i]}\n";

                Console.WriteLine($"Total Hours Worked: {p.depTotal[i]}");
                tempOut = tempOut + $"Total Hours Worked: {p.depTotal[i]}\n";

                // Calculate the average hours worked in the given department
                double temp = new double();
                temp = (p.depTotal[i] / p.depAmountEmp[i]);

                Console.WriteLine($"Average Hours Worked: {temp}");
                tempOut = tempOut + $"Average Hours Worked: {temp}\n";

                Console.WriteLine($"Employee with most hours: {p.depEmpMost[i]}");
                tempOut = tempOut + $"Employee with most hours: {p.depEmpMost[i]}\n";

                p.CreateLine();
                tempOut = tempOut + $"---\n";
            }

            // Additionally all employee records are printed individually to reference if above is correct
            Console.WriteLine("All employee records:");
            tempOut = tempOut + $"All employee records:\n";

            p.CreateLine();
            tempOut = tempOut + $"---\n";

            // All employee records
            for (int i = 0; i < p.empName.Length; i++)
            {
                Console.WriteLine($"Employee: {p.empName[i]} of department {p.empDepStr[i]} has worked {p.empTotal[i]}");
                tempOut = tempOut + $"Employee: {p.empName[i]} of department {p.empDepStr[i]} has worked {p.empTotal[i]}\n";
            }

            p.CreateLine();
            tempOut = tempOut + $"---\n";

            p.PrintFile(tempOut);
        }
    }

    // Features Output file functionality + Provides user feedback operation is complete, Still has Console print lines, No Error messages or Classes
}
