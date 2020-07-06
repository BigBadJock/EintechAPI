using Core.Common;
using System;

namespace Eintech.DataModels
{
    /// <summary>
    /// Person Model
    /// </summary>
    public class Person : BaseModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
