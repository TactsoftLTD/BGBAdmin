using FluentValidation.Attributes;

using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace IDIMAdmin.Models.User
{
	[Validator(typeof(ImageSlideVmValidator))]
    public class ImageSlideVm
    {
        public int ImageId { get; set; }
        [Display(Name = "Photo")]
        public string ImagePath { get; set; }
        public string Title { get; set; }
        public string AlternateText { get; set; }
        public Nullable<int> Priority { get; set; }
        public string Description { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
    }
}