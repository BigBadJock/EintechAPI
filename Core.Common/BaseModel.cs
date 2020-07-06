using Core.Common.Contracts;
using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Common
{
    public abstract class BaseModel : IModel
    {
        /// <summary>
        /// Unique Id
        /// </summary>
        /// <example>1</example>
        [Key]
        public int Id { get; set; }


        /// <summary>
        /// Date & Time entity created
        /// </summary>
        [Editable(false)]
        public DateTime Created { get; set; }

        /// <summary>
        /// Date & Time entity last updated
        /// </summary>
        [Editable(false)]
        public DateTime LastUpdated { get; set; }



    }
}
