using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace dotnetapiapp.Models;

public class CourseEnquiryDbContext : DbContext
{

    public CourseEnquiryDbContext(DbContextOptions<CourseEnquiryDbContext> options)
        : base(options)
    {
    }
    public virtual DbSet<Course> Courses { get; set; }
    public virtual DbSet<Enquiry> Enquires { get; set; }
}
