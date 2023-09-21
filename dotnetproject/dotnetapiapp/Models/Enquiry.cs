using System;
using System.Collections.Generic;

namespace BookStoreDBFirst.Models;
public class Enquiry
{
    public int EnquiryID  { get; set; }
    public DateTime EnquiryDate  { get; set; }
    public string KidName  { get; set; }
    public string ParentsName  { get; set; }
    public string EmailID  { get; set; }
    public string ContactNumber  { get; set; }
    public string CourseName  { get; set; }

}