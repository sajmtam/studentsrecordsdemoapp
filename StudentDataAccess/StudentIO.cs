﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace StudentDataAccess
{
    public static class StudentIO
    {
        static string filePath = @$"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\test.txt";
        static List<Student> students = new List<Student>();

        #region Load all Students
        public static List<Student> LoadStudents()
        {
            students.Clear();
            try
            {
                using (StreamReader streamReader = File.OpenText(filePath))
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        if (line.Trim().Length > 0)
                            students.Add(new Student(line));
                    }
                }
            }
            catch (IOException ex)
            {
                Trace.WriteLine(ex.Message);
                try
                {
                    using (StreamWriter streamWriter = File.AppendText(filePath))
                    {

                        streamWriter.Write(string.Empty);
                    }

                }
                catch (UnauthorizedAccessException unAuthException)
                {
                    Trace.WriteLine(unAuthException.Message);
                }
            }
            return students;
        }
        #endregion

        #region Save a Student
        public static void SaveStudent(Student student)
        {
            try
            {
                using (StreamWriter streamWriter = File.AppendText(filePath))
                {
                    streamWriter.WriteLine(student.ToJson());
                }

            }
            catch(UnauthorizedAccessException unAuthorizedAccessException)
            {
                Trace.WriteLine(unAuthorizedAccessException.Message);
            }
        }
        #endregion

        #region Remove a Student by Index
        public static void RemoveStudent(int index)
        {
            LoadStudents();
            students.RemoveAt(index);
            File.WriteAllText(filePath, string.Empty);
            foreach (Student student in students)
            {
                SaveStudent(student);
            }
        }
        #endregion

        #region Update the Student by Index
        public static void UpdateStudent(int selectedIndex, Student in_student)
        {
            LoadStudents();
            students[selectedIndex] = in_student;

            File.WriteAllText(filePath, string.Empty);
            foreach (Student student in students)
            {
                SaveStudent(student);
            }
        }
        #endregion

        #region Export Students as CSV file
        public static bool ExportStudents(string path)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"Id, Name, Class, Section, Contact, Address");

            LoadStudents();
            if (students.Count == 0)
            {
                return false;
            }
            foreach (Student student in students)
            {
                builder.AppendLine(student.ToString());
            }
            try
            {
                File.WriteAllText(path, string.Empty);
                using (TextWriter tw = new StreamWriter(path, true))
                {
                    tw.Write(builder);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Helper functions
        public static int NextId()
        {
            LoadStudents();
            Trace.WriteLine($"Number of students: {students.Count}");
            if (students.Count == 0)
            {
                return 0;
            }
            return students[^1].Id + 1;
        }
        #endregion
    }
}
