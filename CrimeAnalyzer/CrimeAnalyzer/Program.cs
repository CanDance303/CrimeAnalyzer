using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace CrimeAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Crime Analyzer");

            if (args.Length != 2)
            {
                Console.WriteLine("CrimeAnalyzer <crime_csv_file_path> <report_file_path>");
                Environment.Exit(1);
            }
            else
            {
                string DataFile = FileChk(args[0]);
                string reportFile = args[1];

                List<Crime> StatsList = ReadData(DataFile);

                string report = CreateReport(StatsList);

                Console.WriteLine(report);
                CreateNewFile(report, reportFile);
            }

        }

        //Exception Handling

        static List<Crime> ReadData(string filename)
        {
            {

                string line;
                int row = 0;
                List<Crime> StatsList = new List<Crime>();

                try
                {
                    StreamReader sr = new StreamReader(filename);

                    string header = sr.ReadLine();                              
                    int numItemsInRow = header.Split(',').Length;               

                    line = sr.ReadLine();
                    while (line != null)
                    {
                        //tracking rows 

                        row++;                                                  
                        int[] values = StringToInt(line.Split(','), row);
                        Length(values.Length, numItemsInRow, row);

                        StatsList.Add(new Crime(values[0], values[1], values[2], values[3], values[4], values[5], values[6], values[7], values[8], values[9], values[10]));
                        line = sr.ReadLine();
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: " + e.Message);                
                }
                return StatsList;
            }

        }


        static string FileChk(string filename)    
        {
            if (File.Exists(filename)) return filename;

            Console.WriteLine("The file {0} does not exist. Goodbye.", filename);
            Environment.Exit(2);
            return null;
        }


        static int[] StringToInt(string[] strArray, int row)    
        {
            int[] intArray = new int[strArray.Length];                           
            int i = 0;
            foreach (string element in strArray)
            {
                if (!Int32.TryParse(strArray[i], out intArray[i]))             
                {
                    Console.WriteLine("Error occured while reading the following row {0}: ", row);
                    Console.WriteLine("Could not convert to integer.");
                    Environment.Exit(3);
                }
                i++;

            }
            return intArray;
        }


        static void Length(int itemsInList, int rowLength, int row)   
        {
            if (itemsInList != rowLength)
            {
                Console.WriteLine("Error occured while reading the following row {0}: ", row);
                Console.WriteLine("Row contains {0} values. Row should contain {1} values.", rowLength, itemsInList);
                Environment.Exit(4);
            }
        }

        static string CreateReport(List<Crime> StatsList)   
        {
           //LINQ stuff

            var years = from crimeStats in StatsList select crimeStats.year;

            var MotorPerYear = from crimeStats in StatsList select crimeStats.motor;

            var murderPerYear = from crimeStats in StatsList select crimeStats.murder;

            var pop2010 = from crimeStats in StatsList where crimeStats.year == 2010 select crimeStats.population;

            var violent2010 = from crimeStats in StatsList where crimeStats.year == 2010 select crimeStats.violent;

            var murdersU15000 = from crimeStats in StatsList where crimeStats.murder < 15000 select crimeStats.year;

            var yrsRobberyOver500000 = from crimeStats in StatsList where crimeStats.robbery > 500000 select crimeStats.year;

            var numRobberyOver500000 = from crimeStats in StatsList where crimeStats.robbery > 500000 select crimeStats.robbery;

            var theft9904 = from crimeStats in StatsList where crimeStats.year >= 1999 && crimeStats.year <= 2004 select crimeStats.theft;

            var murder9497 = from crimeStats in StatsList where crimeStats.year >= 1994 && crimeStats.year <= 1997 select crimeStats.murder;

            var murder1013 = from crimeStats in StatsList where crimeStats.year >= 2010 && crimeStats.year <= 2013 select crimeStats.murder;


            //Math junk

            int minYear = years.Min();
            int maxYear = years.Max();
            int numYears = years.Count();
            int maxMotor = MotorPerYear.Max();
            int minTheft9904 = theft9904.Min();
            int maxTheft9904 = theft9904.Max();

            int[] yrsOver500000Array = yrsRobberyOver500000.Cast<int>().ToArray();
            int[] numOver500000Array = numRobberyOver500000.Cast<int>().ToArray();

            float avgMurder = murderPerYear.Sum() / numYears;
            float avgMurder9497 = murder9497.Sum() / murder9497.Count();
            float avgMurder1013 = murder1013.Sum() / murder1013.Count();
            float violent2010PerCap = (float)violent2010.Sum() / pop2010.First();



            //Results

            string report = "Range: " + minYear + " - " + maxYear + Environment.NewLine; 

            report += "Total number of years in report: " + numYears + Environment.NewLine;

            report += "Years when murder is under 15,000: ";
            foreach (var item in murdersU15000) report += item + " ";
            report += Environment.NewLine + Environment.NewLine;

            report += "Years when there is more than 500,000 robberies: " + yrsOver500000Array + " = " + numOver500000Array + Environment.NewLine;

            report += "Violent crime per capita (2010): " + violent2010PerCap + Environment.NewLine;

            report += "Average murders per year (1994-2013): " + avgMurder + Environment.NewLine;

            report += "Average murders per year (1994-1997): " + avgMurder9497 + Environment.NewLine;

            report += "Average murders per year (2010-2013): " + avgMurder1013 + Environment.NewLine;

            report += "Minimum thefts per year (1999-2004): " + minTheft9904 + Environment.NewLine;

            report += "Maximum thefts per year (1999-2004): " + maxTheft9904 + Environment.NewLine;

            report += "Year with highest number of motor vehicle thefts: " + maxMotor + Environment.NewLine;


            return report;

        }

        static void CreateNewFile(string content, string name)
        {
            try
           {
                StreamWriter sw = new StreamWriter(name); 
                sw.WriteLine(content);

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);   
            }
            finally
           {
                Console.WriteLine("Report {0} was successfully created.", name);
           }
        }

    }
}