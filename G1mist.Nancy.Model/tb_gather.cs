// <auto-generated />
// This file was generated by a T4 template.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the .tt file (i.e. the T4 template) and save it to regenerate this file.

// Make sure the compiler doesn't complain about missing Xml comments
#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ServiceStack.OrmLite;
using ServiceStack.DataAnnotations;
using ServiceStack.Model;

namespace G1mist.Nancy.Model
{


	[Alias("tb_gather")]


    public partial class tb_gather 
    {

        [AutoIncrement]

		[PrimaryKey]
        public int id { get; set;}


        [Required]

        public string time { get; set;}


        [Required]

        public double temperature { get; set;}


        [Required]

        public double voltage { get; set;}


        [Required]

        public double electrical { get; set;}


        [Required]

        public double lumen { get; set;}


        [Required]

        public double lightstate { get; set;}


        [Required]

        public double angle { get; set;}


    }

}
#pragma warning restore 1591
