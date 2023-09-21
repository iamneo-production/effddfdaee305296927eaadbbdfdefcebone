using System;
using System.Collections.Generic;

namespace BookStoreDBFirst.Models;
public class Course
{
    public int CourseID  { get; set; }
    public string CourseName  { get; set; }
    public string Description  { get; set; }
    public string Duration  { get; set; }
    public decimal Cost  { get; set; }

}
