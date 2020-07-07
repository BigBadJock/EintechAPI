using Core.Common;
using System;

namespace Eintech.DataModels
{
    /// <summary>
    /// Customer Model
    /// </summary>
    public class Customer : BaseModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
