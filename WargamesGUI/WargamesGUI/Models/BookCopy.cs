using System;
using System.Collections.Generic;
using System.Text;
using WargamesGUI.Services;
using WargamesGUI.Models;

namespace WargamesGUI.Models
{
    public class BookCopy
    {
        public int Copy_Id { get; set; }
        public int fk_Book_Id { get; set; }
        public int fk_Availability { get; set; }
        public int fk_Condition_Id { get; set; }
        public DateTime CopyCreated { get; set; }

        public BookCondition BookCondition { get; set; }
        public BookAvailability BookAvailability { get; set; }
        public Book2 Book { get; set; }


        public override string ToString()
        {
            return $"Exemplar ID: {Copy_Id}\nSkick: {BookCondition.ConditionType}\nStatus: {BookAvailability.Status}\n";
        }
    }
}
